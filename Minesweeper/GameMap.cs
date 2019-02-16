using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;

namespace Minesweeper
{
    public class GameMap
    {
        #region Declaration of variables
        public int numberOfRows;                       //This variable indicates the number of rows. It has a default constructed value of 16 although it can be customized by user input
        public int numberOfColumns;                    //This variable indicates the number of Columns. It has a default constructed value of 16 although it can be customized by user input
        public int mineSizeInPixels;                   //This variable indicates the size of a possible mine space. It has a default constructed value of 20 although it can be customized by user input
        public int numberOfBombs;
        private int gameMapWidthInPixels;
        private int gameMapHeightInPixels;
        private int columnPositionOfMouse;
        private int rowPositionOfMouse;
        private int oldColumnPositionOfMouse;
        private int oldRowPositionOfMouse;
        private int automaticClickNumber;
        public int totalNumberOfBombsLeft;
        public int totalNumberOfSafeSpacesLeft;
        public bool fromLoad = false;
        public bool exitImmediately = false;
        public DrawMap mapEditor;

        public PictureBox bitmapContainer;                  //This variable is the object we will display the object in.

        public enum MineSpaceStates
        {
            Initial,
            MappedAsSafe,
            FlaggedAsUnsafe,
            Pressed
        }

        public MineSpaceStates[,] stateOfMineSpace;
        public bool[,] containsMine;

       public Form Game;                                   //This is the object that will contain the game and game map
        public Label NumberOfBombsLeftLabel;
        public Label NumberOfSafeSpacesLeftLabel;
        public MenuStrip myMenuStrip;
        public ToolStripMenuItem File;
        public ToolStripMenuItem NewGame;
        public ToolStripMenuItem SaveGame;
        public ToolStripMenuItem LoadGame;
        public ToolStripMenuItem DeleteGame;
        #endregion

        #region Constructors
        public GameMap()                //Default Constuctor
        {
            numberOfRows = 16;          //Default Value
            numberOfColumns = 16;       //Default Value
            mineSizeInPixels = 20;      //Default Value  
            numberOfBombs = 16;
            createMap();            //All the variables are set up, so draw the map.
        }

        public GameMap(int rows, int columns, int bombs)                //Constructor for custom number of rows and columns
        {
            numberOfRows = rows;          //custom number of rows
            numberOfColumns = columns;       //custom number of columns
            mineSizeInPixels = 20;      //Default Value
            numberOfBombs = bombs;

            createMap();            //All the variables are set up, so draw the map.
        }

        public GameMap(int rows, int columns, int mineSize, int bombs)                //Constructor for custom number of rows and columns and custom mine size
        {
            numberOfRows = rows;          //custom number of rows
            numberOfColumns = columns;       //custom number of columns
            mineSizeInPixels = mineSize;      //custom mine size
            numberOfBombs = bombs;

            createMap();            //All the variables are set up, so draw the map.
        }

        public GameMap(int mineSize, int bombs)                //Constructor for custom mine size
        {
            numberOfRows = 16;          //Default number of columns
            numberOfColumns = 16;       //Default number of columns
            mineSizeInPixels = mineSize;      //custom mine size
            numberOfBombs = bombs;
            createMap();            //All the variables are set up, so draw the map.
        }

