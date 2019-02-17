using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    /// <summary>
    /// This class enables the user to save a game through our user interface.
    /// </summary>
    class SaveGame:SaveTypeDialog
    {
        private int numberOfRows;                       //This variable indicates the number of rows. It has a default constructed value of 16 although it can be customized by user input
        private int numberOfColumns;                    //This variable indicates the number of Columns. It has a default constructed value of 16 although it can be customized by user input
        private GameMap.MineSpaceStates[,] stateOfMineSpace;
        private bool[,] containsMine;
        private GameFilesDialog saveDialog;
        private GameMap GameMapSender;
        private string saveString;
        /// <summary>
        /// Creates an instance of the SaveGame Class.
        /// </summary>
        /// <param name="sender">This is the object that called this constructor.</param>
        /// <param name="actionAfterSave">This is the action that this class should do when it is done.</param>
        /// <param name="NumberOfColumns">This is number of columns that the GameMap that called it has.</param>
        /// <param name="NumberOfRows">This is number of rows that the GameMap that called it has.</param>
        /// <param name="StateOfMineSpace">This is a two diminsional array that represents the state of each mine space that the GameMap that called it has.</param>
        /// <param name="ContainsMine">This is a two diminsional array that represents whether each mine space has a mine for the GameMap object that called it has.</param>
        public SaveGame( GameMap sender, GameFilesDialog.ActionsAfterDialog actionAfterSave, int NumberOfColumns, int NumberOfRows, GameMap.MineSpaceStates[,] StateOfMineSpace, bool[,] ContainsMine)
        {
            numberOfColumns = NumberOfColumns;
            numberOfRows = NumberOfRows;
            stateOfMineSpace = StateOfMineSpace;
            containsMine = ContainsMine;
            GameMapSender = sender;
            saveDialog = new GameFilesDialog(actionAfterSave, sender, this, "save game", "Save");
        }
        /// <summary>
        /// Overwrites the virtual method that the parent has. 
        /// Saves the currently selected save file.
        /// </summary>
        public override void ButtonClicked()
        {
            saveString = saveDialog.getSaveString();
            bool skip = false;
            try
            {
                if (saveDialog.saveStringAlreadyExists())
                {
                    DialogResult shouldSave = MessageBox.Show("Are you sure that you want to overwrite the old save by that name?", "Save", MessageBoxButtons.YesNo);
                    if (shouldSave == DialogResult.No)
                    {
                        skip = true;
                    }
                }
                if (!skip)
                {
                    saveDialog.setDialogDone(true);
                    System.IO.File.WriteAllText(Application.StartupPath + "\\" + saveString + ".txt", "");
                    writeNewLineText(numberOfColumns.ToString());
                    writeNewLineText(numberOfRows.ToString());
                    for (int y = 0; y < numberOfRows; y++)
                    {
                        for (int x = 0; x < numberOfColumns; x++)
                        {
                            if (containsMine[x,y])
                            {
                                writeSpaceText("1");
                            }
                            else
                            {
                                writeSpaceText("0");
                            }
                        }
                    }
                    writeNewLineText("");
                    for (int y = 0; y < numberOfRows; y++)
                    {
                        for (int x = 0; x < numberOfColumns; x++)
                        {
                            int value = Convert.ToInt32(stateOfMineSpace[x, y]);
                            writeSpaceText(value.ToString());
                        }
                    }
                    writeNewLineText("");
                    saveDialog.SaveGameDialogDone();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error encountered while trying to save the file!");
            }

        }
        /// <summary>
        /// Writes a string of text followed by a new line.
        /// </summary>
        /// <param name="text">This is the string that is written</param>
        private void writeNewLineText(string text)
        {
            System.IO.File.AppendAllText(Application.StartupPath + "\\" + saveString + ".txt", text + Environment.NewLine);

        }
        /// <summary>
        /// Writes a string of text followed by a space.
        /// </summary>
        /// <param name="text">This is the string that is written</param>
        private void writeSpaceText(string text)
        {
            System.IO.File.AppendAllText(Application.StartupPath + "\\" + saveString + ".txt", text + " ");
        }
    }
}
