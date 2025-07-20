using System.Collections;
using UnityEngine;


namespace Assets.angryDad.Scripts.Trigers.SpawnTrigers
{
    public class TimerLogic 
    {

        public  float GetInterval(float minDelay, float maxDelelay) 
        {
           
           minDelay = Random.Range(minDelay, maxDelelay);
           maxDelelay = Random.Range(minDelay, maxDelelay);

            float currentInterval = Random.Range(minDelay, maxDelelay);
            return currentInterval;
        }
      
        
    }
}