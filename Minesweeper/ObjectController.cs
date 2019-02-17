using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Minesweeper.GameMap;

namespace Minesweeper
{ 
    /// <summary>
    /// This class is used to create objects.
    /// </summary>
    static class ObjectController
    {
        private static GameMap gameMap;
        private static SaveGame SaveGame;
        private static LoadGame LoadGame;
        private static DeleteSave DeleteGame;
        /// <summary>
        /// This method creates an instance of the GameMap class.
        /// </summary>
        public static void createGameMap()
        {
            gameMap = new GameMap();
        }
        /// <summary>
        /// This method creates an instance of the GameMap class from a load.
        /// </summary>
        /// <param name="NumberOfColumns">This is the number of columns that the new map will have.</param>
        /// <param name="NumberOfRows">This is the number of rows that the new map will have.</param>
        /// <param name="ContainsMine">This is a two diminsional array of bool that describes if there is a mine at the current position of the array. A value of 1 means that the array contains a mine.</param>
        /// <param name="StateOfMineSpace">This is a two diminsional array of an enum. The enum represents the current state of a mine space that is denoted by the column and row index.</param>
        public static void createGameMapFromLoad(int NumberOfColumns, int NumberOfRows, bool[,] ContainsMine, MineSpaceStates[,] StateOfMineSpace)
        {
            gameMap = new GameMap(true,NumberOfColumns,NumberOfRows,ContainsMine,StateOfMineSpace);
        }
        /// <summary>
        /// This method creates an instance of the SaveGame class.
        /// </summary>
        /// <param name="sender">This is the object that called this method</param>
        /// <param name="actionAfterSave">This is an enum value that tells the SaveGame instance what it should do when it is done.</param>
        /// <param name="NumberOfColumns">This value represents how many columns the object that called this method has.</param>
        /// <param name="NumberOfRows">This value represents how many rows the object that called this method has.</param>
        /// <param name="StateOfMineSpace">This is a two diminsional array of an enum. The enum represents the current state of a mine space that is denoted by the column and row index.</param>
        /// <param name="ContainsMine">This is a two diminsional array of bool that describes if there is a mine at the current position of the array. A value of 1 means that the array contains a mine.</param>
        public static void createSaveGameDialog(GameMap sender, GameFilesDialog.ActionsAfterDialog actionAfterSave, int NumberOfColumns, int NumberOfRows, GameMap.MineSpaceStates[,] StateOfMineSpace, bool[,] ContainsMine)
        {
            SaveGame = new SaveGame(sender, actionAfterSave, NumberOfColumns , NumberOfRows, StateOfMineSpace, ContainsMine);
        }
        /// <summary>
        /// This method creates an instance of the load game class.
        /// </summary>
        /// <param name="sender">This is the object that called this method.</param>
        public static void createLoadGameDialog(GameMap sender)
        {
            LoadGame = new LoadGame(sender);
        }
        /// <summary>
        /// This method creates an instance of the DeleteSave class.
        /// </summary>
        /// <param name="sender">This is the object that called this method.</param>
        /// <param name="actionAfterSave">This is what the DeleteSave instance should do after it is done.</param>
        public static void deleteSaveGameDialog(GameMap sender, GameFilesDialog.ActionsAfterDialog actionAfterSave)
        {
            DeleteGame = new DeleteSave(sender, actionAfterSave);
        }
    }
}
