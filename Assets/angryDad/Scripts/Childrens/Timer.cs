using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.angryDad.Scripts.Childrens
{
    public  class Timer
    {
        private float duration;
        private float timeRemaining;

        public Timer(float duration)
        {
            this.duration = duration;
            this.timeRemaining = duration;
        }

        public void Reset() => timeRemaining = duration;
        public void Tick(float deltaTime) => timeRemaining -= deltaTime;
        public bool IsFinished => timeRemaining <= 0f;
    }
}
