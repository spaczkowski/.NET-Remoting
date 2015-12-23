using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RemotableObjects
{
    public abstract class Character
    {
        protected String name;
        protected Point position;
        protected int healthPoints;
        protected int attack;
        protected int defense;
        protected LinkedList<Item> equipment;

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

        public Point Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }

        public int HealthPoints
        {
            get
            {
                int totalHealthPoints = healthPoints;
                //TODO: Trzeba przejść po liście equipment i dodać do zmiennej totalHealthPoints ewentualne modyfikacje liczby żyć
                return totalHealthPoints;
            }

            set
            {
                healthPoints = value;
            }
        }

        public int Attack
        {
            get
            {
                //TODO: Analogicznie jak wyżej
                return attack;
            }

            set
            {
                attack = value;
            }
        }

        public int Defense
        {
            get
            {
                //TODO: Analogicznie jak wyżej
                return defense;
            }

            set
            {
                defense = value;
            }
        }
    }
}
