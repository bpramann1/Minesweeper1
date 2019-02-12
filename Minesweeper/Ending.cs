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
        private object m_object;

        public Ending(object sender)
        {
            m_object = sender;
        }
        public Ending(object sender, bool endedByBomb)
        {
            m_object = sender;
            if (endedByBomb == true)
            {
                hitBomb = true;
            }
            else
            {
                hitBomb = false;
            }
        }
        public void hitMine()
        {
            hitBomb = true;
        }
        public void displayEnd()
        {
            if (hitBomb == true)
            {
                //make a form that pops up and shows an ending if the player hit the bomb. 
                LoseScreen lScreen;
                lScreen = new LoseScreen();
                lScreen.TopMost = true;
                System.Media.SoundPlayer player = new System.Media.SoundPlayer(Properties.Resources.retroExplosion);
                player.Play();
                lScreen.Show();
            }
            else
            {
                //make a form that pops up and shows and ending if the player did not hit a bomb.
                WinScreen wScreen;
                wScreen = new WinScreen();
                System.Media.SoundPlayer player = new System.Media.SoundPlayer(Properties.Resources.retroExplosion);
                player.Play();
                wScreen.TopMost = true;
                wScreen.Show();
            }
        }


    }
}
