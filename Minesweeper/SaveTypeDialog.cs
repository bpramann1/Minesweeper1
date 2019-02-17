using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    /// <summary>
    /// This class is here to be inherited by the SaveGame, LoadGame, and Delete game class. This class is basically virtual.
    /// </summary>
    class SaveTypeDialog
    {
        /// <summary>
        /// This creates an instance of the Save Type Class. This should not be used.
        /// </summary>
        public SaveTypeDialog()
        {

        }
        /// <summary>
        /// This method is a virtual method that should be overwritten by all the children of this class.
        /// </summary>
        public virtual void ButtonClicked()
        {

        }
    }
}
