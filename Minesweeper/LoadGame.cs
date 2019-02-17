using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    /// <summary>
    /// Handles the loading of a previously saved game 
    /// </summary>
    class LoadGame : SaveTypeDialog
    {
        private int numberOfRows;                       //This variable indicates the number of rows. It has a default constructed value of 16 although it can be customized by user input
        private int numberOfColumns;                    //This variable indicates the number of Columns. It has a default constructed value of 16 although it can be customized by user input
        private GameMap.MineSpaceStates[,] stateOfMineSpace;
        private bool[,] containsMine;
        private GameFilesDialog loadDialog;
        private GameMap GameMapSender;
        private string saveString;
        private bool gameExists;

        /// <summary>
        /// Presents the user with a window that allows them to load a game. 
        /// </summary>
        /// <param name="sender">
        /// The game map that called the function
        /// </param>
        public LoadGame(GameMap sender)
        {
            GameMapSender = sender;
            loadDialog = new GameFilesDialog(GameFilesDialog.ActionsAfterDialog.Nothing, sender, this, "load game", "Load");
        }

        /// <summary>
        /// Loads the previously saved game selected by the user 
        /// </summary>
        public override void ButtonClicked()
        {
            saveString = loadDialog.getSaveString();
            gameExists = loadDialog.saveStringAlreadyExists();
            try
            {
                if (!gameExists)
                {
                    DialogResult shouldSave = MessageBox.Show("No saved game by that name was found. Try again?", "Save", MessageBoxButtons.YesNo);
                    if (shouldSave == DialogResult.No)
                    {
                        loadDialog.setDialogDone(true);
                    }
                }
                else
                {
  
                    string[] linesOfText = ReadTextFile();

                    
                    int.TryParse(LoadNewLineText(linesOfText, 0), out numberOfColumns);
                    int.TryParse(LoadNewLineText(linesOfText, 1), out numberOfRows);
                    containsMine = LoadBoolArrayFromSpaceTextFixedLength(LoadNewLineText(linesOfText, 2), numberOfColumns, numberOfRows);
                    stateOfMineSpace = LoadEnumArrayFromSpaceTextFixedLength(LoadNewLineText(linesOfText, 3), numberOfColumns, numberOfRows);
                    ObjectController.createGameMapFromLoad(numberOfColumns, numberOfRows, containsMine, stateOfMineSpace);
                    loadDialog.setDialogDone(true);
                    GameMapSender.setExitImmediately(true);
                    GameMapSender.Game.Close();
                    loadDialog.GameFilesForm.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error encountered while trying to load the file!");
            }

        }

        /// <summary>
        /// Reads a text file into an array of strings
        /// </summary>
        /// <returns>
        /// An array of strings that contains the text from the file 
        /// </returns>
        private string[] ReadTextFile()
        {
           return System.IO.File.ReadAllLines(Application.StartupPath + "\\" + saveString + ".txt");
        }

        /// <summary>
        /// returns a string from an index of an array of strings 
        /// </summary>
        /// <param name="text">
        /// The array of strings to take the text from 
        /// </param>
        /// <param name="index">
        /// The index of the array to return 
        /// </param>
        /// <returns>
        /// The string at text[index]
        /// </returns>
        private string LoadNewLineText(string[] text, int index)
        {
            return text[index];
        }

        /// <summary>
        /// Parses an array of intes from a string
        /// </summary>
        /// <param name="text">
        /// The string to be parsed 
        /// </param>
        /// <param name="numberOfColumns">
        /// The number of columns in the game 
        /// </param>
        /// <param name="numberOfRows">
        /// The number of rows in the game 
        /// </param>
        /// <returns>
        /// The array of ints parsed from the string 
        /// </returns>
        private int[,] LoadIntArrayFromSpaceTextFixedLength(string text, int numberOfColumns, int numberOfRows)
        {
            string remainingText = text;
            int[,] array = new int[numberOfColumns,numberOfRows];
            int indexOfNextSpace;
            for (int y = 0; y < numberOfRows; y++)
            {
                for (int x = 0; x < numberOfColumns; x++)
                {
                    indexOfNextSpace = remainingText.IndexOf(" ");
                    int.TryParse(remainingText.Substring(0, indexOfNextSpace+1), out array[x,y]);                   
                    remainingText = remainingText.Substring(indexOfNextSpace+1);
                }
            }
            return array;
        }

        /// <summary>
        /// Loads the state of each cell from a string 
        /// </summary>
        /// <param name="text">
        /// The string to be parsed 
        /// </param>
        /// <param name="numberOfColumns">
        /// The number of columns in the game 
        /// </param>
        /// <param name="numberOfRows">
        /// The number of rows in the game 
        /// </param>
        /// <returns>
        /// An array of enums that represents the state of each cell 
        /// </returns>
        private GameMap.MineSpaceStates[,] LoadEnumArrayFromSpaceTextFixedLength(string text, int numberOfColumns, int numberOfRows)
        {
            string remainingText = text;
            GameMap.MineSpaceStates[,] array = new GameMap.MineSpaceStates[numberOfColumns, numberOfRows];
            int currentlyParsedValue;
            int indexOfNextSpace;
            for (int y = 0; y < numberOfRows; y++)
            {
                for (int x = 0; x < numberOfColumns; x++)
                {
                    indexOfNextSpace = remainingText.IndexOf(" ");
                    int.TryParse(remainingText.Substring(0, indexOfNextSpace+1), out currentlyParsedValue);
                    array[x, y] = (GameMap.MineSpaceStates)currentlyParsedValue;
                    remainingText = remainingText.Substring(indexOfNextSpace+1);
                }
            }
            return array;
        }

        /// <summary>
        /// Loads an array of bools from a string that represents whether or not each cell contains a mine 
        /// </summary>
        /// <param name="text">
        /// The string to be parsed 
        /// </param>
        /// <param name="numberOfColumns">
        /// The number of columns in the game 
        /// </param>
        /// <param name="numberOfRows">
        /// The number of rows in the game 
        /// </param>
        /// <returns>
        /// An array of bools that represents whether or not each cell contains a mine 
        /// </returns>
        private bool[,] LoadBoolArrayFromSpaceTextFixedLength(string text, int numberOfColumns, int numberOfRows)
        {
            string remainingText = text;
            bool[,] array = new bool[numberOfColumns, numberOfRows];
            int indexOfNextSpace;
            for (int y = 0; y < numberOfRows; y++)
            {
                for (int x = 0; x < numberOfColumns; x++)
                {
                   indexOfNextSpace = remainingText.IndexOf(" ");
                   array[x, y] = (remainingText.Substring(0, indexOfNextSpace) == "1");
                   remainingText = remainingText.Substring(indexOfNextSpace+1);
                }
            }
            return array;
        }
    }
}
