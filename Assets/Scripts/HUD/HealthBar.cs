using UnityEngine;

namespace HUD
{
    public class HealthBar : MonoBehaviour
    {
        public void UpdatePercent(float percent)
        {
            transform.GetChild(0).localScale = new Vector3(2*(1-percent), 1, 1);
        }
    }
}