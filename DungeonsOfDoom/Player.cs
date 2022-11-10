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
                    if(item is Item)
                    {
                        var itemEffects = (Item)item;
                        var (effect, value) = itemEffects.GetItemEffect();
                        if(effect == ItemEffect.AttackRelative)
                        {
                            multiplier *= value;
                        }
                        else if(effect == ItemEffect.AttackAbsolute)
                        {
                            bonusDamage += (int)Math.Round(value);
                        }
                    }

                });
                return (int)Math.Round(baseDamage * multiplier) + bonusDamage;
            }
        }
        public void UseItem(IPocketable item)
        {
            if(item is Consumable)
            {
                var itemToUse = (Consumable)item;
                var (effect, value) = itemToUse.GetItemEffect();
                int bonusDamage = 0;
                if(effect == ItemEffect.HealthAbsolute)
                {
                    bonusDamage += (int)Math.Round(value);
                }
                Health += bonusDamage;
            }
        }
        public int X { get; set; }
        public int Y { get; set; }

    }
}
