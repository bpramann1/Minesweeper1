using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
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

        public DrawMap(GameMap sender, int NumberOfColumns, int NumberOfRows, int MineSizeInPixels)
        {
            caller = sender;
            numberOfColumns = NumberOfColumns;
            numberOfRows = NumberOfRows;
            mineSizeInPixels = MineSizeInPixels;
            InitializeVariables();
        }

        public int getMapHeight()
        {
            return mapHeightInPixels;
        }

        public int getMapWidth()
        {
            return mapWidthInPixels;
        }

        private void InitializeVariables()
        {
            mapWidthInPixels = (numberOfColumns * mineSizeInPixels) + 1;            //Width is calculated by the combined width of the mine spaces plus one for the ending line to create the ending mine
            mapHeightInPixels = (numberOfRows * mineSizeInPixels) + 1;               //Height is calculated by the combined height of the mine spaces plus one for the ending line to create the ending mine

            updateScreenBitmap = new Bitmap(mapWidthInPixels, mapHeightInPixels);
            updateScreenGraphics = Graphics.FromImage(updateScreenBitmap);      //This cause updateScreenGraphics to work with our bitmap

            font = new Font(FontFamily.GenericSansSerif, mineSizeInPixels/2, FontStyle.Regular);
        }

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
        public void FillRectangle(int columnIndex, int rowIndex, Brush brush)
        {
            updateScreenGraphics.FillRectangle(brush, columnIndex * mineSizeInPixels + 1, rowIndex * mineSizeInPixels + 1, mineSizeInPixels - 1, mineSizeInPixels - 1);
        }
        public void DrawBitmap(int columnIndex, int rowIndex, Bitmap bitmap)
        {
            updateScreenGraphics.DrawImage(bitmap, columnIndex * mineSizeInPixels + 1, rowIndex * mineSizeInPixels + 1, mineSizeInPixels - 1, mineSizeInPixels - 1);
        }
        public void RefreshScreen()
        {
            caller.bitmapContainer.Image = updateScreenBitmap; // physically update the screen with the bitmap
        }
        public void DrawString(string outputString, int column, int row)
        {
            updateScreenGraphics.DrawString(outputString, font, Brushes.Black, (float)(column+0.20) * mineSizeInPixels, (float)(row+0.15) * mineSizeInPixels);
        }
    }
}
