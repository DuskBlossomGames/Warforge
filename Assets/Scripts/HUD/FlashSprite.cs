using System;
using UnityEngine;

namespace HUD
{
    public class FlashSprite : MonoBehaviour
    {
        public uint cycleFrames;

        private EventWindow _cycle;
        private SpriteRenderer _sr;

        private void Awake()
        {
            _cycle = new EventWindow(cycleFrames);
            _sr = GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            _cycle.Tick();
            if (_cycle.hasEnded) _cycle.Restart();

            _sr.color = new Color(_sr.color.r, _sr.color.g, _sr.color.b, Math.Abs(_cycle.Percent() * 2 - 1));
        }
    }
}