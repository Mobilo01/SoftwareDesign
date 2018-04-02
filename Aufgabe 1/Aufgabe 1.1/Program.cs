using System;

namespace Aufgabe1
{
    class Program
    {
        static void Main(string[] args)
        {
            try{
                var form = args[0];
                var d = double.Parse(args[1]);
                    switch(form){
                        case "w":
                            Console.WriteLine(GetCubeInfo(d));
                            break;
                        case "k":
                            Console.WriteLine(GetBallInfo(d));
                            break;
                        case "o":
                            Console.WriteLine(GetOctInfo(d));
                            break;
                        default:
                            Console.WriteLine("Bitte Typ korrekt eingeben!");
                            break;
                    }

            } catch(Exception){
                Console.WriteLine("Eingabe fehlerhaft");
            }

        }
        public static double getCubeSurface(double d){
            return 6*Math.Pow(d, 2);
        }
        public static double getCubeVolume(double d){
            return Math.Pow(d, 3);
        }

        public static double getBallSurface(double d){
            return Math.PI * Math.Pow(d, 2);
        }
        public static double getBallVolume(double d){
            return (Math.PI * Math.Pow(d, 3))/6;
        }

        public static double getOctSurface(double d){
            return 2*Math.Sqrt(3)*Math.Pow(d, 2);
        }
        public static double getOctVolume(double d){
            return (Math.Sqrt(2)*Math.Pow(d, 3))/3;
        }

        public static string GetCubeInfo(double d){
            return "Wuerfel: A=" + Math.Round(getCubeSurface(d), 2) + " | V=" + Math.Round(getCubeVolume(d), 2);
        }

        public static string GetBallInfo(double d){
            return "Kugel: A=" + Math.Round(getBallSurface(d), 2) + " | V=" + Math.Round(getBallVolume(d), 2);
        }

        public static string GetOctInfo(double d){
            return "Oktaeder: A=" + Math.Round(getOctSurface(d), 2) + " | V=" + Math.Round(getOctVolume(d), 2);
        }
        
    }
}