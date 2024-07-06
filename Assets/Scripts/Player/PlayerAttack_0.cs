using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class PlayerAttack_0 : MonoBehaviour
{
    public GameObject atkObj;
    public uint atkTime;
    public uint baseDmg;
    public float xKb, yKb;
    public float yvelDec;
    public AnimationCurve dashPosition;

    private EventWindow _atkTimer = new(0);
    private bool _isAttacking;
    private ColliderAccum _atkCollider;
    private PlayerController _controller;
    private float _prevXpos;

    private void Awake()
    {
        _atkCollider = atkObj.GetComponent<ColliderAccum>();
        _controller = GetComponent<PlayerController>();
    }

    public void Attack()
    {
        if (!_isAttacking)
        {
            _isAttacking = true;
            _atkTimer.RestartAt(atkTime);
            _controller.Dash(atkTime);
            _controller.NoGrav(atkTime);
            _prevXpos = 0;
            atkObj.GetComponent<SpriteRenderer>().enabled = true;
            foreach (var target in _atkCollider.GetColliders())
            {
                //Debug.LogFormat("Found collider with name: {0}", target.gameObject.name);
            }
            
        }
    }

    private void FixedUpdate()
    {
        _atkTimer.Tick();
        if (_isAttacking)
        {
            float newXpos = dashPosition.Evaluate(1 - _atkTimer.Percent());
            float vel = _controller.playerDir * (newXpos - _prevXpos) / Time.fixedDeltaTime;
            _controller.SetXVel(vel);
            _controller.SetYVel(_controller.velocity.y * yvelDec);

            foreach (var target in _atkCollider.GetColliders())
            {
                if (target.transform.parent.TryGetComponent<RunAtPlayerAI>(out var ai))
                {
                    ai.Knockback(Mathf.Sign(target.transform.position.x - transform.position.x) * xKb, 0);
                }

                if (target.transform.TryGetComponent<CollisionDamage>(out var cd))
                {
                    cd.StunFor(25);
                }
            }

            _isAttacking = _atkTimer.isActive;
            if (!_isAttacking)
            {
                atkObj.GetComponent<SpriteRenderer>().enabled = false;
                foreach (var target in _atkCollider.GetColliders())
                {
                    if (target.transform.parent.TryGetComponent<EnemyInfo>(out var comp))
                    {
                        comp.Damage((uint)Mathf.FloorToInt((float)baseDmg * Random.Range(.85f, 1.15f)));
                        if (target.transform.parent.TryGetComponent<RunAtPlayerAI>(out var ai))
                        {
                            ai.Knockback(Mathf.Sign(target.transform.position.x - transform.position.x) * xKb, yKb);
                        }
                    }
                }
            }
        }
    }
}
