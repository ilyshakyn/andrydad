using System.Collections;
using UnityEngine;

namespace Assets.angryDad.Scripts.Trigers.SpawnTrigers
{
    [CreateAssetMenu(menuName = "Spawner/Strategies/Timed")]
    public class TimedSpawnStrategy : SpawnStrategyBase, ISpawnControllable
    {
        public float minDelay;
        public float maxDelay;
        private bool isSpawning = true;

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
        public void PauseSpawning() => isSpawning = false;
        public void ResumeSpawning() => isSpawning = true;
    }
}