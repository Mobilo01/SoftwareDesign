using System;

namespace Aufgabe1
{
    class Program
    {
        static void Main(string[] args)
        {
            try{
                var form = args[0];
                var diameter = double.Parse(args[1]);
                    switch(form)
                    {
                        case "w":
                            Console.WriteLine(GetCubeInfo(diameter));
                            break;
                        case "k":
                            Console.WriteLine(GetBallInfo(diameter));
                            break;
                        case "o":
                            Console.WriteLine(GetOctInfo(diameter));
                            break;
                        default:
                            Console.WriteLine("Bitte Typ korrekt eingeben!");
                            break;
                    }

            } 
            catch(Exception)
            {
                Console.WriteLine("Eingabe fehlerhaft");
            }

        }
        public static double GetCubeSurface(double diameter)
        {
            return 6*Math.Pow(diameter, 2);
        }
        public static double GetCubeVolume(double diameter)
        {
            return Math.Pow(diameter, 3);
        }

        public static double GetBallSurface(double diameter)
        {
            return Math.PI * Math.Pow(diameter, 2);
        }
        public static double GetBallVolume(double diameter)
        {
            return (Math.PI * Math.Pow(diameter, 3))/6;
        }

        public static double GetOctSurface(double diameter)
        {
            return 2*Math.Sqrt(3)*Math.Pow(diameter, 2);
        }
        public static double GetOctVolume(double diameter)
        {
            return (Math.Sqrt(2)*Math.Pow(diameter, 3))/3;
        }

        public static string GetCubeInfo(double diameter)
        {
            return "Wuerfel: A=" + Math.Round(GetCubeSurface(diameter), 2) + " | V=" + Math.Round(GetCubeVolume(diameter), 2);
        }

        public static string GetBallInfo(double diameter)
        {
            return "Kugel: A=" + Math.Round(GetBallSurface(diameter), 2) + " | V=" + Math.Round(GetBallVolume(diameter), 2);
        }

        public static string GetOctInfo(double diameter)
        {
            return "Oktaeder: A=" + Math.Round(GetOctSurface(diameter), 2) + " | V=" + Math.Round(GetOctVolume(diameter), 2);
        }
        
    }
}