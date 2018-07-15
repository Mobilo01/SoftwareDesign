using System;
using System.Collections.Generic;

namespace Abschlussaufgabe_TextAdventure
{
    class NPC:Being
    {
        //public List<string> DialogLines {get; set;} = new List<string>();
        //public int DialogPhase;
        public List<Item> Inventory {get; set;} = new List<Item>();
        //public int MaxDialogPhase;

        public NPC(string name, string description, int health, int damage, Room currentRoom) : base (name,  description, health, damage, currentRoom)
        {
            Name = name;
            Description = description;
            Damage = damage;
            Health = health;
        }
    }
}