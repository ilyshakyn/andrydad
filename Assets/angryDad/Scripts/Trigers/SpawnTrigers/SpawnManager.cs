using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.angryDad.Scripts.Trigers.SpawnTrigers
{
    public class SpawnManager : MonoBehaviour
    {

        [SerializeField] private List<SpawnGroup> groups;

        private void Start()
        {
            foreach (var group in groups)
            {
                group.Init();
            }
        }

        public void PauseAllSpawns()
        {
            foreach (var group in groups)
                group.Pause();
        }

        public void ResumeAllSpawns()
        {
            foreach (var group in groups)
                group.Resume();
        }
    }
}