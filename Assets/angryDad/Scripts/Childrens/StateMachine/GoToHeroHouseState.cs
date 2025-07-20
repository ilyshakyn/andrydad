using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.angryDad.Scripts.Childrens.StateMachine
{
    public class GoToHeroHouseState : IChildState
    {
        private readonly ChildContext context;

        public GoToHeroHouseState(ChildContext context)
        {
           this.context = context;
        }

        public void Enter()
        {
            context.Animator.PlayWalkToHouseAnimation();
            context.Mover.MoveTo(context.Hero.position);
        }

        public void Update()
        {
            if (context.Mover.HasReachedDestination())
            {
                var nextState = new RunCrazyAroundState(context);
                context.Controller.ChangeStateExternally(nextState);
            }
        }

        public void Exit() { }
    }
}
