using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCheck : MonoBehaviour
{
    public LayerMask floorMask;
    public bool isGrounded;

    private int _numFloors = 0;

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((floorMask & 1 << other.gameObject.layer) != 0)
        {
            isGrounded = (++_numFloors) > 0;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if ((floorMask & 1 << other.gameObject.layer) != 0)
        {
            isGrounded = (--_numFloors) > 0;
        }
    }
}
