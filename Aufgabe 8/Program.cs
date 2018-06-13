using System;
using System.Collections.Generic;

namespace Aufgabe_8
{
    class Program
    {
        static void Main(string[] args)
        {
            TicTacToe Game = new TicTacToe(){};
            Game.Spiel();
        }
    }

    public class TicTacToe 
    {
        public string player;
        public string currentLetter;
        public string currentPlayer;
        public string playerOne;
        public string playerTwo;
        public string eins = " ";
        public string zwei = " ";
        public string drei = " ";
        public string vier= " ";
        public string fuenf= " ";
        public string sechs= " ";
        public string sieben= " ";
        public string acht= " ";
        public string neun= " ";
        public int rundenZaehler = 2;
        public void Spiel()
        {   
            Console.WriteLine("Wie heisst Spieler 1?"); 
            Console.WriteLine("> ");
            playerOne = Console.ReadLine();
            Console.WriteLine("Wie heisst Spieler 2?"); 
            Console.WriteLine("> ");
            playerTwo = Console.ReadLine();
                
            Console.WriteLine("Welches Feld moechtest du belegen ? Schreibe einfach eine Zahl von 1 bis 9 fuer die entsprechende Postition, nach folgendem System:");
            Console.WriteLine("  - - -" + "\n" + "| 1 2 3 |" +"\n" + "| 4 5 6 |" + "\n" + "| 7 8 9 |"+ "\n" + "  - - -"); 
            
            for (;;) 
            {
                string currentPlayer = Spieler(playerOne, playerTwo);
                Console.WriteLine(currentPlayer + "(" + currentLetter + ")" + " ist dran"); 
                Console.WriteLine("> ");
                string positionsEingabe = Console.ReadLine();
                bool AenderungAmSpielfeld = false;

                switch(positionsEingabe)
                {
                    case "1" :
                        if(IstFeldFrei(eins) == true)
                        {
                            eins = currentLetter;
                            AenderungAmSpielfeld = true;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Das Feld ist belegt. Bitte waehle ein Neues.");
                        }    
                        break;

                    case "2" :
                        if(IstFeldFrei(zwei) == true)
                        {
                            zwei = currentLetter;
                            AenderungAmSpielfeld = true;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Das Feld ist belegt. Bitte waehle ein Neues.");
                        }   
                        break; 

                    case "3":
                        if(IstFeldFrei(drei) == true)
                        {
                            drei = currentLetter;
                            AenderungAmSpielfeld = true;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Das Feld ist belegt. Bitte waehle ein Neues.");
                        }   
                        break;

                    case "4":
                        if(IstFeldFrei(vier) == true)
                        {
                            vier = currentLetter;
                            AenderungAmSpielfeld = true;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Das Feld ist belegt. Bitte waehle ein Neues.");
                        }   
                        break;    

                    case "5":
                        if(IstFeldFrei(fuenf) == true)
                        {
                            fuenf = currentLetter;
                            AenderungAmSpielfeld = true;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Das Feld ist belegt. Bitte waehle ein Neues.");
                        }   
                        break;

                    case "6":
                        if(IstFeldFrei(sechs) == true)
                        {
                            sechs = currentLetter;
                            AenderungAmSpielfeld = true;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Das Feld ist belegt. Bitte waehle ein Neues.");
                            Console.WriteLine("> ");
                        }   
                        break;
                    
                    case "7":
                        if(IstFeldFrei(sieben) == true)
                        {
                            sieben = currentLetter;
                            AenderungAmSpielfeld = true;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Das Feld ist belegt. Bitte waehle ein Neues.");
                            Console.WriteLine("> ");
                        }   
                        break;

                    case "8":
                        if(IstFeldFrei(acht) == true)
                        {
                            acht = currentLetter;
                            AenderungAmSpielfeld = true;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Das Feld ist belegt. Bitte waehle ein Neues.");
                            Console.WriteLine("> ");
                        }   
                        break;

                    case "9":
                        if(IstFeldFrei(neun) == true)
                        {
                            neun = currentLetter;
                            AenderungAmSpielfeld = true;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Das Feld ist belegt. Bitte waehle ein Neues.");
                            Console.WriteLine("> ");
                        }   
                        break;
                    
                    default :
                        Console.WriteLine("Falsche Eingabe, bitte gib eine Zahl von 1-9 ein."); 
                        Console.WriteLine("> "); 
                        break;
                }

                if(AenderungAmSpielfeld == true)
                {
                    rundenZaehler++;
                    Console.WriteLine(" - - - -" + "\n" + "|" + " " + eins + " " + zwei + " " + drei + " " + "|" +"\n" + "|" + " " + vier + " " + fuenf + " " + sechs + " " + "|" + "\n" + "|" + " " + sieben + " " + acht + " " + neun + " " + "|"+ "\n" + " - - - -");
                }

                if(SpielZuEnde() == true)
                {
                    break;
                }
            }
        }

