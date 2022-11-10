namespace DungeonsOfDoom
{
    abstract class Character
    {
        public Character(int health)
        {
            MaxHealth = health;
            Health = health;
        }
        public int MaxHealth { get; set; }
        int health;
        public virtual int Health
        {
            get
            {
                return health;
            }
            set
            {
                health = value > MaxHealth ? MaxHealth : value;
            }
        }
        protected int baseDamage = 10;
        public virtual int Damage
        {
            get => baseDamage;
            set
            {
                baseDamage = value;
            }

        }
        public bool IsAlive { get { return Health > 0; } }
        public List<Item> Inventory { get; } = new List<Item>();
        public virtual int Attack(Character enemy)
        {
            enemy.Health -= Damage;
            return Damage;
        }
    }
}
