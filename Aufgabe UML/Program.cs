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
        public string Name;
        public int Alter;
    }

    class Teilnehmer: Person
    {
        public int Matrikelnummer;
        
        public List<Kurs> Kurse;
    }

    class Dozent: Person
    {
        public string Raum;
        public string Sprechstunde;

        public List<Kurs> Kurse;

        public void SeineKurse ()
        {
            foreach (Kurs kurs in Kurse)
                Console.WriteLine(kurs.Titel);
        }

        public List<Teilnehmer> AlleKursTeilnehmer()
        {
            List<Teilnehmer> alleTeilnehmer = new List<Teilnehmer>();
            
            foreach (Kurs kurs in Kurse)
                foreach(Teilnehmer teilnehmer in kurs.Teilnehmer)
                    if(!alleTeilnehmer.Contains(teilnehmer))
                        alleTeilnehmer.Add(teilnehmer);

            return alleTeilnehmer;
        }
    }

    class Kurs
    {
        public string Titel;
        public string Termin;
        public string Raum;

        public Dozent Dozent;
        public List<Teilnehmer> Teilnehmer;

        public void KursInfotext()
        {
            Console.WriteLine("Der Kurs " + Titel + " findet am " + Termin + " in Raum " + Raum + " statt.");
        }
    }
}
