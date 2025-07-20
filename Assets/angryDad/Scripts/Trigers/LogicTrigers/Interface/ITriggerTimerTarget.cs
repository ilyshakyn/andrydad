using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

 public interface ITriggerTimerTarget
 {
    event Action<float> OnTriggered;

    int AngerAmount { get; }
    bool IsActive { get; }

    void TurnOn();
    void TurnOff();
    void Trigger();
}
