namespace DungeonsOfDoom
{
    class Player : Character
    {
        public Player(int health, int x, int y) : base(health)
        {
            X = x;
            Y = y;
        }

        public override int Damage
        {
            get
            {
                double multiplier = 1;
                int bonusDamage = 0;
                Inventory.ForEach(item =>
                {
                    var (effect, value) = item.GetItemEffect();
                    if(effect == ItemEffect.AttackRelative)
                    {
                        multiplier *= value;
                    }
                    else if(effect == ItemEffect.AttackAbsolute)
                    {
                        bonusDamage += (int)Math.Round(value);
                    }
                });
                return (int)Math.Round(baseDamage * multiplier) + bonusDamage;
            }

        }
        public int X { get; set; }
        public int Y { get; set; }

    }
}
