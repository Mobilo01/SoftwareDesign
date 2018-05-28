using System;

namespace Aufgabe_UML
{
    class Program
    {
        static void Main(string[] args)
        {

        }
    }

    class Person 
    {
        public int Alter;
        public String Name;
    }

    class Dozent : Person
    {
        public String Raum;
        public String Sprechstunde;

        List<Kurs> Kursliste = new List<Kurs>;

        public void SeineKurse()
        {   
            foreach (Kurs kurs in Kursliste)
                Console.WriteLine(kurs.Titel);
        }

        public List<Teilnehmer> AlleKursTeilnehmer()
        {
            foreach (Kurs kurs in Kursliste)
                {
                    foreach (Teilnehmer teilnehmer in Teilnehmerliste)
                    {
                        Teilnehmerliste.Add(Teilnehmer teilnehmer);
                    }
                }
            return Alle;
        }
    }

    class Teilnehmer : Person
    {
        public int Matrikelnummer;

        List<Kurs> SeineKursliste = new List<Kurs>;
    }

    class Kurs 
    {
        public String Titel;
        public String Raum;
        public String Termin;

        public List<Teilnehmer> Teilnehmerliste = new List<Teilnehmer>;

        public void KursInfoText(Kurs kurs)
        {
            Console.WriteLine("Der Kurs" + kurs.Titel + "findet am" + kurs.Termin + "im Raum" + kurs.Raum + "statt")
        }
    }
}
