namespace DungeonsOfDoom {
    class Player {
        public Player(int health, int x, int y) {
            Health = health;
            X = x;
            Y = y;
        }

        public int Health { get; set; }
        public bool IsAlive { get { return Health > 0; } }
        public int X { get; set; }
        public int Y { get; set; }
        public List<Item> Inventory { get; } = new List<Item>();
    }
}
