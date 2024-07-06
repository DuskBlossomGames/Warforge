using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    public uint maxhealth;
    public uint currhealth;
    public uint xp;
    public PlayerController player;
    public uint resistance;

    private static float _kResCoef = Mathf.Pow(Mathf.PI, 10f); //Dont ask why, but this makes the math work
    private DmgNumInstancer _dmgVis;

    private void Awake()
    {
        _dmgVis = Camera.main.GetComponent<DmgNumInstancer>();
    }

    public void Damage(uint dmg)
    {
        float inDmg = dmg;
        float truedmg = dmg * dmg / Mathf.Pow(Mathf.Pow(inDmg, 10f) + _kResCoef * Mathf.Pow(resistance, 10f), .1f);
        uint finalDmg = (uint)-Mathf.FloorToInt(-truedmg);

        if (finalDmg >= currhealth)
        {
            OnDeath();
            currhealth = 0;
        }
        else 
        {
            currhealth -= finalDmg;
        }
        //Debug.LogFormat("_dmgVis = {0}", _dmgVis);
        _dmgVis.Spawn((int)finalDmg, transform.position);
        
    }

    void OnDeath()
    {
        player.GainXp((int) xp);
        Destroy(gameObject);
    }
}
