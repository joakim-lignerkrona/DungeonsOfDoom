using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonsOfDoom
{
    abstract class Character
    {
        public Character(int health)
        {
            Health = health;
        }
        public int Health { get; set; }
        protected int baseDamage = 10;
        public virtual int Damage
        {
            get => baseDamage;
            set
            {
                baseDamage = value;
            }

        }
        public bool IsAlive { get { return Health > 0; } }
        public List<Item> Inventory { get; } = new List<Item>();
        public virtual int Attack(Character enemy)
        {
            enemy.Health -= Damage;
            return Damage;
        }
    }
}
