using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgNumFloat : MonoBehaviour
{
    private float _currLife;
    private float _maxLife;
    private float _riseSpeed;
    public void Spawn(float maxlife, float riseSpeed)
    {
        _maxLife = maxlife;
        _riseSpeed = riseSpeed;
    }

    private void Update()
    {
        _currLife += Time.deltaTime;
        if(_currLife > _maxLife)
        {
            Destroy(gameObject);
            return;
        }

        ((RectTransform)transform).anchoredPosition += new Vector2(0, _riseSpeed * Time.deltaTime);
    }
}
