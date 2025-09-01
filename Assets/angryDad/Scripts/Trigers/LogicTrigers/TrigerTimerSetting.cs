using System.Collections;
using UnityEngine;

namespace Assets.angryDad.Scripts.Trigers.LogicTrigers
{
    [CreateAssetMenu(fileName = "TriggerSettings", menuName = "Triggers/Trigger Settings")]
    public class TrigerTimerSetting : ScriptableObject
    {
        [Header("Первичный запуск (перед первым тиком)")]
        public float initialMinDelay = 0f;
        public float initialMaxDelay = 0f;

        [Header("Повторные запуски после TurnOff -> Restart")]
        public float restartMinDelay = 5f;
        public float restartMaxDelay = 10f;

        [Header("Тики урона")]
        public bool repeatWhileActive = true;   // включить периодический урон
        public bool useRandomTick = false;      // фиксированный интервал или случайный в диапазоне
        public float tickInterval = 1.0f;       // секунды между тиками (если не рандом)
        public float tickMinInterval = 0.8f;    // если useRandomTick = true
        public float tickMaxInterval = 1.2f;
    }
}