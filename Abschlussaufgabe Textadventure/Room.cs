using System;
using System.Collections.Generic;

namespace Abschlussaufgabe_TextAdventure
{
    class Room: Thing
    {
        public Dictionary<GameLoader.Direction, Room> Neighbours = new Dictionary<GameLoader.Direction, Room>();
        public List<Item> Items = new List<Item>();
        public List<NPC> NPCs = new List<NPC>();
        public Item Key;

        public Room (string name, string description, Item key)
        {
            Name = name;
            Description = description;       
            Key = key;    
        }
    }
}