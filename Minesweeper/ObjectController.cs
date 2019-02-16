using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Minesweeper.GameMap;

namespace Minesweeper
{
    static class ObjectController
    {
        public static GameMap gameMap;
        public static SaveGame SaveGame;
        public static LoadGame LoadGame;
        public static DeleteSave DeleteGame;

        public static void createGameMap()
        {
            gameMap = new GameMap();
        }
        public static void createGameMapFromLoad(int NumberOfColumns, int NumberOfRows, bool[,] ContainsMine, MineSpaceStates[,] StateOfMineSpace)
        {
            gameMap = new GameMap(true,NumberOfColumns,NumberOfRows,ContainsMine,StateOfMineSpace);
        }
        public static void createSaveGameDialog(GameMap sender, GameFilesDialog.ActionsAfterDialog actionAfterSave)
        {
            SaveGame = new SaveGame(sender, actionAfterSave);
        }
        public static void createLoadGameDialog(GameMap sender)
        {
            LoadGame = new LoadGame(sender);
        }
        public static void deleteSaveGameDialog(GameMap sender, GameFilesDialog.ActionsAfterDialog actionAfterSave)
        {
            DeleteGame = new DeleteSave(sender, actionAfterSave);
        }
    }
}
