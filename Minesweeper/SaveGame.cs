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
        private GameFilesDialog saveDialog;
        private GameMap GameMapSender;
        private string saveString;
        public SaveGame( GameMap sender, GameFilesDialog.ActionsAfterDialog actionAfterSave)
        {
            GameMapSender = sender;
            saveDialog = new GameFilesDialog(actionAfterSave, sender, this, "save game");
            saveDialog.saveGameButton.Text = "Save";
        }
        public override void ButtonClicked()
        {
            saveString = saveDialog.saveString;
            bool skip = false;
            try
            {
                if (saveDialog.alreadyExists())
                {
                    DialogResult shouldSave = MessageBox.Show("Are you sure that you want to overwrite the old save by that name?", "Save", MessageBoxButtons.YesNo);
                    if (shouldSave == DialogResult.No)
                    {
                        skip = true;
                    }
                }
                if (!skip)
                {
                    saveDialog.dialogDone = true;
                    System.IO.File.WriteAllText(Application.StartupPath + "\\" + saveString + ".txt", "");
                    writeNewLineText(GameMapSender.numberOfColumns.ToString());
                    writeNewLineText(GameMapSender.numberOfRows.ToString());
                    for (int y = 0; y < GameMapSender.numberOfRows; y++)
                    {
                        for (int x = 0; x < GameMapSender.numberOfColumns; x++)
                        {
                            if (GameMapSender.containsMine[x,y])
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
                    for (int y = 0; y < GameMapSender.numberOfRows; y++)
                    {
                        for (int x = 0; x < GameMapSender.numberOfColumns; x++)
                        {
                            int value = Convert.ToInt32(GameMapSender.stateOfMineSpace[x, y]);
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
