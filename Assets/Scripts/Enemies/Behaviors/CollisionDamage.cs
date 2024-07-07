using System;
using UnityEngine;

namespace Enemies
{
    public class CollisionDamage : MonoBehaviour
    {
        public float damage;
        public float xKb, yKb;

        private EventWindow _hitstun = new(0);

        private void Awake()
        {
            _hitstun.AutoTick();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_hitstun.isActive) return;
            if (other.gameObject.layer != LayerMask.NameToLayer("Player Hitbox")) return;
            
            var player = other.GetComponentInParent<PlayerController>();
            player.Knockback(transform.position.x < player.transform.position.x ? xKb : -xKb, yKb);
            player.Damage(damage);
        }

        public void StunFor(uint frames)
        {
            _hitstun.RestartAt(frames);
        }
    }
}