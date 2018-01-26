using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using static FPBWordScramble.WordScrambleExceptions;


/*Fernando Pereira Borges
 * Student id: 7258262
 * Assignment 3 & 4
 * Purpose: Create a multiplayer scramble game
 * */

namespace FPBWordScramble
{
    [ServiceBehavior]
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class WordScramble : IWordScramble
    {
        // the maximum number of players allowed playing simultaneously
        private const int MAX_PLAYERS = 5;
        // the user hosting the game. If it’s null nobody is hosting the game.
        private static String userHostingTheGame = null;
        // the Word object that contains the scrambled and unscrambled words
        private static Word gameWords;
        // the list of players playing the game
        private static List<String> activePlayers = new List<string>();


        [OperationBehavior]
        public bool isGameBeingHosted()
        {
           
            return userHostingTheGame != null;

        }

        [OperationBehavior]
        public string hostGame(String playerName, String wordToScramble)
        {
         
            gameWords = new Word();

            if (isGameBeingHosted())
            {
                GameBeingHostedException fault = new GameBeingHostedException();
              
                fault.HostMessage = "Game is already being hosted";
                throw new FaultException<GameBeingHostedException>(fault);

                
            }
            else
            {
                gameWords.unscrambledWord = wordToScramble;
                gameWords.scrambledWord = scrambleWord(wordToScramble);
                userHostingTheGame = playerName;
                return playerName;
            }
        }

        [OperationBehavior]
        public Word join(string playerName)
        {
         
            // Check number of players
            if (!isGameBeingHosted())
            {
                GameIsNotBeingHostedException nonHostFault = new GameIsNotBeingHostedException
                {
                    NonHostMessage = "There is no one hosting the game"
                };
                throw new FaultException<GameIsNotBeingHostedException>(nonHostFault);

            }

            else if (playerName == userHostingTheGame)
            {
                HostCannotJoinTheGameException hostFault = new HostCannotJoinTheGameException
                {
                    HostJoinMessage = "The host cannot join the game as a player "
                };
                throw new FaultException<HostCannotJoinTheGameException>(hostFault);
            }

            else if (activePlayers.Count() >= MAX_PLAYERS)
            {
                MaximumNumberOfPlayersReachedException maxFault = new MaximumNumberOfPlayersReachedException
                {
                    MaxPlayersMessage = "maximum number of players has been reached"
                };
                throw new FaultException<MaximumNumberOfPlayersReachedException>(maxFault);
            }
            else
            {
                activePlayers.Add(playerName);

            }
            return gameWords;

        }

        [OperationBehavior]
        public bool guessWord(string playerName, string guessedWord, string unscrambledWord)
        {
           
            if (activePlayers.Contains(playerName) == false)
            {
                PlayerNotPlayingTheGameException playerFault = new PlayerNotPlayingTheGameException
                {
                    PlayerNotPlaying = "Person is not playing the game"
                };
                throw new FaultException<PlayerNotPlayingTheGameException>(playerFault);
            }
            else if (guessedWord != gameWords.unscrambledWord)
            {
                return false;
            }
            else

                return true;
        }

        // Utility function to scramble a word
        private string scrambleWord(string word)
        {
            char[] chars = word.ToArray();
            Random r = new Random(2011);
            for (int i = 0; i < chars.Length; i++)
            {
                int randomIndex = r.Next(0, chars.Length);
                char temp = chars[randomIndex];
                chars[randomIndex] = chars[i];
                chars[i] = temp;
            }
            return new string(chars);
        }
    }
}
