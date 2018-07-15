using System;

namespace Abschlussaufgabe_TextAdventure
{
    class Lock: Interactive
    {
        public int LockId {get; set;}

        public Lock (string name, string description, int lockId): base (name,  description)
        {
            Name = name;
            Description = description;
            LockId = lockId;        
        }

        public bool IsCorrectKey(int lockId, int keyId)
        {
            if(lockId == keyId)
            {
                return true;
            }
            else
            {
                return false; 
            }     
        }
    }
}