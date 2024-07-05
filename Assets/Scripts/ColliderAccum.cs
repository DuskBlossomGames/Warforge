using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderAccum : MonoBehaviour
{
    private List<Collider2D> _colliders = new();
    public List<Collider2D> GetColliders()
    {
        return new(_colliders);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _colliders.Add(other);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        _colliders.Remove(other);
    }

}
