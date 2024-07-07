using TMPro;
using UnityEngine;

namespace HUD
{
    public class HUDBar : MonoBehaviour
    {
        public void UpdatePercent(float percent)
        {
            transform.GetChild(0).localScale = new Vector3(2*percent, 1, 1);
        }

        public void UpdateText(int cur, int max)
        {
            UpdatePercent((float) cur/max);
            
            var text = GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = $"{cur}/{max}";
            }
        }
    }
}