using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.angryDad.Scripts.Systems
{
    public static class GameEvents
    {
        public static event Action<ChildController> OnScareByMic;
        public static event Action<ChildController> OnRunBackHomeAfterDelay;

        public static void ScareChildByMic(ChildController child)
        {
            OnScareByMic?.Invoke(child);
        }

        public static void TriggerReturnToHome(ChildController child)
        {
            OnRunBackHomeAfterDelay?.Invoke(child);
        }
    }
}