        public GameMap(bool isLoadGame, int NumberOfColumns, int NumberOfRows, bool[,] ContainsMine,MineSpaceStates[,] StateOfMineSpace)                //Default Constuctor
        {
            fromLoad = isLoadGame;
            numberOfColumns = NumberOfColumns;
            numberOfRows = NumberOfRows;
            containsMine = ContainsMine;
            stateOfMineSpace = StateOfMineSpace;
            mineSizeInPixels = 50;
            totalNumberOfBombsLeft = 0;
            totalNumberOfSafeSpacesLeft = numberOfColumns*numberOfRows;
            createMap();
            for (int y = 0; y < numberOfRows; y++)
            {
                for (int x = 0; x < numberOfColumns; x++)
                {
                    if (containsMine[x, y])
                    {
                        totalNumberOfBombsLeft++;
                        totalNumberOfSafeSpacesLeft--;
                    }
                    if (stateOfMineSpace[x,y] == MineSpaceStates.FlaggedAsUnsafe)
                    {
                        totalNumberOfBombsLeft--;
                        mapEditor.DrawBitmap(x, y, Properties.Resources.redflag);
                    }
                    if (stateOfMineSpace[x, y] == MineSpaceStates.MappedAsSafe)
                    {
                        totalNumberOfSafeSpacesLeft--;
                        mapEditor.DrawBitmap(x, y, Properties.Resources.greenflag);
                    }
                    if (stateOfMineSpace[x, y] == MineSpaceStates.Pressed)
                    {
                        totalNumberOfSafeSpacesLeft--;
                        mapEditor.FillRectangle(x, y, Brushes.White);
                        int num = numberOfTouchingBombs(x, y);
                        if (num != 0)
                        {
                            mapEditor.DrawString(numberOfTouchingBombs(x, y).ToString(), x, y);
                        }                           
                    }
                }
            }
            initializeMapVarsAfterBombsCreated();
        }

        #endregion




        private void createMap()
        {
            initializeMapVarsBeforeBombsCreated();
            if (!fromLoad)
            {
                CreateBombMap();
            }
            initializeMapVarsAfterBombsCreated();
            drawMap();
            Game.TopMost = true;                                //Make the game form be the topmost form.
        }     

        private void initializeMapVarsBeforeBombsCreated()
        {
            if (!fromLoad)
            {
                stateOfMineSpace = new MineSpaceStates[numberOfColumns, numberOfRows];      //Set the array size. All the elements are automatically intiated to zero, which is the value of initial state in our enum.
                containsMine = new bool[numberOfColumns, numberOfRows];
            }
            mapEditor = new DrawMap(this);
            gameMapHeightInPixels = mapEditor.mapHeightInPixels;
            gameMapWidthInPixels = mapEditor.mapWidthInPixels;
            Game = new Form();
            Game.Show();
            Game.Location = new Point(0, 0);
            Game.Width = Math.Min(gameMapWidthInPixels + 16, Screen.PrimaryScreen.WorkingArea.Width);                     //16 is the added width of the edges of the form, so width is the width of the game map plus the width of the edges of the form               
            Game.Height = Math.Min(gameMapHeightInPixels + 82, Screen.PrimaryScreen.WorkingArea.Height);                   //38 is the added height of the edges of the form, so height is the height of the game map plus the height of the edges of the form
            Game.Text = "Minesweeper";                                  //Names the title of the form to "Minesweeper"
            Game.FormBorderStyle = FormBorderStyle.Fixed3D;
            if ((gameMapHeightInPixels > Game.Height) || (gameMapWidthInPixels > Game.Width))
            {
                Game.AutoScroll = true;
            }
            Game.FormClosing += new FormClosingEventHandler(this.ExitApplication);
            myMenuStrip = new MenuStrip();
            myMenuStrip.BackColor = SystemColors.Menu;
            Game.MainMenuStrip = myMenuStrip;
            Game.Controls.Add(myMenuStrip);
            File = new ToolStripMenuItem("File");
            myMenuStrip.Items.Add(File);
            NewGame = new ToolStripMenuItem("New Game");
            SaveGame = new ToolStripMenuItem("Save Game");
            LoadGame = new ToolStripMenuItem("Load Game");
            DeleteGame = new ToolStripMenuItem("Delete Game");
            ToolStripItem[] myMenuItem = { NewGame, SaveGame, LoadGame,DeleteGame };
            File.DropDownItems.AddRange(myMenuItem);
            NewGame.Click += new EventHandler(MenuNewGame);
            SaveGame.Click += new EventHandler(MenuSaveGame);
            LoadGame.Click += new EventHandler(MenuLoadGame);
            DeleteGame.Click += new EventHandler(MenuDeleteGame);
            NumberOfBombsLeftLabel = new Label();
            NumberOfBombsLeftLabel.Width = gameMapWidthInPixels / 2;

            Game.Controls.Add(NumberOfBombsLeftLabel);
            NumberOfBombsLeftLabel.Location = new Point(0, myMenuStrip.Height);
            NumberOfSafeSpacesLeftLabel = new Label();
            Game.Controls.Add(NumberOfSafeSpacesLeftLabel);
            NumberOfSafeSpacesLeftLabel.Location = new Point(gameMapWidthInPixels / 2, myMenuStrip.Height);
            NumberOfSafeSpacesLeftLabel.Width = gameMapWidthInPixels / 2;
            NumberOfSafeSpacesLeftLabel.Height = NumberOfBombsLeftLabel.Height;
            bitmapContainer = new PictureBox();
            bitmapContainer.Width = gameMapWidthInPixels;               //bitmap is just big enough to hold the game map
            bitmapContainer.Height = gameMapHeightInPixels;             //bitmap is just big enough to hold the game map
            Game.Controls.Add(bitmapContainer);                         //Add the PictureBox object called bitmapContainer to the form called Game. This makes the bitmapContainer be displayed in game.
            bitmapContainer.Location = new Point(0, NumberOfBombsLeftLabel.Height + myMenuStrip.Height);
            bitmapContainer.MouseMove += new MouseEventHandler(MouseMoveInGame);        //Adds the event handler so that the method MouseMoveInGame is called when the mouse moves in the bitmapContainer
            bitmapContainer.MouseDown += new MouseEventHandler(MineSpaceClicked);
        }

