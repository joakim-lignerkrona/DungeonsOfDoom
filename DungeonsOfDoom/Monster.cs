namespace DungeonsOfDoom
{
    abstract class Monster : Character
    {
        public Monster(string name, int health) : base(health)
        {
            Name = name;
        }

        public string Name { get; set; }
        public static Monster GetRandom()
        {
            int rand = Random.Shared.Next(1, 100);
            if(rand < 70)
            {
                return new Skeleton();
            }
            else if(rand < 90)
            {
                return new Orc();
            }
            else
            {
                return new Dragon();
            }
        }
    }

    class Dragon : Monster
    {
        public Dragon() : base("Dragon", 150)
        {

        }
    }

    class Orc : Monster
    {
        public Orc() : base("Orc", 60)
        {

        }
    }
    class Skeleton : Monster
    {
        public Skeleton() : base("Skeleton", 30)
        {

        }
    }
}
