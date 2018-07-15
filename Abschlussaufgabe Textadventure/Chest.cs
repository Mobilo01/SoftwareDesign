using System;
using System.Collections.Generic;

namespace Abschlussaufgabe_TextAdventure
{
    class Chest: Lock
    {
        public List<Item> ChestInventory {get; set;} = new List<Item>();

        public Chest (string name, string description, int lockId): base (name,  description, lockId)
        {
            Name = name;
            Description = description;
            LockId = lockId;        
        }
    }
}