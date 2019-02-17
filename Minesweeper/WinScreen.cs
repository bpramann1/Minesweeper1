using System;
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
    /// Displays a window that lets the user know they've won and presents them with the option to play again 
    /// </summary>
    public partial class WinScreen : Form
    {
        /// <summary>
        /// Constructs a new win screen 
        /// </summary>
        public WinScreen()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Closes the application 
        /// </summary>
        /// <param name="sender">
        /// Contains info about the object that called this function 
        /// </param>
        /// <param name="e">
        /// Contains info about the event that caused this function to be called 
        /// </param>
        private void quitButton_Click(object sender, EventArgs e)
        {
            // Quit form
            Environment.Exit(0);
        }

        /// <summary>
        /// Presents the user with a new game menu 
        /// </summary>
        /// <param name="sender">
        /// Contains info about the object that called this function 
        /// </param>
        /// <param name="e">
        /// Contains info about the event that caused this function to be called 
        /// </param>
        private void playAgainButton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Application.ExecutablePath);
            Environment.Exit(0);
        }

    }
}
