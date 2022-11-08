﻿using System;

namespace DungeonsOfDoom {
    class ConsoleGame {
        Room[,] world;
        Player player;

        public void Play() {
            Console.CursorVisible = false;
            CreatePlayer();
            CreateWorld();

            do {
                Console.Clear();
                DisplayWorld();
                DisplayStats();
                AskForMovement();
                CheckForItem();
            } while(player.IsAlive);

            GameOver();
        }

        private void CheckForItem() {
            var currenRoom = world[player.X, player.Y];
            if(currenRoom.ItemInRoom != null) {
                player.Inventory.Add(currenRoom.ItemInRoom);
                currenRoom.ItemInRoom = null;
            }
            else if(currenRoom.MonsterInRoom != null && currenRoom.MonsterInRoom.Inventory.Count > 0) {
                for(int i = currenRoom.MonsterInRoom.Inventory.Count - 1; i >= 0; i--) {
                    player.Inventory.Add(currenRoom.MonsterInRoom.Inventory[i]);
                    currenRoom.MonsterInRoom.Inventory.Remove(currenRoom.MonsterInRoom.Inventory[i]);

                }
            }
        }

        private void CreatePlayer() {
            player = new Player(30, 0, 0);
        }

        private void CreateWorld() {
            world = new Room[20, 5];
            for(int y = 0; y < world.GetLength(1); y++) {
                for(int x = 0; x < world.GetLength(0); x++) {
                    world[x, y] = new Room();

                    int percentage = Random.Shared.Next(1, 100);
                    if(percentage < 10)
                        world[x, y].MonsterInRoom = new Monster("Skeleton", 30);
                    percentage = Random.Shared.Next(1, 100);
                    if(percentage < 10) {
                        world[x, y].ItemInRoom = new Item("Sword");
                        if(world[x, y].MonsterInRoom != null) {
                            world[x, y].ItemInRoom.Name = "Monster Sword";
                            world[x, y].MonsterInRoom.Inventory.Add(world[x, y].ItemInRoom);
                            world[x, y].ItemInRoom = null;
                        }
                    }
                }
            }
        }

        private void DisplayWorld() {
            for(int y = 0; y < world.GetLength(1); y++) {
                for(int x = 0; x < world.GetLength(0); x++) {
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

        private void DisplayStats() {
            Console.WriteLine($"Health: {player.Health}");
            Console.WriteLine($"Inventory:");
            DisplayInventory();
        }

        private void DisplayInventory() {
            foreach(var item in player.Inventory) {
                Console.WriteLine($"\t{item.Name}");
            }
        }

        private void AskForMovement() {
            int newX = player.X;
            int newY = player.Y;
            bool isValidKey = true;

            ConsoleKeyInfo keyInfo = Console.ReadKey();
            switch(keyInfo.Key) {
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
                newY >= 0 && newY < world.GetLength(1)) {
                player.X = newX;
                player.Y = newY;
            }
        }

        private void GameOver() {
            Console.Clear();
            Console.WriteLine("Game over...");
            Console.ReadKey();
            Play();
        }
    }
}
