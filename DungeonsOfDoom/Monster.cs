namespace DungeonsOfDoom
{
    class Monster
    {
        public Monster(string name, int health)
        {
            Name = name;
            Health = health;
        }

        public string Name { get; set; }
        public int Health { get; set; }
        public bool IsAlive { get { return Health > 0; } }

    }
}
