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

        public void Init()
        {
            foreach (var point in points)
            {
                point.Configure(prefabToSpawn, strategy);
            }
        }
    }
}