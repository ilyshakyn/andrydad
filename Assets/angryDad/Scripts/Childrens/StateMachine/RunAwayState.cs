using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.angryDad.Scripts.Childrens.StateMachine
{
    public class RunAwayState : IChildState
    {
        private readonly ChildContext _context;
        private Vector3 _escapeTarget;

        public RunAwayState(ChildContext context)
        {
            _context = context;
        }

        public void Enter()
        {
            _context.Animator.PlayRunAwayAnimation();
            _escapeTarget = _context.Controller.transform.position + (_context.Controller.transform.position - _context.Hero.position).normalized * 10f;
            _context.Mover.MoveTo(_escapeTarget);
        }

        public void Update() { }
        public void Exit() { }
    }
}
