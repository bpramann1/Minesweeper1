using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    class SaveGame:SaveTypeDialog
    {
        private int numberOfRows;                       //This variable indicates the number of rows. It has a default constructed value of 16 although it can be customized by user input
        private int numberOfColumns;                    //This variable indicates the number of Columns. It has a default constructed value of 16 although it can be customized by user input
        private GameMap.MineSpaceStates[,] stateOfMineSpace;
        private bool[,] containsMine;
        private GameFilesDialog saveDialog;
        private GameMap GameMapSender;
        private string saveString;
        public SaveGame( GameMap sender, GameFilesDialog.ActionsAfterDialog actionAfterSave, int NumberOfColumns, int NumberOfRows, GameMap.MineSpaceStates[,] StateOfMineSpace, bool[,] ContainsMine)
        {
            numberOfColumns = NumberOfColumns;
            numberOfRows = NumberOfRows;
            stateOfMineSpace = StateOfMineSpace;
            containsMine = ContainsMine;
            GameMapSender = sender;
            saveDialog = new GameFilesDialog(actionAfterSave, sender, this, "save game", "Save");
        }
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
        private void writeNewLineText(string text)
        {
            System.IO.File.AppendAllText(Application.StartupPath + "\\" + saveString + ".txt", text + Environment.NewLine);

        }
        private void writeSpaceText(string text)
        {
            System.IO.File.AppendAllText(Application.StartupPath + "\\" + saveString + ".txt", text + " ");
        }
    }
}
