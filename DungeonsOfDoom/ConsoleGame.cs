using Spectre.Console;

namespace DungeonsOfDoom
{
    class ConsoleGame
    {
        Room[,] world;
        Player player;

        public void Play()
        {
            Console.CursorVisible = false;
            CreatePlayer();
            CreateWorld();

            do
            {
                Console.Clear();
                //Console.SetCursorPosition(0, 0);
                DisplayFancyWorld();
                EnterRoom();
                DisplayStats();
                DisplayInventory();
                AskForMovement();
            } while(player.IsAlive && Monster.Count != 0);

            GameOver();
        }



        private void EnterRoom()
        {
            var currenRoom = world[player.X, player.Y];
            if(currenRoom.ItemInRoom != null)
            {
                player.Inventory.Add(currenRoom.ItemInRoom);
                currenRoom.ItemInRoom = null;
            }
            else if(currenRoom.MonsterInRoom != null)
            {
                FightMonster(player, currenRoom.MonsterInRoom);
                CollectReward(currenRoom);
                currenRoom.MonsterInRoom = null;
            }
        }

        private void FightMonster(Player player, Monster monsterInRoom)
        {
            int turn = 0;
            while(player.IsAlive && monsterInRoom.IsAlive)
            {

                if(turn % 2 == 0)
                {
                    int damage = player.Attack(monsterInRoom);

                }
                else
                {
                    int damage = monsterInRoom.Attack(player);
                }
                turn++;
            }

        }

        private void CollectReward(Room currenRoom)
        {
            if(currenRoom.MonsterInRoom.Inventory.Count > 0)
            {
                for(int i = currenRoom.MonsterInRoom.Inventory.Count - 1; i >= 0; i--)
                {
                    player.Inventory.Add(currenRoom.MonsterInRoom.Inventory[i]);
                    currenRoom.MonsterInRoom.Inventory.Remove(currenRoom.MonsterInRoom.Inventory[i]);
                }
            }
            player.Inventory.Add(currenRoom.MonsterInRoom);
        }

        private void CreatePlayer()
        {
            player = new Player(100, 0, 0);
        }

        private void CreateWorld()
        {
            world = new Room[20, 10];
            Monster.ResetCount();
            for(int y = 0; y < world.GetLength(1); y++)
            {
                for(int x = 0; x < world.GetLength(0); x++)
                {
                    world[x, y] = new Room();

                    int percentage = Random.Shared.Next(1, 100);
                    if(percentage < 10)
                    {
                        world[x, y].MonsterInRoom = Monster.GetRandom();

                    }
                    percentage = Random.Shared.Next(1, 100);
                    if(percentage < 10)
                    {
                        world[x, y].ItemInRoom = Item.GetRandom();
                        if(world[x, y].MonsterInRoom != null)
                        {
                            world[x, y].MonsterInRoom.Inventory.Add(world[x, y].ItemInRoom);
                            world[x, y].ItemInRoom = null;
                        }
                    }
                }
            }
        }

        private void DisplayFancyWorld()
        {
            var table = new Table();
            table.Border(TableBorder.Horizontal);
            for(int x = 0; x < world.GetLength(0); x++)
                table.AddColumn(" ");

            for(int y = 0; y < world.GetLength(1); y++)
            {
                string[] rowToPrint = new string[world.GetLength(0)];
                for(int x = 0; x < world.GetLength(0); x++)
                {
                    Room room = world[x, y];
                    if(player.X == x && player.Y == y)
                        rowToPrint[x] = "[yellow1]P[/]";
                    else if(room.MonsterInRoom != null)
                        rowToPrint[x] = "[red]M[/]";
                    else if(room.ItemInRoom != null)
                        rowToPrint[x] = "[blue]I[/]";
                    else
                        rowToPrint[x] = "[gray].[/]";
                }
                table.AddRow(rowToPrint);
            }
            AnsiConsole.Write(table);
        }
        private void DisplayWorld()
        {
            for(int y = 0; y < world.GetLength(1); y++)
            {
                for(int x = 0; x < world.GetLength(0); x++)
                {
                    Room room = world[x, y];
                    if(player.X == x && player.Y == y)
                        Console.Write("P");
                    else if(room.MonsterInRoom != null)
                        Console.Write("M");
                    else if(room.ItemInRoom != null)
                        Console.Write("I");
                    else
                        Console.Write(".");
                }
                Console.WriteLine();
            }
        }

        private void DisplayStats()
        {
            //Console.WriteLine($"Health: {player.Health}");
            Console.WriteLine();
            AnsiConsole.Write(new BreakdownChart()
                .Width(80)
                .AddItem("Health", player.Health, Color.Red)
                .AddItem(" ", player.MaxHealth - player.Health, Color.Grey));
            Console.WriteLine();
            AnsiConsole.Write(new BreakdownChart()
                .Width(80)
                .AddItem("Progress", Monster.MonsterDefeated, Color.Green)
                .AddItem(" ", Monster.Count, Color.Grey));
        }

        private void DisplayInventory()
        {
            Console.WriteLine();
            AnsiConsole.Write(new Rule("[red]Inventory[/]").LeftAligned());
            foreach(var item in player.Inventory)
            {
                Console.WriteLine($"\t{item.Name}");
            }
        }

        private void AskForMovement()
        {
            int newX = player.X;
            int newY = player.Y;
            bool isValidKey = true;

            ConsoleKeyInfo keyInfo = Console.ReadKey();
            switch(keyInfo.Key)
            {
                case ConsoleKey.C:
                    SelectItem();
                    break;
                case ConsoleKey.RightArrow:
                    newX++;
                    break;
                case ConsoleKey.LeftArrow:
                    newX--;
                    break;
                case ConsoleKey.UpArrow:
                    newY--;
                    break;
                case ConsoleKey.DownArrow:
                    newY++;
                    break;
                default:
                    isValidKey = false;
                    break;
            }

            if(isValidKey &&
                newX >= 0 && newX < world.GetLength(0) &&
                newY >= 0 && newY < world.GetLength(1))
            {
                player.X = newX;
                player.Y = newY;
            }
        }

        private void SelectItem()
        {
            var items = new List<string>();
            player.Inventory.ForEach(item =>
            {
                if(item is Consumable)
                    items.Add(item.Name);
            });
            if(player.Inventory.Count <= 0)
                return;
            Console.Clear();
            var selection = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("Whitch item do you want to use?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
                    .AddChoices(items));
            var itemToConsume = player.Inventory.Find(item => item.Name == selection);
            player.UseItem(itemToConsume);
            player.Inventory.Remove(itemToConsume);
            Console.Clear();

        }

        private void GameOver()
        {
            Console.Clear();
            if(player.IsAlive)
            {
                AnsiConsole.Write(new FigletText("Yay! You won!")
                .Centered()
                .Color(Color.Gold3));
            }
            else
            {
                AnsiConsole.Write(new FigletText("You died, N00b!")
                .Centered()
                .Color(Color.Red));
            }

            var selection = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("Do wou want to play again?")
                    .PageSize(4)
                    .MoreChoicesText("[grey](Do you want to play again?)[/]")
                    .AddChoices("Yes", "No"));
            if(selection == "Yes")
            {
                Console.Clear();
                Play();
            }
        }
    }
}
