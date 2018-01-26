using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FPBWordScrambleClient.WordScrambleServiceReference;
using System.ServiceModel;

/*Fernando Pereira Borges
 * Student id: 7258262
 * Assignment 3 & 4
 * Purpose: Create a multiplayer scramble game
 * */


namespace FPBWordScrambleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            WordScrambleClient proxy = new WordScrambleClient();

            bool canPlayGame = true;

            Console.WriteLine("Player's name?");
            String playerName = Console.ReadLine();

            try
            {
                if (!proxy.isGameBeingHosted())
                {
                    Console.WriteLine("Welcome " + playerName +
                               "! Do you want to host the game?");
                    if (Console.ReadLine().CompareTo("yes") == 0)
                    {
                        Console.WriteLine("Type the word to scramble.");
                        string inputWord = Console.ReadLine();
                        string scrambledWord = proxy.hostGame(playerName, inputWord);
                        canPlayGame = false;
                        Console.WriteLine("You're hosting the game with word '" + inputWord + "' scrambled as '" + scrambledWord + "'");
                        Console.ReadKey();
                    }
                }
                if (canPlayGame)
                {
                    Console.WriteLine("Do you want to play the game?");
                    if (Console.ReadLine().CompareTo("yes") == 0)
                    {
                        Word gameWords = proxy.join(playerName);
                        Console.WriteLine("Can you unscramble this word? => " + gameWords.scrambledWord);
                        String guessedWord;
                        bool gameOver = false;
                        while (!gameOver)
                        {
                            guessedWord = Console.ReadLine();
                            gameOver = proxy.guessWord(playerName, guessedWord, gameWords.unscrambledWord);
                            if (!gameOver)
                            {
                                Console.WriteLine("Nope, try again...");
                            }
                        }
                        Console.WriteLine("You WON!!!");
                        Console.WriteLine("");
                        Console.WriteLine("See you later :)");
                        Console.ReadLine();
                        Environment.Exit(0);
                    }
                }
            }
            catch (FaultException<WordScrambleExceptionsGameBeingHostedException> e)
            {
                Console.WriteLine("The application terminated with an error.");             
                Console.WriteLine("Error Message: {0}", e.Detail.HostMessage);
                Console.ReadLine();

            }
            catch (FaultException<WordScrambleExceptionsGameIsNotBeingHostedException> ex)
            {
                Console.WriteLine("The application terminated with an error.");
                Console.WriteLine("Error Message: {0}", ex.Detail.NonHostMessage);
                Console.ReadLine();

            }
            catch (FaultException<WordScrambleExceptionsHostCannotJoinTheGameException> ex)
            {
               
                Console.WriteLine("The application terminated with an error.");
                Console.WriteLine("Error Message: {0}", ex.Detail.HostJoinMessage);
                Console.ReadLine();

            }
            catch (FaultException<WordScrambleExceptionsMaximumNumberOfPlayersReachedException> ex)
            {
               
                Console.WriteLine("The application terminated with an error.");
                Console.WriteLine("Error Message: {0}", ex.Detail.MaxPlayersMessage);
                Console.ReadLine();

            }
            catch (FaultException<WordScrambleExceptionsPlayerNotPlayingTheGameException> ex)
            {
             
                Console.WriteLine("The application terminated with an error.");
                Console.WriteLine("Error Message: {0}", ex.Detail.PlayerNotPlaying);
                Console.ReadLine();

            }
        }
    }
}
