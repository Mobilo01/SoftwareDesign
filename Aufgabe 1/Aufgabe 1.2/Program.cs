using System;

namespace Aufgabe_1._2
{
    class Program
    {
        static string[] subjects = { "Harry", "Hermine", "Ron", "Hagrid", "Snape", "Dumbledore"};
        static string[] verbs = { "braut", "liebt", "studiert", "hasst", "zaubert", "zerstört"};
        static string[] objects = { "Zaubertränke", "den Grimm", "Lupin", "Hogwards", "die Karte des Rumtreibers", "Dementoren"};

        static void Main(string[] args)
        {
            string[] text = new string[5];
            for (int i = 0; i < text.Length; i++)
            {
                text[i] = GetVerse(text, i);
            }
            for (int i = 0; i < text.Length; i++)
            {
                Console.WriteLine(text[i]);
            }
        }

        public static string GetVerse(string[] text, int phrase)
        {
            Random rnd = new Random();
            string subj = subjects[rnd.Next(subjects.Length)];
            string ver = verbs[rnd.Next(verbs.Length)];
            string obj = objects[rnd.Next(objects.Length)];

            if (phrase==0){
                return subj + " " + ver + " " + obj;
            }else{
                for (int i = 0; i < phrase; i++){
                    if (text[i].Contains(subj)){
                        subj = subjects[rnd.Next(subjects.Length)];
                        i = -1;
                    }
                }
                for (int i = 0; i < phrase; i++){
                    if (text[i].Contains(ver)){
                        ver = verbs[rnd.Next(verbs.Length)];
                        i = -1;
                    }
                }
                for (int i = 0; i < phrase; i++){
                    if (text[i].Contains(obj)){
                        obj = objects[rnd.Next(objects.Length)];
                        i = -1;
                    }
                }

                return subj + " " + ver + " " + obj;

            }
        }
    }
}
