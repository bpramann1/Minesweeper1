using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    class LoadGame : SaveTypeDialog
    {
        public int numberOfRows;                       //This variable indicates the number of rows. It has a default constructed value of 16 although it can be customized by user input
        public int numberOfColumns;                    //This variable indicates the number of Columns. It has a default constructed value of 16 although it can be customized by user input
        public GameMap.MineSpaceStates[,] stateOfMineSpace;
        public bool[,] containsMine;
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
            loadDialog = new GameFilesDialog(GameFilesDialog.ActionsAfterDialog.Nothing, sender, this, "load game");
            loadDialog.saveGameButton.Text = "Load";
        }


        public override void ButtonClicked()
        {
            saveString = loadDialog.saveString;
            gameExists = loadDialog.alreadyExists();
            try
            {
                if (!gameExists)
                {
                    DialogResult shouldSave = MessageBox.Show("No saved game by that name was found. Try again?", "Save", MessageBoxButtons.YesNo);
                    if (shouldSave == DialogResult.No)
                    {
                        loadDialog.dialogDone = true;
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
                    loadDialog.dialogDone = true;
                    GameMapSender.exitImmediately = true;
                    GameMapSender.Game.Close();
                    loadDialog.GameFilesForm.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error encountered while trying to load the file!");
            }

        }
        private string[] ReadTextFile()
        {
           return System.IO.File.ReadAllLines(Application.StartupPath + "\\" + saveString + ".txt");
        }
        private string LoadNewLineText(string[] text, int index)
        {
            return text[index];
        }
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
