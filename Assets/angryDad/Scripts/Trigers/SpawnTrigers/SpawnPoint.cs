using System.Collections;
using UnityEngine;

namespace Assets.angryDad.Scripts.Trigers.SpawnTrigers
{
    public class SpawnPoint : MonoBehaviour, ISpawnPoint
    {
        [SerializeField] private SpawnType type;
        public SpawnType Type => type;
        public Transform GetTransform() => transform;

        private GameObject prefab;
        private SpawnStrategyBase strategy;

        public void Configure(GameObject prefab, SpawnStrategyBase strategy)
        {
            this.prefab = prefab;
            this.strategy = strategy;
            strategy.Setup(this);
        }

        public void Spawn()
        {
            Instantiate(prefab, transform.position, Quaternion.identity);
        }
    }
}