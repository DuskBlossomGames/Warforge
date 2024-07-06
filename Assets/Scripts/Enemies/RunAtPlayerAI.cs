using System;
using TMPro;
using UnityEngine;

namespace Enemies
{
    public class RunAtPlayerAI : MonoBehaviour
    {
        public float maxSpeed;
        public uint knockbackFreezeTime;
        public float jumpHeight;
        public float jumpRange;
        public uint jumpDelay;
        public float accel;
        public PlayerController player;

        private float _curSpeed;
        private float _jumpVel;
        private Rigidbody2D _rb;
        private FloorCheck _fc;
        private EventWindow _jumpCooldown;
        private EventWindow _knockback;
        private float _knockbackDecel;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _fc = GetComponentInChildren<FloorCheck>();
            _jumpVel = Mathf.Sqrt(2 * jumpHeight * player.gravity);

            _jumpCooldown = new EventWindow(jumpDelay, startActive: false);
            _knockback = new EventWindow(knockbackFreezeTime, startActive: false);
        }

        private void FixedUpdate()
        {
            _jumpCooldown.Tick();
            _knockback.Tick();
            
            var dist = player.transform.position.x - transform.position.x;

            if (!_jumpCooldown.isActive && player.IsJumping && _fc.isGrounded && Mathf.Abs(dist) <= jumpRange)
            {
                _rb.velocity += _jumpVel * Vector2.up;
                _jumpCooldown.Restart();
            }

            if (!_knockback.isActive)
            {
                _curSpeed = Mathf.Lerp(_curSpeed, Mathf.Sign(dist) * maxSpeed, accel);
            }
            else
            {
                _curSpeed -= _knockbackDecel * Time.fixedDeltaTime;
            }
            
            _rb.velocity = new Vector2(_curSpeed, _rb.velocity.y);
            _rb.velocity -= new Vector2(0, player.gravity) * Time.fixedDeltaTime;
        }
        
        public void Knockback(float xDist, float yDist)
        {
            _knockbackDecel = 2 * xDist / Mathf.Pow(knockbackFreezeTime * Time.fixedDeltaTime, 2);
        
            _curSpeed = Mathf.Sign(xDist)*Mathf.Sqrt(2 * Math.Abs(_knockbackDecel * xDist));
            _rb.velocity = _curSpeed * Vector2.right + Mathf.Sqrt(2 * yDist * player.gravity) * Vector2.up;
            _knockback.Restart();
        }
    }
}