﻿using System;
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

            GameMap myNewGame; // used for testing
                               //Uncomment one (or more) of the following to test each constructer of the object
                               //myNewGame = new gameMap();
                               //myNewGame = new gameMap(40);
                               //myNewGame = new gameMap(25, 25);
                               //myNewGame = new gameMap(25,50,20);
                               // myNewGame = new gameMap(5, 10, 50);
                               myNewGame = new GameMap(40,80,15);
            //myNewGame = new GameMap(120, 240, 5);//Test this to test our speed of loading


        }
    }
}
