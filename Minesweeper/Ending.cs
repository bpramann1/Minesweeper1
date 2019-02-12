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
     *               @param 'object sender' - object sender inherits vital information about the gamemap including 
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
                wScreen.TopMost = true;
                wScreen.Show();
            }
        }


    }
}
