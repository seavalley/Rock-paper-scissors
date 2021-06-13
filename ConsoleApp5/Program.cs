using System;
using System.Security.Cryptography;
using System.Linq;
using System.Text;

namespace ConsoleApp5
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] moves;
            moves = args;
            while (true)
            {                
                string[] distinct = moves.Distinct().ToArray();
                if (moves.Length < 3)
                {
                    Console.WriteLine("Enter 3 or more parameters, FOR EXAMPLE\nrock scissors paper\nOR\nrock scissors paper lizard spock:");
                }
                else if (moves.Length % 2 == 0)
                {
                    Console.WriteLine("Enter an odd number of parameters, FOR EXAMPLE\nrock scissors paper\nOR\nrock scissors paper lizard spock:");
                }
                else if (moves.Length != distinct.Length)
                {
                    Console.WriteLine("Enter distinct parameters, FOR EXAMPLE\nrock scissors paper\nOR\nrock scissors paper lizard spock:");
                }
                else break;
                moves = Console.ReadLine().Split(' ');
            }
            Menu(moves);
        }

        public static string ConvertByteToString (byte[] bytes)
        {
            string result = BitConverter.ToString(bytes).Replace("-", "").ToLower(); ;
            return result;
        }

        public static byte[] GenerateKey()
        {
            byte[] bytes = new byte[16];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return bytes;
        }
        public static byte[] GetHMAC(byte[] message, byte[] key)
        {
            var hmac = new HMACSHA256(key);
            return hmac.ComputeHash(message);
        }

        public static void GetWinner(int usermove, int computermove, string[] moves)
        {
            if ((usermove < computermove && computermove - usermove <= moves.Length / 2) || (usermove > computermove && usermove - computermove > moves.Length / 2))
            {
                Console.WriteLine("YOU LOSE :(\nBETTER LUCK NEXT TIME!");
            }
            else if (usermove == computermove)
            {
                Console.WriteLine("IT'S A TIE!");
            }
            else
            {
                Console.WriteLine("YOU WON!\nCONGRATS!");
            }
        }

        public static void Menu(string[] moves)
        {
            Console.Clear();
            Random r = new Random();
            int compchoice = r.Next(0, moves.Length);
            byte[] key = GenerateKey();
            Console.WriteLine($"HMAC: {ConvertByteToString(GetHMAC(Encoding.Default.GetBytes(moves[compchoice]), key))}");
            Console.WriteLine("Available moves");
            for (int i = 0; i < moves.Length; i++)
            {
                Console.WriteLine($"{i+1} - {moves[i]}");
            }
            Console.WriteLine("0 - Exit");
            Console.WriteLine("Enter your move:");
            string move = Console.ReadLine();
            if (move == "0")
            {
                Console.WriteLine("Have a good day!");
                return;
            }
            else if (int.TryParse(move, out int result) && moves.Length - result >= 0)
            {
                Console.WriteLine($"Your choice - {moves[result - 1]}");
                Console.WriteLine($"Computer choice - {moves[compchoice]}");
                GetWinner(result - 1, compchoice, moves);
                Console.WriteLine($"HMAC KEY: {ConvertByteToString(key)}");
            }
            else
            {
                Menu(moves);
            }
        }
    }
}
