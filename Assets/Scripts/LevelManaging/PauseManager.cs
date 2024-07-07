using System;
using UnityEngine;

namespace LevelManaging
{
    public class PauseManager : MonoBehaviour
    {

        private static SpriteRenderer _sr;

        private void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();
        }

        public static void Freeze()
        {
            _sr.enabled = true;
            Time.timeScale = 0;
        }

        public static void Unfreeze()
        {
            _sr.enabled = false;
            Time.timeScale = 1;
        }
    }
}