using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.angryDad.Scripts.Trigers.SpawnTrigers
{
    public class SpawnGroup : MonoBehaviour
    {

        public string groupName;
        public SpawnStrategyBase strategy;
        public List<SpawnPoint> points;
        public GameObject prefabToSpawn;
        private void OnEnable()
        {
            Init(); // <-- повторная инициализация стратегии при повторном включении объекта
        }
        public void Init()
        {
            foreach (var point in points)
            {
                point.Configure(prefabToSpawn, strategy);
            }
        }
        public void Pause()
        {
            if (strategy is ISpawnControllable controllable)
                controllable.PauseSpawning();
        }

        public void Resume()
        {
            if (strategy is ISpawnControllable controllable)
                controllable.ResumeSpawning();
        }
    }
}