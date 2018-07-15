using System;
using System.Collections.Generic;
using System.Linq;

namespace Abschlussaufgabe_TextAdventure
{
    class Player: Being
    {
        private static Player _instance;

       // public List<PlayerDialogModel> Dialogs {get; set;} = new List<PlayerDialogModel>();
        public List<Weapon> WeaponList {get; set;} = new List<Weapon>();
        public List<Item> Inventory {get; set;} = new List<Item>();
        public int BaseDamage;
        public Weapon CurrentWeapon;
        public int MaxHealth;
        public Room LastRoom;

        private Player (string name, string description, int health, int damage, Room currentRoom): base (name, description, damage, health, currentRoom)
        {
            Name = name;
            Description = description;
            MaxHealth = health;
            Health = MaxHealth;
            BaseDamage = damage;
            Damage = BaseDamage;
            CurrentRoom = currentRoom;
        }

        public static void Create (string name, string description, int health, int damage, Room currentRoom)
        {
            if (_instance != null)
                throw new Exception ("Object already created!");
            _instance = new Player (name, description, health, damage, currentRoom);
        }

        public static Player Instance
        {
            get
            {
                if(_instance == null)
                    throw new Exception ("Object not created!");
                return _instance;
            }
        }

        #region PlayerActions

        public void Help ()
        {
            Console.WriteLine("You can use the following commands to play the game:");
            Console.WriteLine("'inventory' - to check your pockets");
            Console.WriteLine("'look' - to look around in the room / place you're in");
            Console.WriteLine("'look at <name of something in the room / inventory>' (la) - to look at the object or person and get further information");
            Console.WriteLine("'take <name of an item in the room>' - to pick an item up");
            Console.WriteLine("'drop <name of an item in your inventory' - to drop an item from your inventory");
            Console.WriteLine("'walk <direction>' - move to the direction (north, east, south or west)");
            Console.WriteLine("'interact <Thing or Being in the Room>' - to interact with something or someone");
            Console.WriteLine("'attack <being>' - to start a fight with the being");
            Console.WriteLine("'info' - show info about the Player");
            Console.WriteLine("'quit' - to quit the game");
        }

        public void PlayerInfo ()
        {
            Console.WriteLine("Your stats:");
            Console.WriteLine("Health: " + Health + " / " + MaxHealth);
            Console.WriteLine("Damage: " + Damage + " (= base damage + weapon damage)");
            if(CurrentWeapon != null)
            {
                Console.WriteLine("Current weapon: " + CurrentWeapon.Name);
                Console.WriteLine("Current weapon damage: " + CurrentWeapon.Damage);
            }
            else
                Console.WriteLine("Current weapon: none");  
        }

        public void CheckInventory ()
        {
            Console.WriteLine("Your Inventory:");
            foreach (Item item in Inventory)
            {
                Console.WriteLine(item.Name);
            }
        }

        public void Look ()
        {
            Console.WriteLine("You are looking around in the " + CurrentRoom.Name + ":");
            Console.WriteLine(CurrentRoom.Description);
            Console.WriteLine(Environment.NewLine + "The following places are nearby:");
            foreach(KeyValuePair<GameLoader.Direction, Room> neighbour in CurrentRoom.Neighbours)
            {
                Console.WriteLine("   You can reach the " + neighbour.Value.Name + " in the " + neighbour.Key + ".");
            }
            Console.WriteLine(Environment.NewLine + "The beings in the location are:");
            if (!CurrentRoom.NPCs.Any())
                Console.WriteLine("   none");
            foreach (NPC npc in CurrentRoom.NPCs)
            {
                Console.WriteLine("   " + npc.Name);
            }
            Console.WriteLine(Environment.NewLine + "Also you can see the following things nearby:");
            if (!CurrentRoom.Items.Any())
                Console.WriteLine("   none");
            foreach (Item item in CurrentRoom.Items)
                Console.WriteLine("   " + item.Name);
        }

        public void LookAt (string thing)
        {
           if (String.IsNullOrWhiteSpace(thing))
                Console.WriteLine ("Please select a Object to look at.");
            else
            {
                Thing subject = CurrentRoom.Items.Find(x => x.Name.ToLower() == thing);
                if (subject == null)
                    subject = CurrentRoom.NPCs.Find(x => x.Name.ToLower() == thing);
                if(subject == null)
                    subject = Inventory.Find(x => x.Name.ToLower() == thing);
                if(subject == null)
                    Console.WriteLine("There is nothing with the name '" + thing + "' in the room.");
                else
                {
                    Console.WriteLine(subject.Description);
                    if(subject is Weapon)
                    {
                        Weapon weapon = (Weapon) subject;
                        Console.WriteLine(weapon.Name + " can be used as a weapon and causes " + weapon.Damage + " damage.");
                    } 
                }
                
            } 
        }

