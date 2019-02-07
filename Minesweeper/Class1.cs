using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Minesweeper
{
    class gameMap
    {
        private int numberOfRows;                       //This variable indicates the number of rows. It has a default constructed value of 16 although it can be customized by user input
        private int numberOfColumns;                    //This variable indicates the number of Columns. It has a default constructed value of 16 although it can be customized by user input
        private int mineSizeInPixels;                   //This variable indicates the size of a possible mine space. It has a default constructed value of 20 although it can be customized by user input
        private int gameMapWidthInPixels;
        private int gameMapHeightInPixels;

        private Bitmap updateScreenBitmap;           //This variable is the bitmap we will update and then display to the screen when it is fully updated
        private Graphics updateScreenGraphics;       //This variable enables us to draw to the bitmap
        PictureBox bitmapContainer;                  //This variable is the object we will display the object in.
        Form Game;                                   //This is the object that will contain the game and game map


        public gameMap()                //Default Constuctor
        {
            numberOfRows = 16;          //Default Value
            numberOfColumns = 16;       //Default Value
            mineSizeInPixels = 20;      //Default Value

            
            createMap();            //All the variables are set up, so draw the map.
        }

        public gameMap(int rows, int columns)                //Constructor for custom number of rows and columns
        {
            numberOfRows = rows;          //custom number of rows
            numberOfColumns = columns;       //custom number of columns
            mineSizeInPixels = 20;      //Default Value


            createMap();            //All the variables are set up, so draw the map.
        }

        public gameMap(int rows, int columns, int mineSize)                //Constructor for custom number of rows and columns and custom mine size
        {
            numberOfRows = rows;          //custom number of rows
            numberOfColumns = columns;       //custom number of columns
            mineSizeInPixels = mineSize;      //custom mine size


            createMap();            //All the variables are set up, so draw the map.
        }

        public gameMap(int mineSize)                //Constructor for custom mine size
        {
            numberOfRows = 16;          //Default number of columns
            numberOfColumns = 16;       //Default number of columns
            mineSizeInPixels = mineSize;      //custom mine size


            createMap();            //All the variables are set up, so draw the map.
        }

        private void createMap()
        {
            gameMapWidthInPixels = (numberOfColumns * mineSizeInPixels) + 1;            //Width is calculated by the combined width of the mine spaces plus one for the ending line to create the ending mine
            gameMapHeightInPixels = (numberOfRows * mineSizeInPixels)+ 1;               //Height is calculated by the combined height of the mine spaces plus one for the ending line to create the ending mine
            Game = new Form();  
            Game.Show();
            Game.Width = gameMapWidthInPixels + 16;                     //16 is the added width of the edges of the form, so width is the width of the game map plus the width of the edges of the form               
            Game.Height = gameMapHeightInPixels + 38;                   //38 is the added height of the edges of the form, so height is the height of the game map plus the height of the edges of the form
            Game.Text = "Minesweeper";                                  //Names the title of the form to "Minesweeper"
            bitmapContainer = new PictureBox();
            bitmapContainer.Width = gameMapWidthInPixels;               //bitmap is just big enough to hold the game map
            bitmapContainer.Height = gameMapHeightInPixels;             //bitmap is just big enough to hold the game map
            Game.Controls.Add(bitmapContainer);                         //Add the PictureBox object called bitmapContainer to the form called Game. This makes the bitmapContainer be displayed in game.
            updateScreenBitmap = new Bitmap(gameMapWidthInPixels, gameMapHeightInPixels);
            updateScreenGraphics = Graphics.FromImage(updateScreenBitmap);      //This cause updateScreenGraphics to work with our bitmap

            updateScreenGraphics.FillRectangle(Brushes.Gold, 0, 0, gameMapWidthInPixels, gameMapHeightInPixels);        //Color the whole bitmap the color of the mine spaces
            
            

            
            for (int rowIndex = 0; rowIndex <= numberOfRows; rowIndex++)        // Iterate from 0 to the number of rows plus one. We will do plus one because we need to draw one more line than we have mine spaces.
            {
                updateScreenGraphics.DrawLine(Pens.Black, 0, rowIndex * mineSizeInPixels, gameMapWidthInPixels, rowIndex * mineSizeInPixels);       //Draw horizontal lines that are rowIndex mines down. This will cause all the horizontal lines of the mine spaces to be drawn.

            }
            for (int columnIndex = 0; columnIndex <= numberOfColumns; columnIndex++)
            {
                updateScreenGraphics.DrawLine(Pens.Black, columnIndex * mineSizeInPixels, 0, columnIndex * mineSizeInPixels, gameMapHeightInPixels);        //Draw vertical lines that are columnIndex mines right of the left of the screen. This will cause all the vertical lines of the mine spaces to be drawn.
            }
            bitmapContainer.Image = updateScreenBitmap;         //physically drawn our bitmap to the PictureBox which is in the form called game. Since we only physically draw once, this is very fast.
            Game.TopMost = true;                                //Make the game form be the topmost form.
        }
       
    }
}
