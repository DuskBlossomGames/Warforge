using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using HUD;
using LevelManaging;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public int[] xpLevels;
    private int _upgradeLevel;
    private int _currentXp;

    public GameObject upgradeChoices;
    
    public HUDBar xpBar, healthBar;
    public float maxHealth;
    private float _curHealth;

    public uint knockbackFreezeTime;
    public float floorSpeed;
    public float currSpeed;
    public float accelSpeed;

    public Vector2 velocity { get { return _rb.velocity; } }

    public float gravity;
    public float jumpHeight;
    public float jumpShortHeight;
    public uint jumpPreBuffer;
    public uint jumpHighTime;
    private EventWindow _jumpPreBuffer;
    private EventWindow _jumpHold;
    private EventWindow _knockback;
    private float _knockbackDecel;
    private EventWindow _dashAtk = new(0);
    private EventWindow _jmpAtk = new(0);

    public float playerDir { get { return transform.localScale.x; } }

    public FloorCheck floorCheck;
    private Rigidbody2D _rb;
    private InputAction _moveAct;
    private InputAction _jumpAct;
    private bool _hasJumped;
    private float _jumpVel;
    private float _jumpShortDelta;
    
    public bool IsJumping { get; private set; }

    private void Awake()
    {
        _curHealth = maxHealth;
        
        _rb = GetComponent<Rigidbody2D>();
        _moveAct = InputSystem.actions.FindAction("Move");
        _jumpAct = InputSystem.actions.FindAction("Jump");
        _jumpVel = Mathf.Sqrt(2 * jumpHeight * gravity);
        _jumpShortDelta = Mathf.Sqrt(2 * jumpShortHeight * gravity) - _jumpVel;
        _jumpHold = new(0);

        _jumpPreBuffer = new EventWindow(jumpPreBuffer, startActive: false);
        _knockback = new EventWindow(knockbackFreezeTime, startActive: false);
    }

    private void FixedUpdate()
    {
        _jumpPreBuffer.Tick();
        _jumpHold.Tick();
        _knockback.Tick();
        _dashAtk.Tick();
        _jmpAtk.Tick();

        Vector2 movement = _moveAct.ReadValue<Vector2>();
        float lateral = movement.x;

        IsJumping &= !floorCheck.isGrounded;
        if (DidJump())
        {
            _jumpPreBuffer.RestartAt(jumpPreBuffer);
            IsJumping = true;
        } 
        if (floorCheck.isGrounded && _jumpPreBuffer.isActive) {
            _jumpPreBuffer.Zero();
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpVel);
            _jumpHold.RestartAt(jumpHighTime);
        }
        if (!_jumpAct.IsPressed() && _jumpHold.isActive)
        {
            _rb.velocity += _jumpShortDelta * _jumpHold.Percent() * Vector2.up;
            _jumpHold.Zero();
        }

        if (!_knockback.isActive && !_dashAtk.isActive)
        {
            float modAccelSpeed = currSpeed * floorSpeed * lateral == 0 ? accelSpeed / 3 : accelSpeed;
            currSpeed = Mathf.Lerp(currSpeed, floorSpeed * lateral, modAccelSpeed);
        }
        else if (_knockback.isActive)
        {
            currSpeed -= _knockbackDecel * Time.fixedDeltaTime;
        } 
        transform.localScale = Util.SetX(transform.localScale, lateral > 0 ? 1 : lateral < 0 ? -1 : playerDir);

        _rb.velocity = new Vector2(currSpeed, _rb.velocity.y);

        if (_jmpAtk.hasEnded)
        {
            _rb.velocity -= new Vector2(0, gravity) * Time.fixedDeltaTime;
        }
    }

    /* Sync across video frames and engine frames */
    private void Update()
    {
        if (_jumpAct.WasPressedThisFrame())
        {
            _hasJumped = true;
        }
    }

    bool DidJump()
    {
        if (_hasJumped)
        {
            _hasJumped = false;
            return true;
        }
        return false;
    }

    public void GainXp(int xp)
    {
        _currentXp += xp;
        if (xpLevels[_upgradeLevel] <= _currentXp)
        {
            _currentXp -= xpLevels[_upgradeLevel++];
            
            _curHealth = maxHealth;
            healthBar.UpdatePercent(0);
            
            StartCoroutine(ChooseUpgrade());
        }
        
        xpBar.UpdateText(_currentXp, xpLevels[_upgradeLevel]);
    }

    public IEnumerator ChooseUpgrade()
    {
        PauseManager.Freeze();
        yield return 0;
        
        currSpeed = 0;
        gameObject.transform.position = new Vector3();
        upgradeChoices.SetActive(true);

        var buttons = upgradeChoices.GetComponentsInChildren<Button>();
        
        Action select = null;
        select = () =>
        {
            PauseManager.Unfreeze();
            upgradeChoices.SetActive(false);

            foreach (var button in buttons)
            {
                button.OnClick -= select;
            }
        };
        
        foreach (var button in buttons)
        {
            button.OnClick += select;
        }
    }

    public void Damage(float damage)
    {
        if ((_curHealth -= damage) < 0)
        {
            Destroy(gameObject);
        }
        
        healthBar.UpdatePercent(1-_curHealth/maxHealth);
    }

    public void Knockback(float xDist, float yDist)
    {
        if (_dashAtk.isActive) return;
        IsJumping = false;

        _knockbackDecel = 2 * xDist / Mathf.Pow(knockbackFreezeTime * Time.fixedDeltaTime, 2);
        
        currSpeed = Mathf.Sign(xDist)*Mathf.Sqrt(2 * Math.Abs(_knockbackDecel * xDist));
        _rb.velocity = _rb.velocity * Vector2.right + Mathf.Sqrt(2 * yDist * gravity) * Vector2.up;
        _knockback.Restart();
    }

    public void Dash(uint frames)
    {
        _dashAtk.RestartAt(frames);
    }

    public void NoGrav(uint frames)
    {
        _jmpAtk.RestartAt(frames);
    }

    public void SetXVel(float xvel)
    {
        currSpeed = xvel;
        _rb.velocity = new Vector2(xvel, _rb.velocity.y);
    }

    public void SetYVel(float yvel)
    {
        _rb.velocity = new Vector2(_rb.velocity.x, yvel);
    }
}