        private void initializeMapVarsAfterBombsCreated()
        {
            NumberOfBombsLeftLabel.Text = totalNumberOfBombsLeft.ToString() + " bombs left";
            NumberOfSafeSpacesLeftLabel.Text = totalNumberOfSafeSpacesLeft.ToString() + " safe spaces left";
        }

        private void drawMap()
        {
            mapEditor.InitialDraw();
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
                        mapEditor.FillRectangle(columnPositionOfMouse, rowPositionOfMouse, Brushes.LightGray);
                    }
                }
                if (ColumnRowInGameArray(oldColumnPositionOfMouse, oldRowPositionOfMouse))
                {
                    if (stateOfMineSpace[oldColumnPositionOfMouse, oldRowPositionOfMouse] == MineSpaceStates.Initial) //Check to see if the mine should draw to initial color
                    {
                        //Redraw over the last hightlighted rectangle
                        mapEditor.FillRectangle(oldColumnPositionOfMouse, oldRowPositionOfMouse, Brushes.Gray);
                    }
                }
                mapEditor.RefreshScreen();
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

        private void RevealMineSpace(int column, int row)//Only Call when you know that the column and row are in game
        {
            if (stateOfMineSpace[column, row] == MineSpaceStates.FlaggedAsUnsafe)
            {
                totalNumberOfBombsLeft++;
                NumberOfBombsLeftLabel.Text = totalNumberOfBombsLeft.ToString() + " bombs left";
            }
            if (stateOfMineSpace[column,row]!= MineSpaceStates.MappedAsSafe)
            {
                totalNumberOfSafeSpacesLeft--;
                if(totalNumberOfSafeSpacesLeft == 0)
                {
                    Ending end = new Ending(this);
                }
                NumberOfSafeSpacesLeftLabel.Text = totalNumberOfSafeSpacesLeft.ToString() + " safe spaces left";
            }
            stateOfMineSpace[column, row] = MineSpaceStates.Pressed;
            if (containsMine[column, row])
            {

                //Higlight the current rectangle
                mapEditor.DrawBitmap(column, row, Properties.Resources.mine1);
                Ending end = new Ending(this,true);
                //this.EndReveal();
                
            }


            else
            {
                int numberOfAdjacentBombs = numberOfTouchingBombs(column, row);
                //Higlight the current rectangle
                mapEditor.FillRectangle(column, row, Brushes.White);
                if (numberOfAdjacentBombs == 0)
                {
                    automaticClickNumber++;
                    switch ((automaticClickNumber%40)/10)
                    {
                        case 0:
                            ClickAdjacentSpacesUpFirst(column, row);
                            break;
                        case 1:
                            ClickAdjacentSpacesLeftFirst(column, row);
                            break;
                        case 2:
                            ClickAdjacentSpacesDownFirst(column, row);
                            break;
                        case 3:
                            ClickAdjacentSpacesRightFirst(column, row);
                            break;
                        default:
                            break;
                    }
                    //updateScreenGraphics.DrawString(automaticClickNumber.ToString(), SystemFonts.DefaultFont, Brushes.Black, column * mineSizeInPixels, row * mineSizeInPixels);
                }
                else
                {
                    mapEditor.DrawString(numberOfAdjacentBombs.ToString(), column, row);
                }
            }
            mapEditor.RefreshScreen();


        }

