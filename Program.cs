using System.ComponentModel;
using System.Data;
using System.Reflection.Metadata.Ecma335;

namespace Wordle
{
	class Program
	{
		public static class GlobalVariables
		{
			public static bool[] correctGuess = new bool[5];
			public static string ga;
        }
		static void Main(string[] arg)
		{
			Boot();
			Start();
		}

        static void Start()
        {
			Console.BackgroundColor = ConsoleColor.Magenta;
			Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Douglas's Wordle");
			Console.ResetColor();
            Console.WriteLine("1 - Play");
            Console.WriteLine("0 - Exit");

            String choice = Console.ReadLine() ?? ""; // [?? ""] removes the null warning.

            if (choice.Equals("1"))
            {
                Play();
            }
            else if (choice.Equals("0"))
            {
                Exit();
            }
            else
            {
                Console.WriteLine("Hey");
            }
        }

		static void Boot()
		{
			String aa = "AugeAgora";
			Console.BackgroundColor = ConsoleColor.DarkMagenta;
			Console.ForegroundColor = ConsoleColor.White;
			for (int i = 0; i < 9; i++)
			{
				Console.Write(aa[i]);
				Thread.Sleep(100);
			}
            Thread.Sleep(200);
            Console.ResetColor();
			Console.Clear();
		}

        static void Exit()
		{
			Console.Clear();
            String aa = "Goodbye!";
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < 8; i++)
            {
                Console.Write(aa[i]);
                Thread.Sleep(100);
            }
            Thread.Sleep(200);
            Console.ResetColor();
            Console.Clear();
            Environment.Exit(1);
		}

		static void Play()
		{
			Console.Clear();
			
			for (int i = 0; i < 5; i++)
			{
				GlobalVariables.correctGuess[i] = false;
			}
			GlobalVariables.ga = GenerateAnswer();
			char[] answer = Answer();
			char[] validGuess;
			Console.BackgroundColor = ConsoleColor.DarkYellow;
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine("Please type in a five letter word and press ENTER");
			Console.ResetColor();
            

            int guessesMade = 0;
			bool win = false;
			
			while(win == false)
			{
				if (GlobalVariables.correctGuess.Contains(false) == false)
				{
					win = true;
					break;
				}

				if (guessesMade == 6 && win == false)
				{
					break;
				}
                answer = Answer(); // Reset each time
                validGuess = GuessCheck();
                Respond(validGuess, answer);
				guessesMade++;
            }
			
			if (win == true)
			{
				Win(guessesMade);
			}
			else
			{
				Lose();
			}
        }

		static void Win(int guessesMade)
		{
			Console.BackgroundColor = ConsoleColor.Green;
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine($"\nHorray, you found the word in {guessesMade} tries!");
			Console.ResetColor();
			Console.ReadLine();
			Console.Clear();
			Start();
		}

		static void Lose()
		{
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nt r a g i c");
            Console.ResetColor();
            Console.ReadLine();
            Console.Clear();
            Start();
        }

		static char[] Answer()
		{
			char[] answer = GlobalVariables.ga.ToCharArray();
            return answer;
		}

		static string GenerateAnswer()
		{
            Random random = new Random();
            int randomNumber = random.Next(0, 6);
            string[] answerPool = { "stoat", "drunk", "happy", "furry", "goons", "funny" };
            string randomAnswer = answerPool[randomNumber];
			return randomAnswer;
        }

		static char[] GuessCheck()
		{
            Console.ForegroundColor = ConsoleColor.Cyan;
            String guess = Console.ReadLine().ToLower();
            Console.ResetColor();

            char[] validGuess = {};

			while (guess.Length !=5)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Invalid Guess");
				Console.ForegroundColor = ConsoleColor.Cyan;
				guess = Console.ReadLine() ?? "";
				Console.ResetColor();
			}
			validGuess = guess.ToCharArray();
			return validGuess;
        }

		static void Respond(char[] vg, char[] answer)
		{
			for (int i = 0; i < 5; i++)
			{
				if (vg[i] == answer[i])
				{
					Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(vg[i]);
					Console.ResetColor();
                    Console.Write(" ");
					GlobalVariables.correctGuess[i] = true;
                }
				else if (answer.Contains(vg[i]))
				{
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(vg[i]);
                    Console.ResetColor();
                    Console.Write(" ");
                    GlobalVariables.correctGuess[i] = false;
                }
				else
				{
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(vg[i]);
                    Console.ResetColor();
                    Console.Write(" ");
                    GlobalVariables.correctGuess[i] = false;
                }
                // Changes the value in answer so that when checking if answer contains
                // x char it doesn't say a char is within the array multiple times incorrectly.
                answer[i] = '0';
			}
            Console.WriteLine();
        }
	}
}