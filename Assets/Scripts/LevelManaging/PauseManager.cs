using System;
using UnityEngine;

namespace LevelManaging
{
    public class PauseManager : MonoBehaviour
    {
        public static bool Frozen { get; private set; }
        
        private static SpriteRenderer _sr;

        private void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();
        }

        public static void Freeze()
        {
            Frozen = _sr.enabled = true;
            Time.timeScale = 0;
        }

        public static void Unfreeze()
        {
            Frozen = _sr.enabled = false;
            Time.timeScale = 1;
        }
    }
}