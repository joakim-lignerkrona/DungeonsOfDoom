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
