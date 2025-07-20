using System.Collections;
using UnityEngine;

namespace Assets.angryDad.Scripts.Trigers.SpawnTrigers
{
    [CreateAssetMenu(menuName = "Spawner/Strategies/Timed")]
    public class TimedSpawnStrategy : SpawnStrategyBase
    {
        public float minDelay;
        public float maxDelay;
        TimerLogic intervalLogic = new TimerLogic();

        public override void Setup(ISpawnPoint point)
        {
            MonoBehaviour context = point.GetTransform().GetComponent<MonoBehaviour>();
            context.StartCoroutine(SpawnLoop(point));
        }

        private IEnumerator SpawnLoop(ISpawnPoint point)
        {
            
            while (true)
            {
                yield return new WaitForSeconds(intervalLogic.GetInterval(minDelay, maxDelay));
                point.Spawn();
            }
        }
    }
}