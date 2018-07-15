using System;

namespace Abschlussaufgabe_TextAdventure
{
    class Weapon: Item
    {
        public int Damage;

        public Weapon (string name, string description, int damage): base (name,  description)
        {
            Name = name;
            Description = description;
            Damage = damage;        
        }
    }
}