using Assets.angryDad.Scripts.Childrens.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.angryDad.Scripts.Childrens.Factory
{
    public static  class ChildStateFactory
    {
        public static IChildState CreateState(ChildStateType type, ChildContext context)
        {
            return type switch
            {
                ChildStateType.Play => new PlayOnPlaygroundState(context),
                ChildStateType.WalkToHouse => new GoToHeroHouseState(context),
                ChildStateType.RunCrazy => new RunCrazyAroundState(context),
                ChildStateType.RunAway => new RunAwayState(context),
                _ => null
            };
        }
    }
}
