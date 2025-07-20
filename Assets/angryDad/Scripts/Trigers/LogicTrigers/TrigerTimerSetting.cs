using System.Collections;
using UnityEngine;

namespace Assets.angryDad.Scripts.Trigers.LogicTrigers
{
    [CreateAssetMenu(fileName = "TriggerSettings", menuName = "Triggers/Trigger Settings")]
    public class TrigerTimerSetting : ScriptableObject
    {
        public float minDelay = 5f;
        public float maxDelay = 10f;
    }
}