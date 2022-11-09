using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonsOfDoom {
    abstract class Character {

        public Character(int health) {
            Health = health;
        }
        public int Health { get; set; }
        public bool IsAlive { get { return Health > 0; } }
        public List<Item> Inventory { get; } = new List<Item>();
    }



}
