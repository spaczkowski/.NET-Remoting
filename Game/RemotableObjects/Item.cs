using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemotableObjects
{
    public enum ItemType
    {
        Instant, //Działa natychmiast i znika
        Equipment //Trzeba mieć przy sobie
    }

    //Przedmioty, które zwiększają jakieś statystyki.
    //Podałem 3, które mi przyszły do głowy, listę można oczywiście powiększyć :)
    public class Item : GameObject
    {
        private int bonusAttack;
        private int bonusLife;
        private int bonusDefense;
        private ItemType itemType;

        public ItemType ItemType
        {
            get
            {
                return itemType;
            }

            set
            {
                itemType = value;
            }
        }

        public override bool isSolid()
        {
            return false;
        }

        //TODO: gettery i settery
    }
}
