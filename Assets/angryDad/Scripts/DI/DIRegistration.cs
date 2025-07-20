using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.angryDad.DI
{
    internal class DIRegistration
    {
        public Func<DiContainer,object> Factory { get; set; }
        public bool IsSingleton { get; set; }
        public object Instance { get; set; }
    
    }
}
