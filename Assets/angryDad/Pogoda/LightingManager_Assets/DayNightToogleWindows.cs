using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.angryDad.Pogoda.LightingManager_Assets
{
    public class DayNightToogleWindows: DayNightToggleTarget
    {
        [SerializeField] List<Window> windows;
        public virtual void Awake()
        {
            
                gameObject.SetActive(initiallyActive);
        }
        public override void EnableObject()
        {
            if (gameObject != null)
                gameObject.SetActive(true);
        }


        public override void DisableObject()
        {
            if (gameObject != null)
                gameObject.SetActive(false);
        }

    }
}
