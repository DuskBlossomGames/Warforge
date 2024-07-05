using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack_0 : MonoBehaviour
{
    public GameObject atkObj;
    public uint atkTime;

    private EventWindow _atkTimer = new(0);
    private bool _isAttacking;
    private Collider2D _atkCollider;

    private void Awake()
    {
        _atkCollider = atkObj.GetComponent<Collider2D>();
    }

    public void Attack()
    {
        if (!_isAttacking)
        {
            _isAttacking = true;
            _atkTimer.RestartAt(atkTime);
            atkObj.SetActive(true);
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
                atkObj.SetActive(false);
            }
        }
    }
}
