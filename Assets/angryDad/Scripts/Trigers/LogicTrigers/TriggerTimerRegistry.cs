using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.angryDad.Scripts.Trigers.LogicTrigers
{
    public static class TriggerTimerRegistry
    {
        private static readonly List<ITriggerTimerTarget> _registeredTargets = new();

        public static IEnumerable<ITriggerTimerTarget> RegisteredTargets => _registeredTargets;

        public static void Register(ITriggerTimerTarget target)
        {
            if (!_registeredTargets.Contains(target))
                _registeredTargets.Add(target);
        }

        public static void Unregister(ITriggerTimerTarget target)
        {
            if (_registeredTargets.Contains(target))
                _registeredTargets.Remove(target);
        }
    }
}

