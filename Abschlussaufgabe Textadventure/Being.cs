using System;
using System.Collections.Generic;
using System.Linq;


namespace Abschlussaufgabe_TextAdventure
{
    class Being:Interactive
    {
        public int Damage;
        public int Health;
        public List<Item> Items = new List<Item>();
        public Room CurrentRoom;

        public Being (string name, string description, int damage, int health, Room currentRoom): base (name,  description)
        {
            Name = name;
            Description = description;
            Damage = damage;
            Health = health;
            CurrentRoom = currentRoom;

        }

        public void Fight(Player player, NPC enemy)
        {
            bool endFight = false;

            Console.WriteLine("A fight versus " + enemy.Name + " just started.");
            Console.WriteLine("Your enemy has " + enemy.Health + " Health and " + enemy.Damage + " Attack Damage");

            for(;;)
            {
                if(endFight)
                    break;

                Console.WriteLine(Environment.NewLine + "Player's turn what will you do?");
                Console.WriteLine("- fight");
                Console.WriteLine("- run");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "fight":
                        Console.WriteLine("Do you wanna 'select' a weapon or 'attack'?");
                        input = Console.ReadLine();
                        switch (input)
                        {
                            case "attack":
                                Damagecalculation(player, enemy);
                                break;
                            case "select":
                                SelectWeaponInFight(player);
                                break;
                            case "quit": 
                                GameLoader.IsFinished = true;
                                endFight = true;
                                break;
                            default:
                            Console.WriteLine("Please type either attack or select");
                            break;
                        }   
                        break;
                    case "run":
                        if(player.CurrentRoom != player.LastRoom)
                        {
                            player.CurrentRoom = player.LastRoom;
                            Console.WriteLine("Your fled and ran to the last room.");
                        }    
                        else 
                            Console.WriteLine("You can't run.");
                        break;
                    case "quit": 
                        GameLoader.IsFinished = true;
                        endFight = true;
                        break;
                    default:
                        Console.WriteLine("Please type either fight or run");
                        break;
                }

                if (enemy.Health == 0)
                {
                    Console.WriteLine("You won this fight.");
                    break;
                }
                
                if(!endFight)
                {
                    Console.WriteLine("It's your enemy's turn. He attacks.");
                    Damagecalculation(enemy, player);
                    break;
                }    

                if (player.Health == 0)
                {
                    Console.WriteLine("You Lost! Game Over!");
                    GameLoader.IsFinished = true;
                    break;
                }  
            }
        }

        public void Damagecalculation (Being attacker, Being defender)
        {
            defender.Health -= attacker.Damage;
            if (defender.Health <= 0)
                defender.Health = 0;
            Console.WriteLine(attacker.Name + " attacked with " + attacker.Damage + " Damage. " + defender.Name + " has " + defender.Health + " Health left.");
        }

        public void SelectWeaponInFight(Player player)
        {
            int indexLength = 0;
            for(;;)
            {
                Console.WriteLine("Please select a weapon with the number on the left." + Environment.NewLine);
                int index =  0;
                if (player.WeaponList.Any())
                {
                    foreach (Weapon weapon in player.WeaponList)
                        {
                            Console.WriteLine(index + weapon.Name);
                            index ++;
                            indexLength = index;
                        }
                        string input = Console.ReadLine();
                        int x = Int32.Parse(input);

                        for (int i = 0; i<indexLength+1; i++)
                            {
                                if(x==i)
                                {
                                    player.CurrentWeapon = player.WeaponList.ElementAt(i);
                                    player.Damage = player.CurrentWeapon.Damage;
                                } 
                            }
                break;
                }
                else
                {
                    Console.WriteLine("You don't have any weapons." + Environment.NewLine);
                    break;
                } 

            }
        }
        
    }
}