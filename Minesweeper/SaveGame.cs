using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    class SaveGame
    {

        private Form saveGameForm;
        private GameMap GameMapSender;

        public enum ActionsAfterSave
        {
            Nothing,
            Restart,
            Exit
        }
        private ActionsAfterSave actionAfterSave;

        public SaveGame(GameMap sender, ActionsAfterSave ActionAfterSave)
        {
            actionAfterSave = ActionAfterSave;
            GameMapSender = sender;
            CreateSaveGameDialog();
        }

        public void CreateSaveGameDialog()
        {
            saveGameForm = new Form();


        }
        public void SaveGameDialogDone()
        {
            switch (actionAfterSave)
            {
                case ActionsAfterSave.Restart:
                    System.Diagnostics.Process.Start(Application.ExecutablePath);
                    Environment.Exit(0);
                    break;
                case ActionsAfterSave.Exit:
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }
        
        private void ExitButtonClicked(object sender, FormClosingEventArgs e)
        {
            DialogResult shouldSave = MessageBox.Show("Are you sure that you do not want to save?", "Save", MessageBoxButtons.YesNo);
            if (shouldSave == DialogResult.Yes)
            {
                SaveGameDialogDone();
            }
            else
            {
                e.Cancel = true;
            }

        }
    }
}
