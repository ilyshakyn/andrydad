using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.angryDad.Scripts.Childrens.StateMachine
{
    public class RunCrazyAroundState : IChildState
    {
        private readonly ChildContext _context;
        private Vector3 _crazyTarget;

        public RunCrazyAroundState(ChildContext context)
        {
            _context = context;
        }

        public void Enter()
        {
            _context.Animator.PlayRunCrazyAnimation();
            _crazyTarget = GetRandomPointNear(_context.Hero.position, 3f);
            _context.Mover.MoveTo(_crazyTarget);
        }

        public void Update()
        {
            if (_context.Mover.HasReachedDestination())
            {
                _crazyTarget = GetRandomPointNear(_context.Hero.position, 3f);
                _context.Mover.MoveTo(_crazyTarget);
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
