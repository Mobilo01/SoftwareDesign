using System;

namespace Abschlussaufgabe_TextAdventure
{
    class Key: Item
    {
        public int KeyId;
        public Key (string name, string description, int keyId): base (name,  description)
        {
            Name = name;
            Description = description;  
            KeyId = keyId;       
        }
    }
}