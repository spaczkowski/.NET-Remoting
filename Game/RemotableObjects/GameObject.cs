using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemotableObjects
{
    [Serializable]
    public abstract class GameObject
    { 
        private String name;

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public abstract bool isSolid();
    }
}
