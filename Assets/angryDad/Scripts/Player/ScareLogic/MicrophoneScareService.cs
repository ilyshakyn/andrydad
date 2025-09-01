using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.angryDad.Scripts.Player.ScareLogic
{
    public class MicrophoneScareService
    {
        public float CurrentScareRadius { get; private set; }

        private readonly float maxRadius;
        private readonly float volumeThreshold;

        public MicrophoneScareService(float maxRadius, float volumeThreshold)
        {
            this.maxRadius = maxRadius;
            this.volumeThreshold = volumeThreshold;
        }

        public void UpdateScare(float loudness, Vector3 sourcePosition)
        {
            float scaled = Mathf.Clamp01(loudness / volumeThreshold);
            CurrentScareRadius = scaled * maxRadius;
        }
    }
}
