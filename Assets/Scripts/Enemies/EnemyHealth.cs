using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public uint maxhealth;
    public uint currhealth;
    public uint resistance;

    private static float _kResCoef = Mathf.Pow(Mathf.PI, 10f); //Dont ask why, but this makes the math work

    public void Damage(uint dmg)
    {
        float inDmg = dmg;
        float truedmg = dmg * dmg / Mathf.Pow(Mathf.Pow(inDmg, 10f) + _kResCoef * Mathf.Pow(resistance, 10f), .1f);
        uint finalDmg = (uint)-Mathf.RoundToInt(-truedmg) + 1;

        currhealth -= finalDmg;
        if(currhealth <= 0)
        {
            OnDeath();
        }
    }

    void OnDeath()
    {
        Destroy(gameObject);
    }
}
