using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTrack : MonoBehaviour
{
    public Transform target;
    public Vector2 camOff = new Vector2(0, 5);
    public bool isFloat;
    private Vector2 _avgDelta;

    private void Update()
    {
        if (isFloat)
        {
            camOff = new Vector2(0, 5);
        }
        else
        {
            camOff = new Vector2(0, Mathf.Max(0, 1 - target.position.y));
        }
        camOff -= _avgDelta;

        float lerpamt = Mathf.Pow(.05f, Time.deltaTime);
        transform.position = (Vector3)(camOff + (((Vector2)transform.position - camOff) * lerpamt + (Vector2)target.position*(1 - lerpamt))) - 10 * Vector3.forward;
        var delta = (Vector2)transform.position - (Vector2)target.position;
        camOff += _avgDelta;
        delta -= camOff;

        _avgDelta = delta * lerpamt + _avgDelta * (1 - lerpamt);
    }
}
