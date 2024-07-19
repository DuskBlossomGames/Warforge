using System;
using System.Collections;
using System.Collections.Generic;
using Enemies.Behaviors;
using UnityEngine;

namespace DefaultNamespace.Player
{
    public class BulletController : MonoBehaviour
    {
        private bool _active;
        
        private float _xKb, _yKb;
        private uint _dmg;
        private uint _pierce;
        
        private ColliderAccum _ca;
        private Rigidbody2D _rb;

        private EventWindow _rangeDestroy;

        private void Awake()
        {
            _rangeDestroy = new EventWindow(0, false);
            _rb = GetComponent<Rigidbody2D>();
            _ca = GetComponent<ColliderAccum>();
        }

        public void Shoot(float vel, float kb, uint dmg, float range, uint pierce)
        {
            var angle = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
            _rb.velocity = new Vector2(vel * Mathf.Cos(angle), vel * Mathf.Sin(angle));

            _xKb = kb * Mathf.Cos(angle);
            _yKb = kb * Mathf.Sin(angle);
            
            _dmg = dmg;
            _pierce = pierce;

            _rangeDestroy.RestartAt((uint) (range / (vel * Time.fixedDeltaTime)));
            
            _active = true;
        }

        private void FixedUpdate()
        {
            if (!_active) return;
         
            _rangeDestroy.Tick();
            if (_rangeDestroy.hasEnded)
            {
                Destroy(gameObject);
                return;
            }
            
            foreach (var target in _ca.GetColliders())
            {
                if (target.gameObject.layer == LayerMask.NameToLayer("Floor"))
                {
                    Destroy(gameObject);
                    return;
                }
                
                if (target.transform.parent.TryGetComponent<MovementController>(out var mc))
                {
                    mc.Knockback(_xKb, _yKb);
                }

                if (target.transform.parent.TryGetComponent<EnemyInfo>(out var ei))
                {
                    ei.Damage(_dmg);
                }

                if (_pierce-- == 0)
                {
                    Destroy(gameObject);
                    return;
                }
            }
        }
    }
}