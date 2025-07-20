using System;
using System.Collections;
using UnityEngine;

namespace Assets.angryDad.Scripts.Trigers.LogicTrigers
{
    public class TriggerRestartEventBus 
    {

        public event Action<ITriggerTimerTarget> OnRestartRequested;

        public void RequestRestart(ITriggerTimerTarget source)
        {
            OnRestartRequested?.Invoke(source);
        }
    }
}
