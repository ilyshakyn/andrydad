using Assets.angryDad.Scripts.Trigers.Interface;
using Assets.angryDad.Scripts.Trigers.LogicTrigers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


public class TriggerTimerController
{


    private readonly MonoBehaviour _context;
    private readonly float _defaultMinDelay = 3f;
    private readonly float _defaultMaxDelay = 6f;

    private readonly Dictionary<ITriggerTimerTarget, Coroutine> _coroutines = new();

    public TriggerTimerController(MonoBehaviour context)
    {
        _context = context;
    }

    public void Register(ITriggerTimerTarget target)
    {
        if (_coroutines.ContainsKey(target)) return;

        Coroutine c = _context.StartCoroutine(TriggerLoop(target));
        _coroutines[target] = c;
    }

    public void RestartWithDelay(ITriggerTimerTarget target)
    {
        if (_coroutines.TryGetValue(target, out Coroutine existing))
        {
            _context.StopCoroutine(existing);
        }

        Coroutine c = _context.StartCoroutine(WaitAndRestart(target));
        _coroutines[target] = c;
    }

    private IEnumerator WaitAndRestart(ITriggerTimerTarget target)
    {
        float delay = GetDelayFromTarget(target);
        yield return new WaitForSeconds(delay);

        target.TurnOn();
        if (target.IsActive)
            target.Trigger();

        Coroutine loop = _context.StartCoroutine(TriggerLoop(target));
        _coroutines[target] = loop;
    }

    private IEnumerator TriggerLoop(ITriggerTimerTarget target)
    {
        while (true)
        {
            float delay = GetDelayFromTarget(target);
            yield return new WaitForSeconds(delay);

            target.TurnOn();
            if (target.IsActive)
                target.Trigger();
        }
    }

    private float GetDelayFromTarget(ITriggerTimerTarget target)
    {
        if (target is ITriggerTimerConfigurable configurable)
        {
            var settings = configurable.GetTriggerSettings();
            return Random.Range(settings.minDelay, settings.maxDelay);
        }
        return Random.Range(_defaultMinDelay, _defaultMaxDelay);
    }

    public bool IsRegistered(ITriggerTimerTarget target) => _coroutines.ContainsKey(target);

    public void StopAll()
    {
        foreach (var c in _coroutines.Values)
            _context.StopCoroutine(c);

        _coroutines.Clear();
    }
}