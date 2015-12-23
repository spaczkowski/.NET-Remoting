using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;

namespace RemotableObjects
{
    public class Player : Character
    {
        public void collectItem(Item gameObject)
        {
            if (gameObject.ItemType == ItemType.Equipment)
            {
                equipment.AddLast(gameObject);
            }
            else if (gameObject.ItemType == ItemType.Instant)
            {
                //TODO: modyfikacja statystyk w zależności od pól w Collectable
            }
        }
    }
}
