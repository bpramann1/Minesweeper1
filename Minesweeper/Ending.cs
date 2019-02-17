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
        private void hitMine()
        {
            hitBomb = true;
        }
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
