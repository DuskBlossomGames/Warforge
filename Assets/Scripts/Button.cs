using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Button : MonoBehaviour
    {
        public event Action OnClick;

        private void OnMouseUpAsButton()
        {
            OnClick?.Invoke();
        }
    }
}