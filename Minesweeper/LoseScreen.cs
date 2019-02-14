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
    public partial class LoseScreen : Form
    {
        public LoseScreen()
        {
            InitializeComponent();
        }

        private void retryButton_Click(object sender, EventArgs e)
        {
            // Restart the game

        }

        private void quitButton_Click(object sender, EventArgs e)
        {
            // Close the form
            this.Close();
        }

        private void LoseScreen_Load(object sender, EventArgs e)
        //private void LoseScreen_FormClosing(object sender, FormClosingEventArgs e)
        //{
        //    Application.Exit();
        //}

    }
}
