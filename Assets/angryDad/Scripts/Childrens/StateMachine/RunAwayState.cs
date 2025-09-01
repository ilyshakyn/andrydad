using Assets.angryDad.Scripts.Systems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.angryDad.Scripts.Childrens.StateMachine
{
    public class RunAwayState : IChildState
    {
        private readonly ChildContext context;
        private Vector3 escapeTarget;
        private bool hasTriggeredReturn = false;

        public RunAwayState(ChildContext context)
        {
            this.context = context;
        }

        public void Enter()
        {
            context.Animator.PlayRunAwayAnimation();

            escapeTarget = context.Controller.transform.position +
                (context.Controller.transform.position - context.Hero.position).normalized * 10f;

            context.Mover.MoveTo(escapeTarget);
            context.Controller.StartCoroutine(ReturnAfterDelay(5f));
        }

        private IEnumerator ReturnAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (!hasTriggeredReturn)
            {
                GameEvents.TriggerReturnToHome(context.Controller);
                hasTriggeredReturn = true;
            }
        }

        public void Update() { }
        public void Exit() { }
    }
}
