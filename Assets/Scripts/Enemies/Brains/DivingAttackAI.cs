using System;
using Enemies.Behaviors;
using UnityEngine;

namespace Enemies
{
    public class DivingAttackAI : MonoBehaviour
    {
        public float hoverHeight, hoverMinDist, hoverTargDist, hoverMaxDist;
        public float flySpeed;
        public uint telegraphTime, attackCooldown;
        public Color telegraphColor;

        private EventWindow _telegraph, _cooldown;
        private MovementController _mc;
        private Rigidbody2D _rb;
        private SpriteRenderer _sr;
        private Color _origColor;
        private bool _attacking;

        private ContactFilter2D _filter;
        private Collider2D _collider;
        
        private void Awake()
        {
            _mc = GetComponent<MovementController>();
            _sr = GetComponent<SpriteRenderer>();
            _rb = GetComponent<Rigidbody2D>();
            _origColor = _sr.color;
            _telegraph = new EventWindow(telegraphTime, false);
            _cooldown = new EventWindow(attackCooldown, false);
            
            _filter.SetLayerMask(LayerMask.GetMask("Floor"));
            _collider = GetComponent<Collider2D>();
        }

        private void FixedUpdate()
        {
            _telegraph.Tick();
            _cooldown.Tick();

            var dist = PlayerController.Player.transform.position.x - transform.position.x;
            var yDist = transform.position.y - PlayerController.Player.transform.position.y;

            if (!_telegraph.isActive && !_attacking)
            {
                if (Mathf.Abs(hoverTargDist - Mathf.Abs(dist)) < 2) _mc.mode = MovementController.MoveMode.NONE;
                if (Mathf.Abs(dist) > hoverMaxDist) _mc.mode = MovementController.MoveMode.PLAYER;
                if (Mathf.Abs(dist) < hoverMinDist) _mc.mode = MovementController.MoveMode.PLAYER_AWAY;
                
                transform.localScale = Util.SetX(transform.localScale, Mathf.Abs(transform.localScale.x)*Mathf.Sign(dist));

                if (_collider.IsTouching(_filter))
                {
                    _rb.velocity = Util.SetY(_rb.velocity, -flySpeed);
                }
                else
                {
                    if (Math.Abs(hoverHeight-yDist) > 1.5)
                    {
                        _rb.velocity = Util.SetY(_rb.velocity, Math.Sign(hoverHeight-yDist) * flySpeed);
                    }
                    else
                    {
                        _rb.velocity = Util.SetY(_rb.velocity, 0);
                    }
                }
            }

            if (_mc.mode == MovementController.MoveMode.NONE && !_telegraph.isActive && !_cooldown.isActive && !_attacking)
            {
                _telegraph.Restart();
                _sr.color = telegraphColor;
            }

            if (_telegraph.isActive && _telegraph.time == 0)
            {
                _sr.color = _origColor;
                _attacking = true;
                _mc.mode = MovementController.MoveMode.PLAYER;
            }

            if (_attacking)
            {
                if (_mc.mode == MovementController.MoveMode.PLAYER)
                {
                    _rb.velocity = Util.SetY(_rb.velocity, yDist > 0.25 ? -flySpeed : 0);
                }
                else
                {
                    if (hoverTargDist - yDist < 0.25)
                    {
                        _rb.velocity = Util.SetY(_rb.velocity, 0);
                        _attacking = false;
                        _mc.mode = MovementController.MoveMode.NONE;
                        _cooldown.Restart();
                    }
                }
                
                if (Mathf.Abs(dist) < 1.25)
                {
                    _mc.mode = MovementController.MoveMode.PLAYER_AWAY;
                    _rb.velocity = Util.SetY(_rb.velocity, flySpeed);
                }
            }
        }
    }
}