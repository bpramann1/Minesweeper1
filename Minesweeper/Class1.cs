﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace Minesweeper
{
    class GameMap
    {
        private int numberOfRows;                       //This variable indicates the number of rows. It has a default constructed value of 16 although it can be customized by user input
        private int numberOfColumns;                    //This variable indicates the number of Columns. It has a default constructed value of 16 although it can be customized by user input
        private int mineSizeInPixels;                   //This variable indicates the size of a possible mine space. It has a default constructed value of 20 although it can be customized by user input
        private int gameMapWidthInPixels;
        private int gameMapHeightInPixels;
        private int columnPositionOfMouse;
        private int rowPositionOfMouse;
        private int oldColumnPositionOfMouse;
        private int oldRowPositionOfMouse;
        private int numberOfCallsOnStack;

        enum MineSpaceStates
        {
            Initial,
        MappedAsSafe,
        FlaggedAsUnsafe,
        Pressed
        }

        private MineSpaceStates[,] stateOfMineSpace;
        private bool[,] containsMine;

        private Bitmap updateScreenBitmap;           //This variable is the bitmap we will update and then display to the screen when it is fully updated
        private Graphics updateScreenGraphics;       //This variable enables us to draw to the bitmap
        PictureBox bitmapContainer;                  //This variable is the object we will display the object in.
        Form Game;                                   //This is the object that will contain the game and game map


        public GameMap()                //Default Constuctor
        {
            numberOfRows = 16;          //Default Value
            numberOfColumns = 16;       //Default Value
            mineSizeInPixels = 20;      //Default Value         
            createMap();            //All the variables are set up, so draw the map.
        }

        public GameMap(int rows, int columns)                //Constructor for custom number of rows and columns
        {
            numberOfRows = rows;          //custom number of rows
            numberOfColumns = columns;       //custom number of columns
            mineSizeInPixels = 20;      //Default Value


            createMap();            //All the variables are set up, so draw the map.
        }

        public GameMap(int rows, int columns, int mineSize)                //Constructor for custom number of rows and columns and custom mine size
        {
            numberOfRows = rows;          //custom number of rows
            numberOfColumns = columns;       //custom number of columns
            mineSizeInPixels = mineSize;      //custom mine size


            createMap();            //All the variables are set up, so draw the map.
        }

        public GameMap(int mineSize)                //Constructor for custom mine size
        {
            numberOfRows = 16;          //Default number of columns
            numberOfColumns = 16;       //Default number of columns
            mineSizeInPixels = mineSize;      //custom mine size


            createMap();            //All the variables are set up, so draw the map.
        }

        private void createMap()
        {
            numberOfCallsOnStack = new StackTrace().FrameCount;
            stateOfMineSpace = new MineSpaceStates[numberOfColumns, numberOfRows];      //Set the array size. All the elements are automatically intiated to zero, which is the value of initial state in our enum.
            containsMine = new bool[numberOfColumns,numberOfRows];
            CreateBombMap();
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
            bitmapContainer.MouseMove += new MouseEventHandler(MouseMoveInGame);        //Adds the event handler so that the method MouseMoveInGame is called when the mouse moves in the bitmapContainer
            bitmapContainer.MouseDown += new MouseEventHandler(MineSpaceClicked);
            updateScreenBitmap = new Bitmap(gameMapWidthInPixels, gameMapHeightInPixels);
            updateScreenGraphics = Graphics.FromImage(updateScreenBitmap);      //This cause updateScreenGraphics to work with our bitmap

            updateScreenGraphics.FillRectangle(Brushes.Gray, 0, 0, gameMapWidthInPixels, gameMapHeightInPixels);        //Color the whole bitmap the color of the mine spaces
            
            

            
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
       

        private void MouseMoveInGame(object sender, System.EventArgs e)
        {
            
            int mousePositionX = Cursor.Position.X - Game.Location.X - bitmapContainer.Location.X - 8;
            /*Calculates the distance the mouse is from the upper left corner of the bitmap container.
            It is calculated by taking the current position of the curser and subtracting the location of the form
            and the thickness of the border of the form and the location of the bitmapContainer in the form.
            */
            int mousePositionY = Cursor.Position.Y - Game.Location.Y - bitmapContainer.Location.Y - 30;
            /*Calculates the distance the mouse is from the upper left corner of the bitmap container.
            It is calculated by taking the current position of the curser and subtracting the location of the form
            and the thickness of the border of the form and the location of the bitmapContainer in the form.
            */
            columnPositionOfMouse = (mousePositionX / mineSizeInPixels); //Calculates the mine position of the mouse
            rowPositionOfMouse = (mousePositionY / mineSizeInPixels);    //Calculates the mine position of the mouse    
            if ((columnPositionOfMouse != oldColumnPositionOfMouse) || (rowPositionOfMouse != oldRowPositionOfMouse)) // Check to see if the mouse changed mine spaces
            {
                if (ColumnRowInGameArray(columnPositionOfMouse, rowPositionOfMouse))
                {
                    if (stateOfMineSpace[columnPositionOfMouse, rowPositionOfMouse] == MineSpaceStates.Initial) //Check to see if the mine should be highlighted
                    {
                        //Higlight the current rectangle
                        updateScreenGraphics.FillRectangle(Brushes.LightGray, columnPositionOfMouse * mineSizeInPixels, rowPositionOfMouse * mineSizeInPixels, mineSizeInPixels, mineSizeInPixels); //fill the rectangle with the highlight color
                        updateScreenGraphics.DrawRectangle(Pens.Black, columnPositionOfMouse * mineSizeInPixels, rowPositionOfMouse * mineSizeInPixels, mineSizeInPixels, mineSizeInPixels); // surround the rectangle with a black border
                    }
                }
                if (ColumnRowInGameArray(oldColumnPositionOfMouse, oldRowPositionOfMouse))
                {
                    if (stateOfMineSpace[oldColumnPositionOfMouse, oldRowPositionOfMouse] == MineSpaceStates.Initial) //Check to see if the mine should draw to initial color
                    {
                        //Redraw over the last hightlighted rectangle
                        updateScreenGraphics.FillRectangle(Brushes.Gray, oldColumnPositionOfMouse * mineSizeInPixels, oldRowPositionOfMouse * mineSizeInPixels, mineSizeInPixels, mineSizeInPixels);//fill the rectangle with the normal mine color
                        updateScreenGraphics.DrawRectangle(Pens.Black, oldColumnPositionOfMouse * mineSizeInPixels, oldRowPositionOfMouse * mineSizeInPixels, mineSizeInPixels, mineSizeInPixels);// surround the rectangle with a black border
                    }
                }
                bitmapContainer.Image = updateScreenBitmap; // physically update the screen with the bitmap
            }
            //Done Drawing the Rectangle
            oldColumnPositionOfMouse = columnPositionOfMouse;// Set the value to hold the mouse position so that we can check to see if it had changed
            oldRowPositionOfMouse = rowPositionOfMouse;// Set the value to hold the mouse position so that we can check to see if it had changed
        }


        private void MineSpaceClicked(object sender, MouseEventArgs mouse)
        {
            if (mouse.Button == MouseButtons.Left)
                {
                LeftMouseButtonClicked(columnPositionOfMouse, rowPositionOfMouse);
            }
            else if(mouse.Button == MouseButtons.Right)
            {
                RightMouseButtonClicked();
            }
        }

        private void LeftMouseButtonClicked(int column, int row)
        {
            if (ColumnRowInGameArray(column,row))
            {
                if (stateOfMineSpace[column, row] == MineSpaceStates.Initial)
                {
                    RevealMineSpace(column, row);
                }
            }
        }

        private void RevealMineSpace(int column, int row)
        {
            stateOfMineSpace[column, row] = MineSpaceStates.Pressed;
            int numberOfAdjacentBombs = numberOfTouchingBombs(column, row);
            if (ColumnRowInGameArray(column, row))
            {
                if (containsMine[column, row])
                {
                    //Higlight the current rectangle
                    updateScreenGraphics.FillRectangle(Brushes.Black, column * mineSizeInPixels, row * mineSizeInPixels, mineSizeInPixels, mineSizeInPixels); //fill the rectangle with the black color
                    updateScreenGraphics.DrawRectangle(Pens.Black, column * mineSizeInPixels, row * mineSizeInPixels, mineSizeInPixels, mineSizeInPixels); // surround the rectangle with a black border
                    EndReveal();
                }
                else
                {
                    //Higlight the current rectangle
                    updateScreenGraphics.FillRectangle(Brushes.White, column * mineSizeInPixels, row * mineSizeInPixels, mineSizeInPixels, mineSizeInPixels); //fill the rectangle with the black color
                    updateScreenGraphics.DrawRectangle(Pens.Black, column * mineSizeInPixels, row * mineSizeInPixels, mineSizeInPixels, mineSizeInPixels); // surround the rectangle with a black border
                    if (numberOfAdjacentBombs == 0)
                    {
                        ClickAdjacentSpaces(column, row);
                    }
                    else
                    {
                        updateScreenGraphics.DrawString(numberOfAdjacentBombs.ToString(), SystemFonts.DefaultFont, Brushes.Black, column * mineSizeInPixels, row * mineSizeInPixels);
                    }
                }
   //             if(numberOfCallsOnStack < 20)
   //             {
                    bitmapContainer.Image = updateScreenBitmap;
 //               }

            }
        }

        private void RightMouseButtonClicked()
        {
            if (ColumnRowInGameArray(columnPositionOfMouse, rowPositionOfMouse))
            {
                switch (stateOfMineSpace[columnPositionOfMouse, rowPositionOfMouse])
                {
                    case MineSpaceStates.Initial:
                        //Higlight the current rectangle
                        updateScreenGraphics.FillRectangle(Brushes.Red, columnPositionOfMouse * mineSizeInPixels, rowPositionOfMouse * mineSizeInPixels, mineSizeInPixels, mineSizeInPixels); //fill the rectangle with the black color
                        updateScreenGraphics.DrawRectangle(Pens.Black, columnPositionOfMouse * mineSizeInPixels, rowPositionOfMouse * mineSizeInPixels, mineSizeInPixels, mineSizeInPixels); // surround the rectangle with a black border
                        stateOfMineSpace[columnPositionOfMouse, rowPositionOfMouse] = MineSpaceStates.FlaggedAsUnsafe;
                        break;
                    case MineSpaceStates.FlaggedAsUnsafe:
                        //Higlight the current rectangle
                        updateScreenGraphics.FillRectangle(Brushes.Green, columnPositionOfMouse * mineSizeInPixels, rowPositionOfMouse * mineSizeInPixels, mineSizeInPixels, mineSizeInPixels); //fill the rectangle with the black color
                        updateScreenGraphics.DrawRectangle(Pens.Black, columnPositionOfMouse * mineSizeInPixels, rowPositionOfMouse * mineSizeInPixels, mineSizeInPixels, mineSizeInPixels); // surround the rectangle with a black border
                        stateOfMineSpace[columnPositionOfMouse, rowPositionOfMouse] = MineSpaceStates.MappedAsSafe;
                        break;
                    case MineSpaceStates.MappedAsSafe:
                        //Higlight the current rectangle
                        updateScreenGraphics.FillRectangle(Brushes.Gray, columnPositionOfMouse * mineSizeInPixels, rowPositionOfMouse * mineSizeInPixels, mineSizeInPixels, mineSizeInPixels); //fill the rectangle with the black color
                        updateScreenGraphics.DrawRectangle(Pens.Black, columnPositionOfMouse * mineSizeInPixels, rowPositionOfMouse * mineSizeInPixels, mineSizeInPixels, mineSizeInPixels); // surround the rectangle with a black border
                        stateOfMineSpace[columnPositionOfMouse, rowPositionOfMouse] = MineSpaceStates.Initial;
                        break;
                    default:
                        break;
                }
                bitmapContainer.Image = updateScreenBitmap;
            }
        }


        private bool ColumnRowInGameArray(int column, int row)//Bounds Checking
        {
            if ((column >= 0) && (column < numberOfColumns) && (row >= 0) && (row < numberOfRows))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private int numberOfTouchingBombs(int column, int row)
        {
            int numberOfBombs = 0;
            //Top row
            if (ColumnRowInGameArray(column - 1, row - 1))
            {
                if(containsMine[column - 1, row - 1])
                {
                    numberOfBombs++;
                }
            }
            if (ColumnRowInGameArray(column, row - 1))
            {
                if (containsMine[column, row - 1])
                {
                    numberOfBombs++;
                }
            }
            if (ColumnRowInGameArray(column + 1, row - 1))
            {
                if (containsMine[column + 1, row - 1])
                {
                    numberOfBombs++;
                }
            }
            //Middle row
            if (ColumnRowInGameArray(column - 1, row))
            {
                if (containsMine[column - 1, row])
                {
                    numberOfBombs++;
                }
            }
            if (ColumnRowInGameArray(column + 1, row))
            {
                if (containsMine[column + 1, row])
                {
                    numberOfBombs++;
                }
            }
            //Bottom row
            if (ColumnRowInGameArray(column - 1, row + 1))
            {
                if (containsMine[column - 1, row + 1])
                {
                    numberOfBombs++;
                }
            }
            if (ColumnRowInGameArray(column, row + 1))
            {
                if (containsMine[column, row + 1])
                {
                    numberOfBombs++;
                }
            }
            if (ColumnRowInGameArray(column + 1, row + 1))
            {
                if (containsMine[column + 1, row + 1])
                {
                    numberOfBombs++;
                }
            }
            return numberOfBombs;
        }

        private void ClickAdjacentSpaces(int column, int row)
        {
            //Top row
            if (ColumnRowInGameArray(column - 1, row - 1))
            {
                if (stateOfMineSpace[column - 1, row - 1] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column - 1, row - 1);
                }
            }
            if (ColumnRowInGameArray(column, row - 1))
            {
                if (stateOfMineSpace[column, row - 1] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column, row - 1);
                }
            }
            if (ColumnRowInGameArray(column + 1, row - 1))
            {
                if (stateOfMineSpace[column + 1, row - 1] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column + 1, row - 1);
                }
            }
            //Middle row
            if (ColumnRowInGameArray(column - 1, row))
            {
                if (stateOfMineSpace[column - 1, row] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column - 1, row);
                }
            }
            if (ColumnRowInGameArray(column + 1, row))
            {
                if (stateOfMineSpace[column + 1, row] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column + 1, row);
                }
            }
            //Bottom row
            if (ColumnRowInGameArray(column - 1, row + 1))
            {
                if (stateOfMineSpace[column - 1, row + 1] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column - 1, row + 1);
                }
            }
            if (ColumnRowInGameArray(column, row + 1))
            {
                if (stateOfMineSpace[column, row + 1] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column, row + 1);
                }
            }
            if (ColumnRowInGameArray(column + 1, row + 1))
            {
                if (stateOfMineSpace[column + 1, row + 1] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column + 1, row + 1);
                }
            }
        }


        private void CreateBombMap()
        {
            Random rnd = new Random();
            for (int columnIndex = 0; columnIndex < numberOfColumns; columnIndex++)
            {
                for (int rowIndex = 0; rowIndex < numberOfRows; rowIndex++)
                {
                    if (rnd.Next(8) > 0)
                    {
                        containsMine[columnIndex,rowIndex] = false;
                    }
                    else
                    {
                        containsMine[columnIndex,rowIndex] = true;
                    }
                }
            }
        }


        private void EndReveal()
        {
            for (int rowIndex = 0; rowIndex < numberOfRows; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < numberOfColumns; columnIndex++)
                {
                    if (stateOfMineSpace[columnIndex,rowIndex] != MineSpaceStates.Pressed)
                    {
                        stateOfMineSpace[columnIndex, rowIndex] = MineSpaceStates.Pressed;
                        int numberOfAdjacentBombs = numberOfTouchingBombs(columnIndex, rowIndex);
                        if (ColumnRowInGameArray(columnIndex, rowIndex))
                        {
                            if (containsMine[columnIndex, rowIndex])
                            {
                                //Higlight the current rectangle
                                updateScreenGraphics.FillRectangle(Brushes.Black, columnIndex * mineSizeInPixels, rowIndex * mineSizeInPixels, mineSizeInPixels, mineSizeInPixels); //fill the rectangle with the black color
                                updateScreenGraphics.DrawRectangle(Pens.Black, columnIndex * mineSizeInPixels, rowIndex * mineSizeInPixels, mineSizeInPixels, mineSizeInPixels); // surround the rectangle with a black border
                            }
                            else
                            {
                                //Higlight the current rectangle
                                updateScreenGraphics.FillRectangle(Brushes.White, columnIndex * mineSizeInPixels, rowIndex * mineSizeInPixels, mineSizeInPixels, mineSizeInPixels); //fill the rectangle with the black color
                                updateScreenGraphics.DrawRectangle(Pens.Black, columnIndex * mineSizeInPixels, rowIndex * mineSizeInPixels, mineSizeInPixels, mineSizeInPixels); // surround the rectangle with a black border
                                if (numberOfAdjacentBombs != 0)
                                {
                                    updateScreenGraphics.DrawString(numberOfAdjacentBombs.ToString(), SystemFonts.DefaultFont, Brushes.Black, columnIndex * mineSizeInPixels, rowIndex * mineSizeInPixels);
                                }
                            }

                        }

                    }
                }

            }
            bitmapContainer.Image = updateScreenBitmap;
        }


    }




}
