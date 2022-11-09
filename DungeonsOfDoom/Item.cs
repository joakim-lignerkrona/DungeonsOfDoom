namespace DungeonsOfDoom
{
    abstract class Item
    {
        public Item(string name)
        {
            Name = name;
        }


        public string Name { get; set; }

        public static Item GetRandom()
        {
            int rand = Random.Shared.Next(1, 100);
            if(rand < 70)
            {
                return new RawMeat();
            }
            else
            {
                return new Sword();
            }
        }
    }

    class Sword : Item
    {
        public Sword() : base("Sword")
        {

        }
    }

    class RawMeat : Item
    {
        public RawMeat() : base("RawMeat")
        {

        }
    }
}
