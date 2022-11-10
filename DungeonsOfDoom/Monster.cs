namespace DungeonsOfDoom
{
    abstract class Monster : Character, IPocketable
    {
        public static int MonstersInWorld { get; private set; } = 0;
        public static int MonsterDefeated { get; private set; } = 0;
        public static int Count
        {
            get
            {
                return MonstersInWorld - MonsterDefeated;

            }

        }

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

        internal static void ResetCount()
        {
            MonsterDefeated = 0;
            MonstersInWorld = 0;
        }

        public Monster(string name, int health) : base(health)
        {
            Name = name;
            MonstersInWorld++;
        }
        public override int Health
        {
            get => base.Health;
            set
            {
                base.Health = value;
                if(value < 0)
                {
                    MonsterDefeated++;
                }
            }
        }

        public string Name { get; set; }
    }

    class Dragon : Monster
    {
        public Dragon() : base("Dragon", 150)
        {

        }
        public override int Attack(Character enemy)
        {
            var damage = 0;
            if(Random.Shared.Next(1, 100) < 90)
            {
                damage = Random.Shared.Next(10, 40);
            }
            enemy.Health -= damage;
            return damage;
        }
    }

    class Orc : Monster
    {
        public Orc() : base("Orc", 30)
        {

        }

    }
    class Skeleton : Monster
    {
        public Skeleton() : base("Skeleton", 20)
        {

        }

        public override int Attack(Character enemy)
        {
            int damage;
            if(this.Health <= enemy.Health / 2)
            {
                damage = 5;
            }
            else
            {
                damage = 1;
            }

            enemy.Health -= damage;
            return damage;

        }
    }
}
