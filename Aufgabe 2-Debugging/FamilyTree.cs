using System;
using static System.Console;

namespace Debugging
{
    public class Person
    {
        public string FirstName;
        public string LastName;
        public DateTime DateOfBirth;

        public Person Mom;
        public Person Dad;

        public override string ToString(){
        return ($"{FirstName} {LastName}"); 
        }
    }

    public class Familytree
    {
        public static Person Find(Person person)
        {
            Person ret = null;
            if (person.LastName == "Battenberg")
                return person; 

            if(person.Mom != null){
            ret = Find(person.Mom);
            if (ret != null)
                return ret;
            }

            if (person.Dad != null){
            ret = Find(person.Dad);
            if (ret != null)
                return ret;
            }
            return null;
        }

        public static Person WhoIsThisOld(Person person)
        {
            Person ret = null;
            int age = DateTime.Now.Year - person.DateOfBirth.Year;
            if (age >= 160 && age <= 170)
                return person; 
            if(person.Mom != null)
                ret = WhoIsThisOld(person.Mom);
            if (ret != null)
                return ret;
            if(person.Dad != null)
                ret = WhoIsThisOld(person.Dad);
            return ret;
        }

        public static Person BuildTree()
        {
            return  
                new Person { FirstName = "Willi", LastName = "Cambridge", DateOfBirth=new DateTime(1982, 07, 21),
                    Mom = new Person { FirstName = "Diana", LastName = "Spencer", DateOfBirth = new DateTime(1961, 07, 01),
                        Mom = new Person {FirstName = "Franzi", LastName="Roche", DateOfBirth = new DateTime(1936, 01, 20),
                            Mom = new Person {FirstName = "Ruth", LastName="Gill", DateOfBirth = new DateTime(1908, 06, 07)},
                            Dad = new Person {FirstName = "Moritz", LastName="Roche", DateOfBirth = new DateTime(1885, 07, 08)}
                        },
                        Dad = new Person {FirstName = "Eddie", LastName="Spencer", DateOfBirth = new DateTime(1924, 01, 24),
                            Mom = new Person {FirstName = "Cynthia", LastName="Hamilton", DateOfBirth = new DateTime(1897, 08, 16)},
                            Dad = new Person {FirstName = "Albert", LastName="Spencer", DateOfBirth = new DateTime(1892, 05, 23)}
                        },
                    },
                    Dad = new Person {FirstName = "Charlie", LastName = "Wales", DateOfBirth = new DateTime(1948, 11, 14),
                        Mom = new Person {FirstName = "Else", LastName="Windsor", DateOfBirth = new DateTime(1926, 04, 21),
                            Mom = new Person {FirstName = "Lisbeth", LastName="Bowes-Lyon", DateOfBirth = new DateTime(1900, 08, 04)},
                            Dad = new Person {FirstName = "Schorsch-Albert", LastName="York", DateOfBirth = new DateTime(1895, 12, 14)}
                        },
                        Dad = new Person {FirstName = "Philip", LastName="Battenberg", DateOfBirth = new DateTime(1921, 06, 10),
                            Mom = new Person {FirstName = "Alice", LastName="Battenberg", DateOfBirth = new DateTime(1885, 02, 25)},
                            Dad = new Person {FirstName = "Andi", LastName="ElGreco", DateOfBirth = new DateTime(1882, 02, 01)},
                        },
                    },
                };
        }
    }
}