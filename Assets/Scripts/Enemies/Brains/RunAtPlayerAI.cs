using System;
using Enemies.Behaviors;
using LevelManaging;
using TMPro;
using UnityEngine;

namespace Enemies
{
    public class RunAtPlayerAI : MonoBehaviour
    {
        public float jumpRange;
        public uint jumpDelay;

        private MovementController _mc;
        private EventWindow _jumpCooldown;

        private void Start()
        {
            _mc = GetComponent<MovementController>();

            _jumpCooldown = new EventWindow(jumpDelay, startActive: false);
        }

        private void FixedUpdate()
        {
            _jumpCooldown.Tick();
            
            var dist = PlayerController.Player.transform.position.x - transform.position.x;

            if (!_jumpCooldown.isActive && PlayerController.Player.IsJumping && Mathf.Abs(dist) <= jumpRange)
            {
                _mc.Jump();
                _jumpCooldown.Restart();
            }
        }
    }
}