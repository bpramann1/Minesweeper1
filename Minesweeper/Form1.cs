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
        public Form1()
        {
            InitializeComponent();

            gameMap myNewGame; // used for testing
            //Uncomment one (or more) of the following to test each constructer of the object
            //myNewGame = new gameMap();
            //myNewGame = new gameMap(40);
            //myNewGame = new gameMap(25, 25);
            myNewGame = new gameMap(5,5,50);


        }
    }
}
