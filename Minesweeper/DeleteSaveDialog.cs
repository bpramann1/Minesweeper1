using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    /// <summary>
    /// This class is here to enable the user to delete save games through our interface.
    /// </summary>
    class DeleteSave : SaveTypeDialog
    {
        private GameFilesDialog saveDialog;
        private GameMap GameMapSender;
        private string saveString;
        /// <summary>
        /// This constructor creates an instance of the DeleteSave Class.
        /// </summary>
        /// <param name="sender">This is the object that created this instance</param>
        /// <param name="actionAfterSave">This represents the action that we should take after we are done deleting games.</param>
        public DeleteSave(GameMap sender, GameFilesDialog.ActionsAfterDialog actionAfterSave)
        {
            GameMapSender = sender;
            saveDialog = new GameFilesDialog(actionAfterSave, sender, this, "delete more save games", "Delete");
        }
        /// <summary>
        /// This method overwrites the virtual method that the parent has. 
        /// This method is used delete the currently selected save file.
        /// </summary>
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
