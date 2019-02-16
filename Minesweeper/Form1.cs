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
    public partial class Form1 : Form
    {
        public GameMap game;


        public Form1()
        {
            InitializeComponent();

            //GameMap myNewGame; // used for testing
            //Uncomment one (or more) of the following to test each constructer of the object
            // myNewGame = new GameMap(40,80,15, 30);
            // myNewGame = new GameMap(120, 240, 5, 6000);//Test this to test our speed of loading

        }

        private void continueButton_Click(object sender, EventArgs e)
        {
            var numRows = 0;
            var numCols = 0;
            var numMines = 0;
            var rowsParsed = Int32.TryParse(this.rowTextBox.Text, out numRows);
            var colsParsed = Int32.TryParse(this.columnTextBox.Text, out numCols);
            var minesParsed = Int32.TryParse(this.mineTextBox.Text, out numMines);
            var numericInput = rowsParsed && colsParsed && minesParsed;
            var validNumberOfMines = numMines > 0 && numMines < (numRows * numCols);

            if (numericInput)
            {
                if (numRows>0 && numRows<=50)
                {
                    if (numCols > 0 && numCols <= 50)
                    {
                        if (validNumberOfMines)
                        {
                            game = new GameMap(numRows, numCols, 50, numMines);
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show(this, "Invalid Number of mines");
                        }
                    }
                    else
                    {
                        MessageBox.Show(this, "Invalid number or columns. Please enter a number between 0 and 50.");
                    }
                }
                else
                {
                    MessageBox.Show(this, "Invalid number or rows. Please enter a number between 0 and 50.");
                }
            }
            else
            {
                MessageBox.Show(this, "Non-Numeric Input");
            }
        }

    }
}
