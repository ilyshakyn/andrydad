using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.angryDad.Scripts.Player.ScareLogic
{
    public class MicrophoneVolumeService
    {
        public float Volume { get; private set; }

        private AudioClip _micClip;
        private const int sampleWindow = 64;
        private string _deviceName;
        private int _sampleRate = 44100;

        public void StartRecording()
        {
            _deviceName = Microphone.devices.Length > 0 ? Microphone.devices[0] : null;
            if (_deviceName == null)
            {
                Debug.LogError("No microphone found!");
                return;
            }

            _micClip = Microphone.Start(_deviceName, true, 1, _sampleRate);
        }

        public void StopRecording()
        {
            if (_deviceName != null)
                Microphone.End(_deviceName);
        }

        public void Update()
        {
            if (_micClip == null) return;

            float[] samples = new float[sampleWindow];
            int micPos = Microphone.GetPosition(_deviceName) - sampleWindow + 1;
            if (micPos < 0) return;

            _micClip.GetData(samples, micPos);
            float max = 0f;
            foreach (var s in samples)
            {
                float abs = Mathf.Abs(s);
                if (abs > max) max = abs;
            }

            Volume = max;
        }
    }
}