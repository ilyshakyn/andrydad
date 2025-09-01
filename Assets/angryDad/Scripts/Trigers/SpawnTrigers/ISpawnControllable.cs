using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.angryDad.Scripts.Trigers.SpawnTrigers
{
    public interface ISpawnControllable
    {
        void PauseSpawning();
        void ResumeSpawning();
    }
}
