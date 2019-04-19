using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    /// <summary>
    /// This is class creates the graphics for the game map. It draws its changes onto a bitmap that is the picture that the user sees as the game.
    /// </summary>
    public class DrawMap
    {
        private Bitmap updateScreenBitmap;         //This variable is the bitmap we will update and then display to the screen when it is fully updated
        private Graphics updateScreenGraphics;   //This variable enables us to draw to the bitmap
        private GameMap caller;
        private int mapWidthInPixels;
        private int mapHeightInPixels;
        private int numberOfRows;                     //This variable indicates the number of rows. It has a default constructed value of 16 although it can be customized by user input
        private int numberOfColumns;                    //This variable indicates the number of Columns. It has a default constructed value of 16 although it can be customized by user input
        private int mineSizeInPixels;                   //This variable indicates the size of a possible mine space. It has a default constructed value of 20 although it can be customized by user input
        private Font font;
        /// <summary>
        /// This is class creates the graphics for the game map. It draws its changes onto a bitmap that is the picture that the user sees as the game.
        /// </summary>
        /// <param name="sender">This parameter is the object that created an instance of this class. It is needed so that we can send it the updated bitmap to display it.</param>
        /// <param name="NumberOfColumns">This parameter is the number of columns that the object that the sender has. This will be used to help us calculate the size of the map as well as for iterating through loops.</param>
        /// <param name="NumberOfRows">This parameter is the number of rows that the object that the sender has. This will be used to help us calculate the size of the map as well as for iterating through loops.</param>
        /// <param name="MineSizeInPixels">This parameter is the mine size that the object that the sender has. This will be used to help us calculate the size of the map as well.</param>
        public DrawMap(GameMap sender, int NumberOfColumns, int NumberOfRows, int MineSizeInPixels)
        {
            caller = sender;
            numberOfColumns = NumberOfColumns;
            numberOfRows = NumberOfRows;
            mineSizeInPixels = MineSizeInPixels;
            InitializeVariables();
        }
        /// <summary>
        /// This Method is used to get the map height in pixels
        /// </summary>
        /// <returns>This method returns the height of the game map in pixels</returns>
        public int getMapHeight()
        {
            return mapHeightInPixels;
        }
        /// <summary>
        /// This Method is used to get the map width in pixels
        /// </summary>
        /// <returns>This method returns the width of the game map in pixels</returns>
        public int getMapWidth()
        {
            return mapWidthInPixels;
        }
        /// <summary>
        /// This method initializes the variable used in this class. It also calculates the map width and the map height.
        /// </summary>
        private void InitializeVariables()
        {
            mapWidthInPixels = (numberOfColumns * mineSizeInPixels) + 1;            //Width is calculated by the combined width of the mine spaces plus one for the ending line to create the ending mine
            mapHeightInPixels = (numberOfRows * mineSizeInPixels) + 1;               //Height is calculated by the combined height of the mine spaces plus one for the ending line to create the ending mine

            updateScreenBitmap = new Bitmap(mapWidthInPixels, mapHeightInPixels);
            updateScreenGraphics = Graphics.FromImage(updateScreenBitmap);      //This cause updateScreenGraphics to work with our bitmap

            font = new Font(FontFamily.GenericSansSerif, mineSizeInPixels/5, FontStyle.Regular);// changed from mineSizeInPixels/2 to mineSizeInPixels/5 to show Brandon's Algorithm
        }
        /// <summary>
        /// This method draws a map of all initial mine spaces. It has the diminsions that are sent in to the constructor.
        /// </summary>
        public void InitialDraw()
        {
            updateScreenGraphics.FillRectangle(Brushes.Gray, 0, 0, mapWidthInPixels, mapHeightInPixels);        //Color the whole bitmap the color of the mine spaces           
            for (int rowIndex = 0; rowIndex <= numberOfRows; rowIndex++)        // Iterate from 0 to the number of rows plus one. We will do plus one because we need to draw one more line than we have mine spaces.
            {
                updateScreenGraphics.DrawLine(Pens.Black, 0, rowIndex * mineSizeInPixels, mapWidthInPixels, rowIndex * mineSizeInPixels);       //Draw horizontal lines that are rowIndex mines down. This will cause all the horizontal lines of the mine spaces to be drawn.

            }
            for (int columnIndex = 0; columnIndex <= numberOfColumns; columnIndex++)
            {
                updateScreenGraphics.DrawLine(Pens.Black, columnIndex * mineSizeInPixels, 0, columnIndex * mineSizeInPixels, mapHeightInPixels);        //Draw vertical lines that are columnIndex mines right of the left of the screen. This will cause all the vertical lines of the mine spaces to be drawn.
            }
            RefreshScreen();
        }
        /// <summary>
        /// This method draws a rectangle at the position defined by the position of the columnIndex and rowIndex parameters. It draws the rectangle with the brush that comes in as a parameter.
        /// </summary>
        /// <param name="columnIndex">This parameter defines the column location of the rectangle.</param>
        /// <param name="rowIndex">This parameter defines the row location of the rectangle.</param>
        /// <param name="brush">This parameter defines the brush to be used to draw the rectangle.</param>
        public void FillRectangle(int columnIndex, int rowIndex, Brush brush)
        {
            updateScreenGraphics.FillRectangle(brush, columnIndex * mineSizeInPixels + 1, rowIndex * mineSizeInPixels + 1, mineSizeInPixels - 1, mineSizeInPixels - 1);
        }
        /// <summary>
        /// This method draws a bitmap at the position defined by the position of the columnIndex and rowIndex parameters.
        /// </summary>
        /// <param name="columnIndex">This parameter defines the column location to draw the bitmap.</param>
        /// <param name="rowIndex">This parameter defines the row location to draw the bitmap.</param>
        /// <param name="bitmap">This parameter defines which bitmap to draw.</param>
        public void DrawBitmap(int columnIndex, int rowIndex, Bitmap bitmap)
        {
            updateScreenGraphics.DrawImage(bitmap, columnIndex * mineSizeInPixels + 1, rowIndex * mineSizeInPixels + 1, mineSizeInPixels - 1, mineSizeInPixels - 1);
        }
        /// <summary>
        /// This method outputs to the screen the updated bitmap inside the bitmap container which is a picture box.
        /// </summary>
        public void RefreshScreen()
        {
            caller.bitmapContainer.Image = updateScreenBitmap; // physically update the screen with the bitmap
        }
        /// <summary>
        /// This method draws a string at the position defined by the position of the columnIndex and rowIndex parameters. It draws it in a predefined font with a black color.
        /// </summary>
        /// <param name="outputString"> This parameter defines the string to be displayed to the screen</param>
        /// <param name="column">This parameter defines the column location to draw the string.</param>
        /// <param name="row">This parameter defines the row location to draw the string.</param>
        public void DrawString(string outputString, int column, int row)
        {
            updateScreenGraphics.DrawString(outputString, font, Brushes.Black, (float)(column+0.20) * mineSizeInPixels, (float)(row+0.15) * mineSizeInPixels);
        }
    }
}
