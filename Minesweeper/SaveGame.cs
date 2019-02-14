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

        public SaveGame(GameMap sender)
        {
            GameMapSender = sender;
            CreateSaveGameDialog();
        }

        public void CreateSaveGameDialog()
        {
            saveGameForm = new Form();

        }
    }
}
