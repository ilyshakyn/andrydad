using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.angryDad.Scripts.Childrens.Animation
{
    internal class ChildAnimator : MonoBehaviour, IChildAnimator
    {
        private readonly Animator animator;

        public ChildAnimator(Animator animator)
        {
            this.animator = animator;
        }

        public void PlayPlayAnimation() => animator.SetTrigger("Play");
        public void PlayWalkToHouseAnimation() => animator?.SetTrigger("WalkToHouse");
        public void PlayRunCrazyAnimation() => animator?.SetTrigger("RunCrazy");
        public void PlayRunAwayAnimation() => animator?.SetTrigger("RunAway");
    }
}
