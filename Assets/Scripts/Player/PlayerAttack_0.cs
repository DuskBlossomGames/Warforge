using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack_0 : MonoBehaviour
{
    public GameObject atkObj;
    public uint atkTime;
    public uint baseDmg;

    private EventWindow _atkTimer = new(0);
    private bool _isAttacking;
    private ColliderAccum _atkCollider;

    private void Awake()
    {
        _atkCollider = atkObj.GetComponent<ColliderAccum>();
    }

    public void Attack()
    {
        if (!_isAttacking)
        {
            _isAttacking = true;
            _atkTimer.RestartAt(atkTime);
            atkObj.GetComponent<SpriteRenderer>().enabled = true;
            foreach (var target in _atkCollider.GetColliders())
            {
                //Debug.LogFormat("Found collider with name: {0}", target.gameObject.name);
                if (target.TryGetComponent<EnemyHealth>(out var comp))
                {
                    comp.Damage((uint)Mathf.FloorToInt((float)baseDmg * Random.Range(.85f, 1.15f)));
                }
            }
            
        }
    }

    private void FixedUpdate()
    {
        _atkTimer.Tick();
        if (_isAttacking)
        {
            _isAttacking = _atkTimer.isActive;
            if (!_isAttacking)
            {
                atkObj.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }
}
