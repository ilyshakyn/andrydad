using System.Collections;
using UnityEngine;

namespace Assets.angryDad.Scripts.Trigers.SpawnTrigers
{
    public abstract  class SpawnStrategyBase : ScriptableObject
    {

        public abstract void Setup(ISpawnPoint point);
    }
}