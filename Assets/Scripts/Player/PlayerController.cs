using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float floorSpeed;
    public float currSpeed;
    public float accelSpeed;

    public float gravity;
    public float jumpHeight;
    public float jumpShortHeight;
    public uint jumpPreBuffer;
    public uint jumpHighTime;
    private EventWindow _jumpPreBuffer;
    private EventWindow _jumpHold;

    public FloorCheck floorCheck;
    private Rigidbody2D _rb;
    private InputAction _moveAct;
    private InputAction _jumpAct;
    private bool _hasJumped;
    private float _jumpVel;
    private float _jumpShortDelta;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _moveAct = InputSystem.actions.FindAction("Move");
        _jumpAct = InputSystem.actions.FindAction("Jump");
        _jumpVel = Mathf.Sqrt(2 * jumpHeight * gravity);
        _jumpShortDelta = Mathf.Sqrt(2 * jumpShortHeight * gravity) - _jumpVel;
        _jumpHold = new(0);

        _jumpPreBuffer = new EventWindow(jumpPreBuffer, startActive: false);
    }

    private void FixedUpdate()
    {
        _jumpPreBuffer.Tick();
        _jumpHold.Tick();

        Vector2 movement = _moveAct.ReadValue<Vector2>();
        float lateral = movement.x;

        if (DidJump())
        {
            _jumpPreBuffer.RestartAt(jumpPreBuffer);
        } 
        if (floorCheck.isGrounded && _jumpPreBuffer.isActive) {
            _jumpPreBuffer.Zero();
            _rb.velocity = new Vector2(0, _jumpVel);
            _jumpHold.RestartAt(jumpHighTime);
        }
        if (!_jumpAct.IsPressed() && _jumpHold.isActive)
        {
            _rb.velocity += _jumpShortDelta * _jumpHold.Percent() * Vector2.up;
            _jumpHold.Zero();
        }

        currSpeed = Mathf.Lerp(currSpeed, floorSpeed * lateral, accelSpeed);
        transform.localScale = Util.SetX(transform.localScale, lateral > 0 ? 1 : lateral < 0 ? -1 : transform.localScale.x);

        _rb.velocity = new Vector2(currSpeed, _rb.velocity.y);
        _rb.velocity -= new Vector2(0, gravity) * Time.fixedDeltaTime;
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
}
