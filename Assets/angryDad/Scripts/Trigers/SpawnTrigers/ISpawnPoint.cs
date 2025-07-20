using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.angryDad.Scripts.Trigers.SpawnTrigers
{
    public interface ISpawnPoint
    {
        SpawnType Type { get; }
        Transform GetTransform();
        void Configure(GameObject prefab, SpawnStrategyBase strategy);
        void Spawn();
    }
}
