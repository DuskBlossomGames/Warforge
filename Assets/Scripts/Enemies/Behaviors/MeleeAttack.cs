using System;
using UnityEngine;

namespace Enemies.Behaviors
{
    public class MeleeAttack : MonoBehaviour
    {
        public bool IsAttacking => _atkTimer.isActive;
        
        public GameObject hitbox;
        public float damage;
        public uint time;
        public float xKb, yKb;

        private EventWindow _atkTimer;
        private ColliderAccum _ca;
        private SpriteRenderer _sr;

        private void Awake()
        {
            _ca = hitbox.GetComponent<ColliderAccum>();
            _sr = hitbox.GetComponent<SpriteRenderer>();
            _atkTimer = new EventWindow(time, false);
        }

        public void Attack()
        {
            _atkTimer.Restart();
            
            foreach (var col in _ca.GetColliders())
            {
                var player = col.GetComponentInParent<PlayerController>();
                if (player == null) continue;
                
                player.Damage(damage);
                player.Knockback(xKb, yKb);
            }
        }

        private void FixedUpdate()
        {
            _atkTimer.Tick();

            _sr.enabled = _atkTimer.isActive;
        }
    }
}