        public void Attack (string person)
        {
           if (String.IsNullOrWhiteSpace(person))
                Console.WriteLine ("Please select a Object to look at.");
            else
                if(CurrentRoom.NPCs.Find(x =>  x.Name.ToLower() == person) != null)
                    if(CurrentRoom.NPCs.Find(x =>  x.Name.ToLower() == person).Health == 0)
                        Console.WriteLine("You can't attack corpses. But you can loot them.");
                    else 
                        Fight(GameLoader.Player, CurrentRoom.NPCs.Find(x =>  x.Name.ToLower() == person));                        
                else if (CurrentRoom.Items.Find(x =>  x.Name.ToLower() == person) != null)
                    Console.WriteLine ("You can't fight this...");
                else
                    Console.WriteLine ("There is no " + person + " nearby.");
        }
        
        public void Walk (string direction)
        {
            Room nextRoom = null;
            bool validDirection = false;
            GameLoader.Direction selectedDirection = GameLoader.Direction.north;

            switch (direction)
            {
                case "north": 
                    CurrentRoom.Neighbours.TryGetValue(GameLoader.Direction.north, out nextRoom);
                    validDirection = true;
                    selectedDirection = GameLoader.Direction.north;
                    break;
                case "south":
                    CurrentRoom.Neighbours.TryGetValue(GameLoader.Direction.south, out nextRoom);
                    validDirection = true;
                    selectedDirection = GameLoader.Direction.south;
                    break;
                case "east": 
                    CurrentRoom.Neighbours.TryGetValue(GameLoader.Direction.east, out nextRoom);
                    validDirection = true;
                    selectedDirection = GameLoader.Direction.east;
                    break;
                case "west": 
                    CurrentRoom.Neighbours.TryGetValue(GameLoader.Direction.west, out nextRoom);
                    validDirection = true;
                    selectedDirection = GameLoader.Direction.west;
                    break;
                default:
                    Console.WriteLine("'" + direction + "'" + " is not a valid direction to go.");
                    break;
            }

            if (validDirection && nextRoom == null)
                Console.WriteLine("There is no place to go in the " + selectedDirection.ToString() + ".");
            else if (nextRoom != null)
            {
                if(nextRoom.Key != null && Inventory.Contains(nextRoom.Key) || nextRoom.Key == null)
                {
                    LastRoom = CurrentRoom;
                    CurrentRoom = nextRoom;
                    Console.WriteLine("You are now in the " + CurrentRoom.Name + ".");
                    Look();
                }
                else
                    Console.WriteLine("You can't go there - you are missing the right key!");
            }   
        }

        public void Take (string thing)
        {
            if (String.IsNullOrWhiteSpace(thing))
                Console.WriteLine ("Please select a Object to pick up.");
            else
            {
                Item item = CurrentRoom.Items.Find(x => x.Name.ToLower() == thing);
            
                if (item == null)
                    Console.WriteLine("There is no item with the name '" + thing + "' in the room.");
                else
                {
                        Inventory.Add(item);
                        CurrentRoom.Items.Remove(item);
                        Console.WriteLine("You added to your inventory: " + item.Name);
                }
            }
        }

        public void Drop (string thing)
        {
            if (String.IsNullOrWhiteSpace(thing))
                Console.WriteLine ("Please select a object to drop.");
            else
            {
                Item item = Inventory.Find(x => x.Name.ToLower() == thing);
                if (item == null)
                    Console.WriteLine("There is no item with the name '" + thing + "' in your inventory.");
                else
                {
                    CurrentRoom.Items.Add(item);
                    Inventory.Remove(item);
                    Console.WriteLine("You dropped a " + item.Name + " from your inventory.");
                }
            }
        }

        public void CheckWeapon() 
        {
            if(CurrentWeapon != null)
                if(Inventory.Find(x => x.Name == CurrentWeapon.Name) == null)
                {
                    Damage = BaseDamage;
                    CurrentWeapon = null;
                }
        }

        #endregion

    }
}