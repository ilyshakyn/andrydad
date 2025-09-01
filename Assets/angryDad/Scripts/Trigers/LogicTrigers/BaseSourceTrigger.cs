using Assets.angryDad.Scripts.Trigers;
using Assets.angryDad.Scripts.Trigers.Interface;
using Assets.angryDad.Scripts.Trigers.LogicTrigers;
using Assets.angryDad.Scripts.Trigers.LogicTrigers.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class  BaseSourceTrigger : MonoBehaviour, ITriggerTimerTarget, ITriggerTimerConfigurable
{

    public bool isdestroy;

    public event Action<float> OnTriggered;

    private ITriggerDamageReceiver _receiver;
    private TriggerRestartEventBus _restartBus;

    [SerializeField] private int _angerAmount = 10;
    [SerializeField] private TrigerTimerSetting _settings;

    [Header("«она урона (дочерний объект со скриптом TriggerZoneSensor)")]
    [SerializeField] private TriggerZoneSensor _zoneSensor;

    public int AngerAmount => _angerAmount;
    public bool IsActive { get; private set; } = true;
    public TrigerTimerSetting GetTriggerSettings() => _settings;

    private bool _playerInside = false;

    protected virtual void Start()
    {
        // ѕолучим при старте получател€ урона (игрок может по€витьс€ чуть позже Ч не критично)
        var playerObj = GameObject.FindWithTag("Player");
        _receiver = playerObj?.GetComponent<ITriggerDamageReceiver>();

        if (_zoneSensor == null)
        {
            _zoneSensor = GetComponentInChildren<TriggerZoneSensor>();
            if (_zoneSensor == null)
                Debug.LogError($"{name}: TriggerZoneSensor (зона урона) не назначен!");
        }

        if (_zoneSensor != null)
        {
            _zoneSensor.PlayerEntered += () => _playerInside = true;
            _zoneSensor.PlayerExited += () => _playerInside = false;
        }
    }

    protected virtual void OnEnable()
    {
        TriggerTimerRegistry.Register(this);
        if (TriggerTimerManager.Instance != null)
            TriggerTimerManager.Instance.RegisterNewTrigger(this);
    }

    protected virtual void OnDisable()
    {
        TriggerTimerRegistry.Unregister(this);
        if (TriggerTimerManager.Instance != null)
            TriggerTimerManager.Instance._controller.Unregister(this);
    }

    public virtual void Trigger()
    {
        if (!IsActive) return;
        if (!_playerInside) return; // ключева€ проверка Ч урон только в зоне

        OnTriggered?.Invoke(_angerAmount);
        _receiver?.ApplyTriggerDamage(_angerAmount);
        // Debug.Log($"{name}: Tick damage {_angerAmount}");
    }

    public void InjectRestartBus(TriggerRestartEventBus bus) => _restartBus = bus;

    public virtual void TurnOn()
    {
        IsActive = true;
        // Debug.Log($"{name}: Turned ON");
    }

    public virtual void TurnOff()
    {
        IsActive = false;
        // Debug.Log($"{name}: Turned OFF");
        _restartBus?.RequestRestart(this);
    }
}