        private void RightMouseButtonClicked()
        {
            if (ColumnRowInGameArray(columnPositionOfMouse, rowPositionOfMouse))
            {
                switch (stateOfMineSpace[columnPositionOfMouse, rowPositionOfMouse])
                {
                    case MineSpaceStates.Initial:
                        //Higlight the current rectangle
                        mapEditor.DrawBitmap(columnPositionOfMouse, rowPositionOfMouse, Properties.Resources.redflag);
                        stateOfMineSpace[columnPositionOfMouse, rowPositionOfMouse] = MineSpaceStates.FlaggedAsUnsafe;
                        totalNumberOfBombsLeft--;
                        if(totalNumberOfBombsLeft == 0)
                        {
                            Ending end = new Ending(this);
                        }
                        break;
                    case MineSpaceStates.FlaggedAsUnsafe:
                        //Higlight the current rectangle
                        //mapEditor.DrawBitmap(columnPositionOfMouse, rowPositionOfMouse, Properties.Resources.greenflag);
                        mapEditor.FillRectangle(columnPositionOfMouse, rowPositionOfMouse, Brushes.Gray);
                        stateOfMineSpace[columnPositionOfMouse, rowPositionOfMouse] = MineSpaceStates.Initial;
                        totalNumberOfBombsLeft++;
                        if (totalNumberOfSafeSpacesLeft == 0)
                        {
                            Ending end = new Ending(this);
                        }

                        break;
                    case MineSpaceStates.MappedAsSafe:
                        //Higlight the current rectangle
                        mapEditor.FillRectangle(columnPositionOfMouse, rowPositionOfMouse, Brushes.Gray);
                        stateOfMineSpace[columnPositionOfMouse, rowPositionOfMouse] = MineSpaceStates.Initial;
                        totalNumberOfSafeSpacesLeft++;
                        break;
                    default:
                        break;
                }
                NumberOfSafeSpacesLeftLabel.Text = totalNumberOfSafeSpacesLeft.ToString() + " safe spaces left";
                NumberOfBombsLeftLabel.Text = totalNumberOfBombsLeft.ToString() + " bombs left";
                mapEditor.RefreshScreen();
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

        private void CreateBombMap()
        {

            if (!fromLoad)
            {
                totalNumberOfBombsLeft = numberOfBombs;
                int totalNumberOfSpaces = numberOfColumns * numberOfRows;
                totalNumberOfSafeSpacesLeft = totalNumberOfSpaces - numberOfBombs;
                for (int i = 0; i < numberOfColumns; i++)
                {
                    for (int j = 0; j < numberOfRows; j++)
                    {
                        containsMine[i, j] = false;
                    }
                }
                int bombStock = numberOfBombs;
                Random blg = new Random();
                while (bombStock > 0)
                {
                    int chosenCol = blg.Next(numberOfColumns);
                    int chosenRow = blg.Next(numberOfRows);
                    if (containsMine[chosenCol, chosenRow] == false)
                    {
                        containsMine[chosenCol, chosenRow] = true;
                        bombStock--;
                    }
                }
            }
        }

        public void EndReveal()
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
                                mapEditor.DrawBitmap(columnIndex, rowIndex, Properties.Resources.mine1);
                            }
                            else
                            {
                                //Higlight the current rectangle
                                mapEditor.FillRectangle(columnIndex, rowIndex, Brushes.White);
                                if (numberOfAdjacentBombs != 0)
                                {
                                    mapEditor.DrawString(numberOfAdjacentBombs.ToString(), columnIndex, rowIndex);
                                }
                            }

                        }

                    }
                }

            }
            mapEditor.RefreshScreen();
        }

        #region ClickAdjacentSquaresFunctions
        private void ClickAdjacentSpacesUpFirst(int column, int row)
        {
            //Up
            if ((column >= 0) && (column < numberOfColumns) && (row - 1 >= 0) && (row - 1 < numberOfRows))
            {
                if (stateOfMineSpace[column, row - 1] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column, row - 1);
                }
            }
            //automaticClickNumber = 0;
            //UpLeft
            if ((column - 1 >= 0) && (column - 1 < numberOfColumns) && (row - 1 >= 0) && (row - 1 < numberOfRows))
            {
                if (stateOfMineSpace[column - 1, row - 1] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column - 1, row - 1);
                }
            }

            //UpRight
            if ((column + 1 >= 0) && (column + 1 < numberOfColumns) && (row - 1 >= 0) && (row - 1 < numberOfRows))
            {
                if (stateOfMineSpace[column + 1, row - 1] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column + 1, row - 1);
                }
            }
            //Left
            if ((column - 1 >= 0) && (column - 1 < numberOfColumns) && (row >= 0) && (row < numberOfRows))
            {
                if (stateOfMineSpace[column - 1, row] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column - 1, row);
                }
            }//Right
            if ((column + 1 >= 0) && (column + 1 < numberOfColumns) && (row >= 0) && (row < numberOfRows))
            {
                if (stateOfMineSpace[column + 1, row] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column + 1, row);
                }
            }
            //DownLeft
            if ((column - 1 >= 0) && (column - 1 < numberOfColumns) && (row + 1 >= 0) && (row + 1 < numberOfRows))
            {
                if (stateOfMineSpace[column - 1, row + 1] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column - 1, row + 1);
                }
            }
            //Down
            if ((column >= 0) && (column < numberOfColumns) && (row + 1 >= 0) && (row + 1 < numberOfRows))
            {
                if (stateOfMineSpace[column, row + 1] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column, row + 1);
                }
            }
            //Down Right
            if ((column + 1 >= 0) && (column + 1 < numberOfColumns) && (row + 1 >= 0) && (row + 1 < numberOfRows))
            {
                if (stateOfMineSpace[column + 1, row + 1] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column + 1, row + 1);
                }
            }
        }
        private void ClickAdjacentSpacesLeftFirst(int column, int row)
        {
            //Left
            if ((column - 1 >= 0) && (column - 1 < numberOfColumns) && (row >= 0) && (row < numberOfRows))
            {
                if (stateOfMineSpace[column - 1, row] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column - 1, row);
                }
            }
            //automaticClickNumber = 0;
            //UpLeft
            if ((column - 1 >= 0) && (column - 1 < numberOfColumns) && (row - 1 >= 0) && (row - 1 < numberOfRows))
            {
                if (stateOfMineSpace[column - 1, row - 1] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column - 1, row - 1);
                }
            }
            //Up
            if ((column >= 0) && (column < numberOfColumns) && (row - 1 >= 0) && (row - 1 < numberOfRows))
            {
                if (stateOfMineSpace[column, row - 1] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column, row - 1);
                }
            }
            //UpRight
            if ((column + 1 >= 0) && (column + 1 < numberOfColumns) && (row - 1 >= 0) && (row - 1 < numberOfRows))
            {
                if (stateOfMineSpace[column + 1, row - 1] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column + 1, row - 1);
                }
            }
            //Right
            if ((column + 1 >= 0) && (column + 1 < numberOfColumns) && (row >= 0) && (row < numberOfRows))
            {
                if (stateOfMineSpace[column + 1, row] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column + 1, row);
                }
            }
            //DownLeft
            if ((column - 1 >= 0) && (column - 1 < numberOfColumns) && (row + 1 >= 0) && (row + 1 < numberOfRows))
            {
                if (stateOfMineSpace[column - 1, row + 1] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column - 1, row + 1);
                }
            }
            //Down
            if ((column >= 0) && (column < numberOfColumns) && (row + 1 >= 0) && (row + 1 < numberOfRows))
            {
                if (stateOfMineSpace[column, row + 1] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column, row + 1);
                }
            }
            //Down Right
            if ((column + 1 >= 0) && (column + 1 < numberOfColumns) && (row + 1 >= 0) && (row + 1 < numberOfRows))
            {
                if (stateOfMineSpace[column + 1, row + 1] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column + 1, row + 1);
                }
            }
        }
        private void ClickAdjacentSpacesDownFirst(int column, int row)
        {
            //Down
            if ((column >= 0) && (column < numberOfColumns) && (row + 1 >= 0) && (row + 1 < numberOfRows))
            {
                if (stateOfMineSpace[column, row + 1] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column, row + 1);
                }
            }
            //automaticClickNumber = 0;
            //UpLeft
            if ((column - 1 >= 0) && (column - 1 < numberOfColumns) && (row - 1 >= 0) && (row - 1 < numberOfRows))
            {
                if (stateOfMineSpace[column - 1, row - 1] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column - 1, row - 1);
                }
            }
            //Up
            if ((column >= 0) && (column < numberOfColumns) && (row - 1 >= 0) && (row - 1 < numberOfRows))
            {
                if (stateOfMineSpace[column, row - 1] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column, row - 1);
                }
            }
            //UpRight
            if ((column + 1 >= 0) && (column + 1 < numberOfColumns) && (row - 1 >= 0) && (row - 1 < numberOfRows))
            {
                if (stateOfMineSpace[column + 1, row - 1] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column + 1, row - 1);
                }
            }
            //Left
            if ((column - 1 >= 0) && (column - 1 < numberOfColumns) && (row >= 0) && (row < numberOfRows))
            {
                if (stateOfMineSpace[column - 1, row] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column - 1, row);
                }
            }//Right
            if ((column + 1 >= 0) && (column + 1 < numberOfColumns) && (row >= 0) && (row < numberOfRows))
            {
                if (stateOfMineSpace[column + 1, row] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column + 1, row);
                }
            }
            //DownLeft
            if ((column - 1 >= 0) && (column - 1 < numberOfColumns) && (row + 1 >= 0) && (row + 1 < numberOfRows))
            {
                if (stateOfMineSpace[column - 1, row + 1] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column - 1, row + 1);
                }
            }
            //Down Right
            if ((column + 1 >= 0) && (column + 1 < numberOfColumns) && (row + 1 >= 0) && (row + 1 < numberOfRows))
            {
                if (stateOfMineSpace[column + 1, row + 1] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column + 1, row + 1);
                }
            }
        }
        private void ClickAdjacentSpacesRightFirst(int column, int row)
        {
            //Right
            if ((column + 1 >= 0) && (column + 1 < numberOfColumns) && (row >= 0) && (row < numberOfRows))
            {
                if (stateOfMineSpace[column + 1, row] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column + 1, row);
                }
            }
            //automaticClickNumber = 0;
            //UpLeft
            if ((column - 1 >= 0) && (column - 1 < numberOfColumns) && (row - 1 >= 0) && (row - 1 < numberOfRows))
            {
                if (stateOfMineSpace[column - 1, row - 1] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column - 1, row - 1);
                }
            }
            //Up
            if ((column >= 0) && (column < numberOfColumns) && (row - 1 >= 0) && (row - 1 < numberOfRows))
            {
                if (stateOfMineSpace[column, row - 1] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column, row - 1);
                }
            }
            //UpRight
            if ((column + 1 >= 0) && (column + 1 < numberOfColumns) && (row - 1 >= 0) && (row - 1 < numberOfRows))
            {
                if (stateOfMineSpace[column + 1, row - 1] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column + 1, row - 1);
                }
            }
            //Left
            if ((column - 1 >= 0) && (column - 1 < numberOfColumns) && (row >= 0) && (row < numberOfRows))
            {
                if (stateOfMineSpace[column - 1, row] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column - 1, row);
                }
            }
            //DownLeft
            if ((column - 1 >= 0) && (column - 1 < numberOfColumns) && (row + 1 >= 0) && (row + 1 < numberOfRows))
            {
                if (stateOfMineSpace[column - 1, row + 1] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column - 1, row + 1);
                }
            }
            //Down
            if ((column >= 0) && (column < numberOfColumns) && (row + 1 >= 0) && (row + 1 < numberOfRows))
            {
                if (stateOfMineSpace[column, row + 1] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column, row + 1);
                }
            }
            //Down Right
            if ((column + 1 >= 0) && (column + 1 < numberOfColumns) && (row + 1 >= 0) && (row + 1 < numberOfRows))
            {
                if (stateOfMineSpace[column + 1, row + 1] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column + 1, row + 1);
                }
            }
        }
        private void ClickAdjacentSpacesGeneric(int column, int row)
        {
            //UpLeft
            if ((column - 1 >= 0) && (column - 1 < numberOfColumns) && (row - 1 >= 0) && (row - 1 < numberOfRows))
            {
                if (stateOfMineSpace[column - 1, row - 1] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column - 1, row - 1);
                }
            }
            //Up
            if ((column >= 0) && (column < numberOfColumns) && (row - 1 >= 0) && (row - 1 < numberOfRows))
            {
                if (stateOfMineSpace[column, row - 1] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column, row - 1);
                }
            }
            //UpRight
            if ((column + 1 >= 0) && (column + 1 < numberOfColumns) && (row - 1 >= 0) && (row - 1 < numberOfRows))
            {
                if (stateOfMineSpace[column + 1, row - 1] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column + 1, row - 1);
                }
            }
            //Left
            if ((column - 1 >= 0) && (column - 1 < numberOfColumns) && (row >= 0) && (row < numberOfRows))
            {
                if (stateOfMineSpace[column - 1, row] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column - 1, row);
                }
            }//Right
            if ((column + 1 >= 0) && (column + 1 < numberOfColumns) && (row >= 0) && (row < numberOfRows))
            {
                if (stateOfMineSpace[column + 1, row] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column + 1, row);
                }
            }
            //DownLeft
            if ((column - 1 >= 0) && (column - 1 < numberOfColumns) && (row + 1 >= 0) && (row + 1 < numberOfRows))
            {
                if (stateOfMineSpace[column - 1, row + 1] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column - 1, row + 1);
                }
            }
            //Down
            if ((column >= 0) && (column < numberOfColumns) && (row + 1 >= 0) && (row + 1 < numberOfRows))
            {
                if (stateOfMineSpace[column, row + 1] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column, row + 1);
                }
            }
            //Down Right
            if ((column + 1 >= 0) && (column + 1 < numberOfColumns) && (row + 1 >= 0) && (row + 1 < numberOfRows))
            {
                if (stateOfMineSpace[column + 1, row + 1] != MineSpaceStates.Pressed)
                {
                    RevealMineSpace(column + 1, row + 1);
                }
            }
        }
        #endregion

        private void ExitApplication(object sender, FormClosingEventArgs e)
        {
            if (!exitImmediately)
            {
                DialogResult shouldSave = MessageBox.Show("Are you sure you want to exit without saving your current game?", "Exit Game Dialog", MessageBoxButtons.YesNo);
                if (shouldSave == DialogResult.Yes)
                {
                    Environment.Exit(0);
                }
                else
                {
                    e.Cancel = true;
                }
            }

        }

        #region Menu Operations
        private void MenuNewGame(object sender, EventArgs e)
        {
            exitImmediately = true;
            DialogResult shouldSave = MessageBox.Show("Do you want to save your current game?", "Save Dialog", MessageBoxButtons.YesNo);
            if (shouldSave == DialogResult.Yes)
            {
                ObjectController.createSaveGameDialog(this, GameFilesDialog.ActionsAfterDialog.Restart);
            }
            else
            {
                System.Diagnostics.Process.Start(Application.ExecutablePath);
                Environment.Exit(0);
            }
        }
        private void MenuSaveGame(object sender, EventArgs e)
        {
            new SaveGame(this, GameFilesDialog.ActionsAfterDialog.Nothing);
        }
        private void MenuLoadGame(object sender, EventArgs e)
        {
            ObjectController.createLoadGameDialog(this);
        }
        private void MenuDeleteGame(object sender, EventArgs e)
        {
            ObjectController.deleteSaveGameDialog(this, GameFilesDialog.ActionsAfterDialog.Nothing);
        }
        #endregion
    }

}




