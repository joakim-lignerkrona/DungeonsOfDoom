namespace DungeonsOfDoom
{
    abstract class Item
    {
        public Item(string name)
        {
            Name = name;
        }
        public string Name { get; set; }

        public abstract (ItemEffect, double) GetItemEffect();

        public static Item GetRandom()
        {
            int rand = Random.Shared.Next(1, 100);
            return rand < 70 ? new RawMeat() : new Sword();
        }
    }
    abstract class Consumable : Item
    {
        public Consumable(string name) : base(name)
        {

        }
    }
    abstract class Weapon : Item
    {
        public Weapon(string name) : base(name)
        {

        }
    }

    class Sword : Weapon
    {
        public Sword() : base("Sword")
        {
        }

        public override (ItemEffect, double) GetItemEffect()
        {
            return (ItemEffect.AttackRelative, 1.8);
        }

    }
    class CookedMeat : Consumable
    {
        public CookedMeat() : base("Cooked Meat")
        {

        }
        public override (ItemEffect, double) GetItemEffect()
        {
            return (ItemEffect.HealthAbsolute, 20);
        }
    }
    class RawMeat : Consumable
    {
        public RawMeat() : base("Raw Meat")
        {

        }
        public override (ItemEffect, double) GetItemEffect()
        {
            return (ItemEffect.HealthAbsolute, 10);
        }
    }
    class BareBone : Consumable
    {
        public BareBone() : base("Bare Bone")
        {

        }
        public override (ItemEffect, double) GetItemEffect()
        {
            return (ItemEffect.HealthAbsolute, 5);
        }
    }
    enum ItemEffect
    {
        AttackAbsolute,
        AttackRelative,
        HealthAbsolute,
        HealthRelative
    }
}
