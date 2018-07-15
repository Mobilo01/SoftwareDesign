using System;
using System.Collections.Generic;
using System.Linq;

namespace Abschlussaufgabe_TextAdventure
{
    static class GameLoader
    {
        public static Player Player;
        public static Room WinningRoom;
        public static List<Room> Rooms = new List<Room>();
        public static List<Item> Items = new List<Item>();
        public static List<NPC> NPCs = new List<NPC>();
        public static List<Key> Keys = new List<Key>();
        public static List<Lock> Locks = new List<Lock>();
        public static List<Chest> Chests = new List<Chest>();
        public static bool IsFinished = false;
        
        static void Main(string[] args)
        {
            LoadGameData();
            Console.WriteLine(Environment.NewLine + "Welcome to whatever this is..." + Environment.NewLine);
            Console.WriteLine("You wake up on a glade in the forest. You don't remember anything, not even you name.");

            for (;;)
            {
                if(Player.CurrentRoom==WinningRoom)
                {
                    Console.WriteLine("You entered the" + WinningRoom + " and won the game!");
                    IsFinished = true;
                }

                if (IsFinished == true)
                {
                    Console.WriteLine("Thank you for playing.");
                    break;
                }

                Player.CheckWeapon();

                Console.WriteLine("Type 'help' and press Enter if you don't know what to do." + Environment.NewLine);

                string input = Console.ReadLine().ToLower();

                string command = input.IndexOf(" ") > -1 
                  ? input.Substring(0, input.IndexOf(" "))
                  : input;

                string parameter = input.IndexOf(" ") > -1 
                  ? input.Substring(input.IndexOf(" ") + 1, input.Length - (input.IndexOf(" ") + 1))
                  : "";
                
                switch(command)
                {
                    case "quit": 
                        IsFinished = true;
                        break;
                    case "help": 
                        Player.Help();
                        break;
                    case "inventory": 
                        Player.CheckInventory();
                        break;
                    case "look": 
                        Player.Look();
                        break;
                    case "walk": 
                        Player.Walk(parameter);
                        break;
                    case "take": 
                        Player.Take(parameter);
                        break;
                    case "drop": 
                        Player.Drop(parameter);
                        break;
                    case "lookat": 
                        Player.LookAt(parameter);
                        break;
                    case "attack": 
                        Player.Attack(parameter);
                        break;
                    case "info": 
                        Player.PlayerInfo();
                        break;
                    default:
                        Console.WriteLine("Unknown command, you might type help if you want to know the available commands");
                        break;
                }
            }
        }
        public enum Direction {north=0, east=1, south=2, west=3};

        #region GameData

