using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    class DeleteSave : SaveTypeDialog
    {
        private GameFilesDialog saveDialog;
        private GameMap GameMapSender;
        private string saveString;
        public DeleteSave(GameMap sender, GameFilesDialog.ActionsAfterDialog actionAfterSave)
        {
            GameMapSender = sender;
            saveDialog = new GameFilesDialog(actionAfterSave, sender, this, "delete more save games", "Delete");
        }
        public override void ButtonClicked()
        {
            saveString = saveDialog.getSaveString();
            try
            {
                if (saveDialog.saveStringAlreadyExists())
                {
                    System.IO.File.Delete(Application.StartupPath + "\\" + saveString + ".txt");
                    saveDialog.PopulateSaveList();
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Error encountered while trying to save the file!");
            }

           
        }
    }
}
