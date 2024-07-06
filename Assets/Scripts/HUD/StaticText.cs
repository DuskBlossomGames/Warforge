using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticText : MonoBehaviour
{
    private Vector2 _pos;

    private void Awake()
    {
        _pos = Camera.main.ScreenToWorldPoint(transform.position);
    }
    void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(_pos);
    }
}