        public static void LoadGameData ()
        {

            #region Items

            Items.Add(new Item("Leaves", "Just some leaves, why would you want those ?"));
            Items.Add(new Item("Bone", "A huge bone, from which animal might it be?"));

            Items.Add(new Key("Chestkey", "A small key, looks like it belongs to a chest.", 0)); 
            Items.Add(new Key("Doorkey", "A huge well-made key.", 1));

            Keys.Add(new Key("Chestkey", "A small key, looks like it belongs to a chest.", 0)); 
            Keys.Add(new Key("Doorkey", "A huge well-made key.", 1));

            Items.Add(new Weapon("Stick", "It's a huge stick, seems like a good waepon in case of emergency.", 2));
            Items.Add(new Weapon("Stones", "Some stones, the size of a human fist", 4));
            Items.Add(new Weapon("Dirt", "It's dirt...", 0));

            Chests.Add(new Chest("Chest", "It's a small chest", 0));
            Locks.Add(new Lock("Door", "A giant well-crafted door", 1));

            WinningRoom = GetRoomByName("Inside");

            #endregion

            #region Rooms

            Rooms.Add(new Room("Forest", "It's a glade in the forest.", null));
            Rooms.Add(new Room("Crossing", "You're on a crossing with 3 possible ways.", null));
            Rooms.Add(new Room("Cave", "A dark cave, seems like it's not very deep", null));
            Rooms.Add(new Room("Bridge", "An old and pretty damaged bridge...it propably won't hold anything heavier than a human.", null));
            Rooms.Add(new Room("House", "You're in front of a good looking house with a solid looking door, seems uninhabited.", GetItemByName("Doorkey")));
            Rooms.Add(new Room("Inside", "You're inside the house", null));

            #endregion

            #region RoomNeighbours

            GetRoomByName("Forest").Neighbours.Add(Direction.west,GetRoomByName("Crossing"));
            
            GetRoomByName("Crossing").Neighbours.Add(Direction.north, GetRoomByName("Bridge"));
            GetRoomByName("Crossing").Neighbours.Add(Direction.east, GetRoomByName("Forest"));
            GetRoomByName("Crossing").Neighbours.Add(Direction.west, GetRoomByName("Cave"));

            GetRoomByName("Cave").Neighbours.Add(Direction.east,GetRoomByName("Crossing"));

            GetRoomByName("Bridge").Neighbours.Add(Direction.north, GetRoomByName("House"));
            GetRoomByName("Bridge").Neighbours.Add(Direction.south, GetRoomByName("Crossing"));

            GetRoomByName("House").Neighbours.Add(Direction.south, GetRoomByName("Bridge"));

            #endregion

            #region Player

            Player.Create("Player", "Uhh, you better hide your face.", 100, 1, GetRoomByName("Forest"));
            Player = Player.Instance;

            #endregion

            #region AddItemsToRooms

            GetRoomByName("Forest").Items.Add(GetItemByName("Stick"));
            GetRoomByName("Forest").Items.Add(GetItemByName("Leaves"));
            GetRoomByName("Forest").Items.Add(GetItemByName("Dirt"));

            GetRoomByName("Cave").Items.Add(GetItemByName("Stones"));

            #endregion

            #region NPCs

            NPCs.Add(new NPC("Troll", "A giant troll, he looks pretty terrifying, it may be smarter not to fight him.", 100, 12, GetRoomByName("Cave")));

            #endregion

            #region AddItemsToInventories

            GetChestByName("Chest").ChestInventory.Add(GetKeyByName("Doorkey"));
            
           // GetCrateByName("Crate").Inventory.Add(GetItemByName("Booze"));

            GetNPCByName("Troll").Inventory.Add(GetItemByName("Bone"));

            #endregion

            #region AddNPCsToRooms

            GetRoomByName("Cave").NPCs.Add(GetNPCByName("Troll"));

            #endregion

          /*  #region NPCDialogLines

            GetNPCByName("Orlan").DialogLines.Add(new CreatureDialogLine("Hello my friend!", 0, null));
            GetNPCByName("Orlan").DialogLines.Add(new CreatureDialogLine("Get out here you needy vagrant!", 1, null));
            GetNPCByName("Orlan").DialogLines.Add(new CreatureDialogLine("Do you have money to pay for it?", 2, null));

            GetNPCByName("Skeleton").DialogLines.Add(new CreatureDialogLine("Who dares to disturb my rest?", 0, null));
            GetNPCByName("Skeleton").DialogLines.Add(new CreatureDialogLine("You won't hoax me you fool! I will now kill you!", 1, null));
            GetNPCByName("Skeleton").DialogLines.Add(new CreatureDialogLine("If you are dying anyway I could also have a little fun with you! Let's fight!", 2, null));

            GetNPCByName("Servant").DialogLines.Add(new CreatureDialogLine("Hello...", 0, null));
            GetNPCByName("Servant").DialogLines.Add(new CreatureDialogLine("Well, actually you can, yes. I lost my engagement ring on the graveyard...please bring it back.", 1, null));
            GetNPCByName("Servant").DialogLines.Add(new CreatureDialogLine("You think I'm a minor background actor? I have to tell you that I have the key to a crypt wich is important for your progress!", 2, null));
            GetNPCByName("Servant").DialogLines.Add(new CreatureDialogLine("Only if you get me my ring I lost somewhere nearby!", 3, null));
            GetNPCByName("Servant").DialogLines.Add(new CreatureDialogLine("I don't exactly know...it has to be somwhere around.", 4, null));
            GetNPCByName("Servant").DialogLines.Add(new CreatureDialogLine("Thank you very much. Here, take this key as a reward. (He hands you over a key)", 5, GetItemByName("Cryptkey")));
            GetNPCByName("Servant").DialogLines.Add(new CreatureDialogLine("Ok. Here, take the key. (He hands you over a key)", 6, GetItemByName("Cryptkey")));

            #endregion

             #region PlayerDialogModels

            Player.Dialogs.Add(new PlayerDialogModel(GetNPCByName("Orlan")));
            Player.Dialogs.Add(new PlayerDialogModel(GetNPCByName("Servant")));
            Player.Dialogs.Add(new PlayerDialogModel(GetNPCByName("Skeleton")));

            #endregion 

            #region PlayerDialogLines

            GetPlayerDialogModelByDialogPartnerName("Orlan").DialogLines.Add(new PlayerDialogLine("Hello. I need something to drink but I don't have money...", 0, null, 1, 1));
            GetPlayerDialogModelByDialogPartnerName("Orlan").DialogLines.Add(new PlayerDialogLine("Good day Sir, I would like some booze.", 0, null, 2, 2));
            GetPlayerDialogModelByDialogPartnerName("Orlan").DialogLines.Add(new PlayerDialogLine("No.", 2, null, 1, 1));

            GetPlayerDialogModelByDialogPartnerName("Skeleton").DialogLines.Add(new PlayerDialogLine("Hello, my name is Geronimo Röder, I'm living here.", 0, null, 1, 1));
            GetPlayerDialogModelByDialogPartnerName("Skeleton").DialogLines.Add(new PlayerDialogLine("I'm so thirsty I'm literally dying...please give me some booze.", 0, null, 2, 2));
            
            GetPlayerDialogModelByDialogPartnerName("Servant").DialogLines.Add(new PlayerDialogLine("Hello my friend, you are looking worried, can I help you somehow?", 0, null, 1, 1));
            GetPlayerDialogModelByDialogPartnerName("Servant").DialogLines.Add(new PlayerDialogLine("Oh look, it's one of these totally useless background characters again...", 0, null, 2, 2));
            GetPlayerDialogModelByDialogPartnerName("Servant").DialogLines.Add(new PlayerDialogLine("Here, your ring - take it back! (Give Ring to Servant)", 1, GetItemByName("Ring"), 1, 5));
            GetPlayerDialogModelByDialogPartnerName("Servant").DialogLines.Add(new PlayerDialogLine("Then give me the damn key you moron!", 2, null, 1, 3));
            GetPlayerDialogModelByDialogPartnerName("Servant").DialogLines.Add(new PlayerDialogLine("Where exactly did you lost it?", 3, null, 1, 4));
            GetPlayerDialogModelByDialogPartnerName("Servant").DialogLines.Add(new PlayerDialogLine("I got your damn key - take it! (Give Ring to Servant)", 3, GetItemByName("Ring"), 2, 6));
            GetPlayerDialogModelByDialogPartnerName("Servant").DialogLines.Add(new PlayerDialogLine("I got your damn key - take it! I found it on the Graveyard. (Give Ring to Servant)", 4, GetItemByName("Ring"), 1, 6));

            #endregion  */
        }

        #endregion

        #region Helper

        public static Room GetRoomByName(string name)
        {
            return Rooms.Find(Room => Room.Name.ToLower() == name.ToLower());
        }

        public static Item GetItemByName(string name)
        {
            return Items.Find(Item => Item.Name.ToLower() == name.ToLower());
        }

        public static Chest GetChestByName(string name)
        {
            return (Chest) Chests.Find(Lock => Lock.Name.ToLower() == name.ToLower());
        }         

        public static Key GetKeyByName(string name)
        {
            return (Key) Keys.Find(Key => Key.Name.ToLower() == name.ToLower());
        }                                                                      

        public static NPC GetNPCByName(string name)
        {
            return NPCs.Find(NPC => NPC.Name.ToLower() == name.ToLower());
        }

/*        public static PlayerDialogModel GetPlayerDialogModelByDialogPartnerName(string name)
        {
            return Player.Dialogs.Find(x => x.DialogPartner.Name.ToLower() == name.ToLower());
        }               */

        #endregion

    }
}