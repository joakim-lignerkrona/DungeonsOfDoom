using System;
using Spectre.Console;

namespace DungeonsOfDoom
{
    class ConsoleGame
    {
        Room[,] world;
        Player player;
        int monsterCount = 0;

        public void Play()
        {
            Console.CursorVisible = false;
            CreatePlayer();
            CreateWorld();

            do
            {
                Console.Clear();
                DisplayFancyWorld();
                DisplayStats();
                AskForMovement();

                EnterRoom();
            } while(player.IsAlive && monsterCount > 0);

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
                    player.Attack(monsterInRoom);

                }
                else
                {
                    monsterInRoom.Attack(player);
                }
                turn++;
            }
            monsterCount--;
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
        }

        private void CreatePlayer()
        {
            player = new Player(100, 0, 0);
        }

        private void CreateWorld()
        {
            world = new Room[20, 5];
            for(int y = 0; y < world.GetLength(1); y++)
            {
                for(int x = 0; x < world.GetLength(0); x++)
                {
                    world[x, y] = new Room();

                    int percentage = Random.Shared.Next(1, 100);
                    if(percentage < 10)
                    {
                        world[x, y].MonsterInRoom = Monster.GetRandom();
                        monsterCount++;
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
                        rowToPrint[x] = "P";
                    else if(room.MonsterInRoom != null)
                        rowToPrint[x] = "M";
                    else if(room.ItemInRoom != null)
                        rowToPrint[x] = "I";
                    else
                        rowToPrint[x] = ".";
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
            Console.WriteLine($"Health: {player.Health}");
            Console.WriteLine($"Inventory:");
            DisplayInventory();
        }

        private void DisplayInventory()
        {
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
                case ConsoleKey.I:
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
            if(player.Inventory.Count <= 0)
                return;
            var items = new List<string>();
            player.Inventory.ForEach(item => items.Add(item.Name));
            var selection = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("Whitch item do you want to use?")
                    .PageSize(4)
                    .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
                    .AddChoices(items));
            var itemToConsume = player.Inventory.Find(item => item.Name == selection);
            player.UseItem(itemToConsume);
            player.Inventory.Remove(itemToConsume);
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
                AnsiConsole.Write(new FigletText("Game Over")
                .Centered()
                .Color(Color.Red));
            }

            var selection = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("Whitch item do you want to use?")
                    .PageSize(4)
                    .MoreChoicesText("[grey](Do you want to play again?)[/]")
                    .AddChoices("[green]Yes[/]", "[red]No[/]"));

            Play();
        }
    }
}
