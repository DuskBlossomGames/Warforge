using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgNumInstancer : MonoBehaviour
{
    public float riseSpeed;
    public float maxlife;
    public int textSize;
    public GameObject textPrefab;
    public Transform canvas;

    public void Spawn(int num, Vector2 loc)
    {
        // var screenLoc = Camera.main.WorldToScreenPoint(loc);
        var text = Instantiate(textPrefab, loc, Quaternion.identity, canvas);
        text.GetComponent<DmgNumFloat>().Spawn(maxlife, riseSpeed);
        text.GetComponent<UnityEngine.UI.Text>().fontSize = textSize;
        text.GetComponent<UnityEngine.UI.Text>().text = num.ToString();
    }
}
