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
            System.Diagnostics.Process.Start(Application.ExecutablePath);
            Application.Exit();

        }

        private void quitButton_Click(object sender, EventArgs e)
        {
            // Close the form
            Application.Exit();
        }

        //private void LoseScreen_FormClosing(object sender, FormClosingEventArgs e)
        //{
        //    Application.Exit();
        //}

    }
}
