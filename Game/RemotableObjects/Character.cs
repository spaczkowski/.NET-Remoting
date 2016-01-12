using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RemotableObjects
{
    [Serializable]
    public abstract class Character
    {
        protected int id;
        protected String name;
        protected Point position;
        protected int healthPoints;
        protected int attack;
        protected int defense;
        protected LinkedList<Item> equipment;
        protected bool isKilled;

        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

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
                foreach (Item item in Equipment)
                {
                    totalHealthPoints += item.BonusLife;
                }
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
                int totalAttack = attack;
                foreach (Item item in Equipment)
                {
                    totalAttack += item.BonusAttack;
                }
                return totalAttack;
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
                int totalDefense = defense;
                foreach (Item item in Equipment)
                {
                    totalDefense += item.BonusDefense;
                }
                return totalDefense;
            }

            set
            {
                defense = value;
            }
        }

        public bool IsKilled
        {
            get
            {
                return isKilled;
            }

            set
            {
                isKilled = value;
            }
        }

        public LinkedList<Item> Equipment
        {
            get
            {
                return equipment;
            }

            set
            {
                equipment = value;
            }
        }
    }
}