        public string Spieler(string playerOne, string playerTwo)
        {

            if(rundenZaehler % 2 == 0)
            {
                player = playerOne;
                currentLetter = "x";
            }
            else
            {
                player = playerTwo;
                currentLetter = "o";
            }
            return player;
        }
        public bool IstFeldFrei(string zahl)
        {
            if (zahl == " ")
            return true;
            else
            return false;
        }

       /* public string Spielfeld()
        {
           // string ausgabe = "---" + "\n" + "|" + eins + " " + zwei + " " + drei + "|" +"\n" + "|" + " " + vier + " " + fuenf + " " + sechs + " " + "|" + "\n" + "|" + " " + sieben + " " + acht + " " + neun + " " + "|"+ "\n" + "---";
            return null;
        }       */

        public bool SpielZuEnde()
        {
            if (GewinnMuster() == true)
            {
                Console.WriteLine(player + " gewinnt!");
                return true;
            }
            
            if(eins!=" "&&zwei!=" "&&drei!= " "&&vier!= " "&&fuenf!= " "&&sechs!= " "&&sieben!= " "&&acht!= " "&&neun!= " ")
            {
                Console.WriteLine(" - - - -" + "\n" + "|" + " " + eins + " " + zwei + " " + drei + " " + "|" +"\n" + "|" + " " + vier + " " + fuenf + " " + sechs + " " + "|" + "\n" + "|" + " " + sieben + " " + acht + " " + neun + " " + "|"+ "\n" + " - - - -");
                Console.WriteLine("Das Spielfeld ist voll. Das bedeutet unentschieden.");
                Console.WriteLine("Noch eine Runde ? Tippe 'y' oder 'n'");
                Console.WriteLine("> ");
                string nochNeRunde = Console.ReadLine();
                if(nochNeRunde == "y")
                {
                    eins = " ";
                    zwei = " ";
                    drei = " ";
                    vier= " ";
                    fuenf= " ";
                    sechs= " ";
                    sieben= " ";
                    acht= " ";
                    neun= " ";
                    rundenZaehler = 3;
                    Console.WriteLine(" - - - -" + "\n" + "|" + " " + eins + " " + zwei + " " + drei + " " + "|" +"\n" + "|" + " " + vier + " " + fuenf + " " + sechs + " " + "|" + "\n" + "|" + " " + sieben + " " + acht + " " + neun + " " + "|"+ "\n" + " - - - -");
                    return false;
                }
                if(nochNeRunde == "n")
                Console.WriteLine("Dann bleibt es halt ein unentschieden. Ihr sind gleich schlecht." );
                
                return true;
            }
            return false;
        }

        public bool GewinnMuster()
        {
            //===
            if(eins==zwei&&zwei==drei)
            {
                if(eins != " "&&zwei != " "&&drei != " ")
                {
                    return true;
                }
            }

            if(vier==fuenf&&fuenf==sechs)
            {
                if(vier != " ")
                {
                    return true;
                }
            }

            if(sieben==acht&&acht==neun)
            {
                if(sieben != " ")
                {
                    return true;
                }
            }

            // +
            // +
            // +
            if(eins==vier&&vier==sieben)
            {
                if(eins != " ")
                {
                    return true;
                }
            }

            if(zwei==fuenf&&fuenf==acht)
            {
                if(zwei != " ")
                {
                    return true;
                }
            }

            if(drei==sechs&&sechs==neun)
            {
                if(drei != " ")
                {
                    return true;
                }
            }
           
           //+  
           //  + 
           //    +
            if(eins==fuenf&&fuenf==neun)
            {
                if(eins != " ")
                {
                    return true;
                }
            }
          
            //    +
            //  +
            //+
            if(drei==fuenf&&fuenf==sieben)
            {
                if(drei != " ")
                {
                    return true;
                }
            }
            return false;
        }
    }
}
