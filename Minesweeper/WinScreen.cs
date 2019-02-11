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
            this.Close();
        }

        private void playAgainButton_Click(object sender, EventArgs e)
        {

        }
    }
}
