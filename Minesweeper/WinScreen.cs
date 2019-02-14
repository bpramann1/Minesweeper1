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
    public partial class WinScreen : Form
    {
        public WinScreen()
        {
            InitializeComponent();
        }

        private void quitButton_Click(object sender, EventArgs e)
        {
            // Quit form
            Application.Exit();
        }

        private void playAgainButton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Application.ExecutablePath);
            Application.Exit();
        }
        //private void WinScreen_FormClosing(object sender, FormClosingEventArgs e)
        //{
        //    Application.Exit();
        //}
    }
}
