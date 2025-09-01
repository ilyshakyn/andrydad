
using UnityEngine;

namespace Assets.angryDad.Scripts.Childrens.StateMachine
{
    public class PlayOnPlaygroundState : IChildState
    {
        private readonly ChildContext context;
        private Vector3 targetPoint;
        private Timer timer;

        public PlayOnPlaygroundState(ChildContext context)
        {
            this.context = context;
        }

        public void Enter()
        {
            context.Animator.PlayPlayAnimation();
            targetPoint = GetRandomPointNear(context.Controller.transform.position, 5f);
            context.Mover.MoveTo(targetPoint);
            timer = new Timer(10f);
            timer.Tick(1f);

        }

        public void Update()
        {
            timer.Tick(Time.deltaTime);

            if (context.Mover.HasReachedDestination())
            {
                targetPoint = GetRandomPointNear(context.Controller.transform.position, 5f);
                context.Mover.MoveTo(targetPoint);
            }

            if (timer.IsFinished)
            {
                var nextState = new GoToHeroHouseState(context);
                context.Controller.ChangeStateExternally(nextState);
            }
        }

        public void Exit() { }

        private Vector3 GetRandomPointNear(Vector3 center, float radius)
        {
            Vector2 rand = UnityEngine.Random.insideUnitCircle * radius;
            return center + new Vector3(rand.x, 0, rand.y);
        }
    }
}
