/*
 * Class: EECS_448
 * Date: 2/11/19
 * Project_1: Minesweeper
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;

namespace Minesweeper
{
    /*
     * Class: Ending
     * Purpose: This class is designed to display an ending form that the user can interact with to either play again, retry, or quit the game. 
     * 
     * Methods:
     *      Constructors:
     *          public Ending(object sender): 
     *               @param 'object sender' - used to give access to Game class variables and data
     *          
     *          public Ending(object sender, bool endedByBomb)
     *               @param 'object sender' - see above
     *               @param 'bool endedByBomb' - used to indicate whether or not the player set off a bomb. constructor updates m_object and hitBomb private member variables.
     *              
     *      General:
     *          public void hitMine()
     *               @call - sets private member variable 'hitBomb' to value true.
     *          
     *          public void displayEnd()
     *               @call - displays either a win screen or a lose screen based on the value of hitBomb, if it is a lose screen the retroExplosion.wav should play. 
     *               currently also plays retroExplosion.wav when the win screen is shown however final version will have a more appropriate file.
     */

    /// <summary>
    /// This class is designed to display an ending form that the user can interact with to either play again, retry, or quit the game. 
    /// </summary>
    class Ending
    {
        private bool gameWon;
        private bool hitBomb;
        private int totalNumberOfBombsLeft;
        private int totalNumberOfSafeSpacesLeft;
        private int numberOfColumns;
        private int numberOfRows;


        private GameMap.MineSpaceStates[,] stateOfMineSpace;
        private bool[,] containsMine;

        /// <summary>
        /// Creates a new ending screen 
        /// </summary>
        /// <param name="sender">
        /// used to give access to Game class variables and data
        /// </param>
        /// <param name="TotalNumberOfBombsLeft">
        /// The total number of bombs remaining in the game
        /// </param>
        /// <param name="TotalNumberOfSafeSpacesLeft">
        /// The total number of safe spaces left in the game 
        /// </param>
        /// <param name="NumberOfColumns">
        /// The number of columns in the game
        /// </param>
        /// <param name="NumberOfRows">
        /// The number of rows in the game 
        /// </param>
        /// <param name="StateOfMineSpace">
        /// An array that contains the states of each space
        /// </param>
        /// <param name="ContainsMine">
        /// An array that contains information about whether a space contains a mine 
        /// </param>
        public Ending(GameMap sender, int TotalNumberOfBombsLeft, int TotalNumberOfSafeSpacesLeft, int NumberOfColumns, int NumberOfRows, GameMap.MineSpaceStates[,] StateOfMineSpace, bool[,] ContainsMine)
        {
            totalNumberOfBombsLeft = TotalNumberOfBombsLeft;
            totalNumberOfSafeSpacesLeft = TotalNumberOfSafeSpacesLeft;
            numberOfColumns = NumberOfColumns;
            numberOfRows = NumberOfRows;
            stateOfMineSpace = StateOfMineSpace;
            containsMine = ContainsMine;
            endResult(sender);
        }

        /// <summary>
        /// Creates a new ending screen 
        /// </summary>
        /// <param name="sender">
        /// used to give access to Game class variables and data
        /// </param>
        /// <param name="endedByBomb">
        /// A flag that tells the function whether the player won or lost 
        /// </param>
        /// <param name="TotalNumberOfBombsLeft">
        /// The total number of bombs remaining in the game 
        /// </param>
        /// <param name="TotalNumberOfSafeSpacesLeft">
        /// The total number of safe spaces left in the game 
        /// </param>
        /// <param name="NumberOfColumns">
        /// The number of columns in the game 
        /// </param>
        /// <param name="NumberOfRows">
        /// The number of rows in the game 
        /// </param>
        /// <param name="StateOfMineSpace">
        /// An array that contains the states of each space
        /// </param>
        /// <param name="ContainsMine">
        /// An array that contains information about whether a space contains a mine
        /// </param>
        public Ending(GameMap sender, bool endedByBomb, int TotalNumberOfBombsLeft, int TotalNumberOfSafeSpacesLeft, int NumberOfColumns, int NumberOfRows, GameMap.MineSpaceStates[,] StateOfMineSpace, bool[,] ContainsMine)
        {
            totalNumberOfBombsLeft = TotalNumberOfBombsLeft;
            totalNumberOfSafeSpacesLeft = TotalNumberOfSafeSpacesLeft;
            numberOfColumns = NumberOfColumns;
            numberOfRows = NumberOfRows;
            stateOfMineSpace = StateOfMineSpace;
            containsMine = ContainsMine;
            if (endedByBomb == true)
            {
                hitBomb = true;
            }
            else
            {
                hitBomb = false;
            }
            endResult(sender);
        }

        /// <summary>
        /// Used to determine if the game ended by hitting a bomb 
        /// </summary>
        private void hitMine()
        {
            hitBomb = true;
        }

        /// <summary>
        /// Determines if the player won or lost and displays the correct form. 
        /// </summary>
        /// <param name="m_GameMap">
        /// The game the player was playing 
        /// </param>
        private void endResult(GameMap m_GameMap)
        {
            if (hitBomb == true)
            {
                LoseScreen lScreen;
                lScreen = new LoseScreen();
                lScreen.TopMost = true;
                System.Media.SoundPlayer player = new System.Media.SoundPlayer(Properties.Resources.retroExplosion);
                player.Play();
                lScreen.Show();
                m_GameMap.EndReveal();
                totalNumberOfBombsLeft = 0;
                totalNumberOfSafeSpacesLeft = 0;
                m_GameMap.NumberOfSafeSpacesLeftLabel.Text = totalNumberOfSafeSpacesLeft.ToString() + " safe spaces left";
                m_GameMap.NumberOfBombsLeftLabel.Text = totalNumberOfBombsLeft.ToString() + " bombs left";
            }
            else
            {
                int rows = numberOfRows;
                int cols = numberOfColumns;
                bool gameLost = false;
                for (int i = 0; i < cols; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        if ((stateOfMineSpace[i, j] == GameMap.MineSpaceStates.FlaggedAsUnsafe) && (containsMine[i, j] == false))
                        {
                            gameLost = true;
                            break;
                        }
                    }
                }
                if (gameLost == true)
                {
                    LoseScreen lScreen;
                    lScreen = new LoseScreen();
                    lScreen.TopMost = true;
                    System.Media.SoundPlayer player = new System.Media.SoundPlayer(Properties.Resources.retroExplosion);
                    player.Play();
                    lScreen.Show();
                    m_GameMap.EndReveal();
                    totalNumberOfBombsLeft = 0;
                    totalNumberOfSafeSpacesLeft = 0;
                    m_GameMap.NumberOfSafeSpacesLeftLabel.Text = totalNumberOfSafeSpacesLeft.ToString() + " safe spaces left";
                    m_GameMap.NumberOfBombsLeftLabel.Text = totalNumberOfBombsLeft.ToString() + " bombs left";
                }
                else
                {
                    WinScreen wScreen;
                    wScreen = new WinScreen();
                    System.Media.SoundPlayer player = new System.Media.SoundPlayer(Properties.Resources.won_game);
                    player.Play();
                    wScreen.TopMost = true;
                    wScreen.Show();
                    m_GameMap.EndReveal();
                    totalNumberOfBombsLeft = 0;
                    totalNumberOfSafeSpacesLeft = 0;
                    m_GameMap.NumberOfSafeSpacesLeftLabel.Text = totalNumberOfSafeSpacesLeft.ToString() + " safe spaces left";
                    m_GameMap.NumberOfBombsLeftLabel.Text = totalNumberOfBombsLeft.ToString() + " bombs left";
                }

            }


        }
    }
}
