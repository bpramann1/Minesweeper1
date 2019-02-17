using System;
using System.Media;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    /// <summary>
    /// Displays a screen that tells the player they've lost and allows them to play again 
    /// </summary>
    public partial class LoseScreen : Form
    {
        /// <summary>
        /// Constructs a new lose screen 
        /// </summary>
        public LoseScreen()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Presents the user with a new game menu 
        /// </summary>
        /// <param name="sender">
        /// Contains info about the object that called this function
        /// </param>
        /// <param name="e">
        /// Contains info about the event that caused the function to be called 
        /// </param>
        private void retryButton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Application.ExecutablePath);
            Environment.Exit(0);

        }

        /// <summary>
        /// Quits the game 
        /// </summary>
        /// <param name="sender">
        /// Contains info about the object that called this function
        /// </param>
        /// <param name="e">
        /// Contains info about the even that caused this function to be called 
        /// </param>
        private void quitButton_Click(object sender, EventArgs e)
        {
            // Close the form
            Environment.Exit(0);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
