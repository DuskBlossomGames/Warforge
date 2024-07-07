using System;
using Enemies.Behaviors;
using UnityEngine;

namespace Enemies
{
    public class BasicAttackAI : MonoBehaviour
    {
        public float attackRange;
        public uint telegraphTime;
        public Color telegraphColor;

        private MeleeAttack _ma;
        private MovementController _mc;
        private SpriteRenderer _sr;
        private EventWindow _telegraph;

        private Color _origColor;

        private void Awake()
        {
            _ma = GetComponent<MeleeAttack>();
            _mc = GetComponent<MovementController>();
            _sr = GetComponent<SpriteRenderer>();
            _origColor = _sr.color;
            _telegraph = new EventWindow(telegraphTime, false);
        }

        private void FixedUpdate()
        {
            _telegraph.Tick();
            
            var dist = Mathf.Abs(PlayerController.Player.transform.position.x - transform.position.x);
            _mc.mode = dist < attackRange
                ? MovementController.MoveMode.NONE
                : MovementController.MoveMode.PLAYER;

            if (_telegraph.isActive)
            {
                if (_telegraph.time == 0)
                {
                    _sr.color = _origColor;
                    SetLayerOrder(-25);
                    _ma.Attack();
                }
            } else if (dist < attackRange && !_ma.IsAttacking)
            {
                _telegraph.Restart();
                _sr.color = telegraphColor;
                SetLayerOrder(25);
            }
        }

        private void SetLayerOrder(int diff)
        {
            foreach (var sr in GetComponentsInChildren<SpriteRenderer>())
            {
                sr.sortingOrder += diff;
            }
        }
    }
}