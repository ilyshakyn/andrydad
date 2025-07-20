using System.Collections;
using UnityEngine;

namespace Assets.angryDad.Scripts.Childrens.StateMachine
{
    public class ChildStateMachine 
    {

        private IChildState currentState;

        public void ChangeState(IChildState newState)
        {
            currentState?.Exit();
            currentState = newState ;
            currentState.Enter();
        }

        public void Update()
        {
            currentState?.Update();
        }
    }
}