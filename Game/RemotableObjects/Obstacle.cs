using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemotableObjects
{
    //Przeszkody na mapie, np. drzewa
    class Obstacle : GameObject
    {
        private bool solid;

        public Obstacle(bool solid)
        {
            this.solid = solid;
        }

        public override bool isSolid()
        {
            return solid;
        }
    }
}
