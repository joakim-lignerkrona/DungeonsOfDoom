namespace DungeonsOfDoom {
    class Player : Character {
        public Player(int health, int x, int y) : base(health) {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

    }
}
