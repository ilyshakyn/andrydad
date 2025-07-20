using Assets.angryDad.Scripts.Trigers;
using Assets.angryDad.Scripts.Trigers.Interface;
using Assets.angryDad.Scripts.Trigers.LogicTrigers;
using Assets.angryDad.Scripts.Trigers.LogicTrigers.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class BaseSourceTrigger : MonoBehaviour, ITriggerTimerTarget, ITriggerTimerConfigurable
{
    public event Action<float> OnTriggered;

    private ITriggerDamageReceiver _receiver;
    private TriggerRestartEventBus _restartBus;

    [SerializeField] private int _angerAmount = 10;
    [SerializeField] private TrigerTimerSetting _settings;

    public int AngerAmount => _angerAmount;
    public bool IsActive { get; private set; } = true;

    public TrigerTimerSetting GetTriggerSettings() => _settings;

    private void Start()
    {
        _angerAmount = Random.Range(0, 10);
        var playerObj = GameObject.FindWithTag("Player");
        _receiver = playerObj?.GetComponent<ITriggerDamageReceiver>();

        if (_receiver == null)
            Debug.LogWarning($"{name}: Player receiver not found!");

        if (TriggerTimerManager.Instance != null)
            TriggerTimerManager.Instance.RegisterNewTrigger(this);
    }

    public virtual void Trigger()
    {
        if (!IsActive) return;
        Debug.Log($"{name}: Triggered with anger {_angerAmount}");
        OnTriggered?.Invoke(_angerAmount);
        _receiver?.ApplyTriggerDamage(_angerAmount);
    }

    public void InjectRestartBus(TriggerRestartEventBus bus)
    {
        _restartBus = bus;
    }

    public virtual void TurnOn()
    {
        IsActive = true;
        Debug.Log($"{name}: Turned ON");
    }

    public virtual void TurnOff()
    {
        IsActive = false;
        Debug.Log($"{name}: Turned OFF");
        _restartBus?.RequestRestart(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            TurnOff();
    }
}