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

    private readonly Dictionary<ITriggerTimerTarget, Coroutine> _restartCoroutines = new();
    private readonly Dictionary<ITriggerTimerTarget, Coroutine> _tickCoroutines = new();

    public TriggerTimerController(MonoBehaviour context) => _context = context;

    public void Register(ITriggerTimerTarget target)
    {
        if (_restartCoroutines.ContainsKey(target)) return;
        // первичный запуск после initial delay
        var c = _context.StartCoroutine(InitialStart(target));
        _restartCoroutines[target] = c;
    }

    public void RestartWithDelay(ITriggerTimerTarget target)
    {
        StopTick(target);
        StopRestart(target);
        var c = _context.StartCoroutine(RestartAfterOff(target));
        _restartCoroutines[target] = c;
    }

    public void Unregister(ITriggerTimerTarget target)
    {
        StopTick(target);
        StopRestart(target);
    }

    public bool IsRegistered(ITriggerTimerTarget target) =>
        _restartCoroutines.ContainsKey(target) || _tickCoroutines.ContainsKey(target);

    public void StopAll()
    {
        foreach (var c in _tickCoroutines.Values) _context.StopCoroutine(c);
        foreach (var c in _restartCoroutines.Values) _context.StopCoroutine(c);
        _tickCoroutines.Clear();
        _restartCoroutines.Clear();
    }

    // ---- internals ----
    private IEnumerator InitialStart(ITriggerTimerTarget target)
    {
        var (min, max) = GetInitialDelay(target);
        if (max > 0f) yield return new WaitForSeconds(UnityEngine.Random.Range(min, max));

        target.TurnOn();
        StartTick(target);
        _restartCoroutines.Remove(target);
    }

    private IEnumerator RestartAfterOff(ITriggerTimerTarget target)
    {
        var (min, max) = GetRestartDelay(target);
        yield return new WaitForSeconds(UnityEngine.Random.Range(min, max));

        target.TurnOn();
        StartTick(target);
        _restartCoroutines.Remove(target);
    }

    private void StartTick(ITriggerTimerTarget target)
    {
        // Если цель уже выключена — не стартуем
        if (!target.IsActive) return;

        if (_tickCoroutines.ContainsKey(target)) return;
        var c = _context.StartCoroutine(TickLoop(target));
        _tickCoroutines[target] = c;
    }

    private void StopTick(ITriggerTimerTarget target)
    {
        if (_tickCoroutines.TryGetValue(target, out var c))
        {
            _context.StopCoroutine(c);
            _tickCoroutines.Remove(target);
        }
    }

    private void StopRestart(ITriggerTimerTarget target)
    {
        if (_restartCoroutines.TryGetValue(target, out var c))
        {
            _context.StopCoroutine(c);
            _restartCoroutines.Remove(target);
        }
    }

    private IEnumerator TickLoop(ITriggerTimerTarget target)
    {
        // Берём настройки
        var settings = (target as ITriggerTimerConfigurable)?.GetTriggerSettings();

        // Если повторение выключено — один раз триггерим и выходим
        if (settings == null || !settings.repeatWhileActive)
        {
            if (target.IsActive) target.Trigger();
            yield break;
        }

        while (target.IsActive)
        {
            target.Trigger();

            float wait = settings.useRandomTick
                ? UnityEngine.Random.Range(settings.tickMinInterval, settings.tickMaxInterval)
                : settings.tickInterval;

            // защитимся от невалидных значений
            if (wait < 0.01f) wait = 0.01f;

            yield return new WaitForSeconds(wait);
        }

        // Вышли из цикла — значит триггер выключили. Ждём Restart через EventBus.
        _tickCoroutines.Remove(target);
    }

    private (float min, float max) GetInitialDelay(ITriggerTimerTarget target)
    {
        if (target is ITriggerTimerConfigurable c && c.GetTriggerSettings() != null)
        {
            var s = c.GetTriggerSettings();
            return (s.initialMinDelay, s.initialMaxDelay);
        }
        return (0f, 0f);
    }

    private (float min, float max) GetRestartDelay(ITriggerTimerTarget target)
    {
        if (target is ITriggerTimerConfigurable c && c.GetTriggerSettings() != null)
        {
            var s = c.GetTriggerSettings();
            return (s.restartMinDelay, s.restartMaxDelay);
        }
        return (3f, 6f); // дефолт
    }
}