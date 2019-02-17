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
        private static GameMap gameMap;
        private static SaveGame SaveGame;
        private static LoadGame LoadGame;
        private static DeleteSave DeleteGame;

        public static void createGameMap()
        {
            gameMap = new GameMap();
        }
        public static void createGameMapFromLoad(int NumberOfColumns, int NumberOfRows, bool[,] ContainsMine, MineSpaceStates[,] StateOfMineSpace)
        {
            gameMap = new GameMap(true,NumberOfColumns,NumberOfRows,ContainsMine,StateOfMineSpace);
        }
        public static void createSaveGameDialog(GameMap sender, GameFilesDialog.ActionsAfterDialog actionAfterSave, int NumberOfColumns, int NumberOfRows, GameMap.MineSpaceStates[,] StateOfMineSpace, bool[,] ContainsMine)
        {
            SaveGame = new SaveGame(sender, actionAfterSave, NumberOfColumns , NumberOfRows, StateOfMineSpace, ContainsMine);
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
