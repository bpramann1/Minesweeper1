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
        
        public static void createGameMap()
        {
            gameMap = new GameMap();
        }
        public static void createGameMapFromLoad(int NumberOfColumns, int NumberOfRows, bool[,] ContainsMine, MineSpaceStates[,] StateOfMineSpace)
        {
            gameMap = new GameMap(true,NumberOfColumns,NumberOfRows,ContainsMine,StateOfMineSpace);
        }
        public static void createSaveGameDialog()
        {

        }
        public static void createLoadGameDialog(GameMap sender)
        {
            LoadGame = new LoadGame(sender);
        }
    }
}
