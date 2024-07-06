using System;
using TMPro;
using UnityEngine;

namespace Enemies
{
    public class RunAtPlayerAI : MonoBehaviour
    {
        public float maxSpeed;
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

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _fc = GetComponentInChildren<FloorCheck>();
            _jumpVel = Mathf.Sqrt(2 * jumpHeight * player.gravity);

            _jumpCooldown = new EventWindow(jumpDelay, startActive: false);
        }

        private void FixedUpdate()
        {
            _jumpCooldown.Tick();
            
            var dist = player.transform.position.x - transform.position.x;

            _curSpeed = Mathf.Lerp(_curSpeed, Mathf.Sign(dist) * maxSpeed, accel);
            if (!_jumpCooldown.isActive && player.IsJumping && _fc.isGrounded && Mathf.Abs(dist) <= jumpRange)
            {
                _rb.velocity += _jumpVel * Vector2.up;
                _jumpCooldown.Restart();
            }
            
            _rb.velocity = new Vector2(_curSpeed, _rb.velocity.y);
            _rb.velocity -= new Vector2(0, player.gravity) * Time.fixedDeltaTime;
        }
    }
}