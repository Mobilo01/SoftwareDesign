using System;

namespace Aufgabe_3
{
    class Program
    {
        public static int binary;
        public static int sum = 0;
        public static int factor = 1;
        

        static void Main(string[] args)
        
        {
            int number = int.Parse(args[0]);
            int fromBase = int.Parse(args[1]);
            int toBase = int.Parse(args[2]);
            int result = ToBaseFromBase(number, toBase, toBase);
            Console.WriteLine($"Die Zahl {number} im {fromBase}er-System entspricht im {toBase}er-System der Zahl: {result}.");
        }

        static int ToHexal(int dec)
        {
            return ToBaseFromDecimal(dec, 6);
        }

        static int ToDecimal(int hex)
        {
            return ToDecimalFromBase(hex, 6);
        }

        static int ToBaseFromBase(int toBase, int fromBase, int divider)
        {
            return ToBaseFromDecimal(toBase, ToDecimalFromBase(fromBase, divider));
        }

        public static int ToBaseFromDecimal(int toBase, int divider)
        {
            int result = toBase / divider;
            int value = toBase % divider;

            if (result == 0)
            {
                return value;
            }
            binary = ((ToBaseFromDecimal(result,divider)*10)+value);
            return binary;
        }

        public static int ToDecimalFromBase(int fromBase, int divider)
        {
            int modulo = fromBase % 10;
            int value = fromBase / 10;

            if (value == 0)
            {
                sum = sum + (modulo * factor);
                return sum;
            }

            if (value != 0)
            {
                sum = sum + (modulo * factor);
                factor = factor * value;
            }
            return ((ToDecimalFromBase(value,divider))+modulo);
        }
    }
}
