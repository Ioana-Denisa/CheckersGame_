using CheckersGame_.Commands;
using CheckersGame_.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CheckersGame_.ViewModels
{
    class MenuCommandsVM
    { 
        private GameLogic gameLogic;
        private ICommand aboutCommand;
        private ICommand resetCommand;
        private ICommand loadCommand;
        private ICommand saveCommand;

        public MenuCommandsVM(GameLogic game)
        {
            this.gameLogic = game;
        }

        //public ICommand AboutCommand
        //{
        //    get
        //    {
        //        if (aboutCommand == null)
        //        {
        //            aboutCommand = new NonGenericCommand(gameLogic.About);
        //        }
        //        return this.aboutCommand;
        //    }
        //}

        //public ICommand SaveCommand
        //{
        //    get
        //    {
        //        if (saveCommand == null)
        //        {
        //            saveCommand = new NonGenericCommand(gameBusinessLogic.SaveGame);
        //        }
        //        return saveCommand;
        //    }
        //}

        //public ICommand LoadCommand
        //{
        //    get
        //    {
        //        if (loadCommand == null)
        //        {
        //            loadCommand = new NonGenericCommand(gameBusinessLogic.LoadGame);
        //        }
        //        return loadCommand;
        //    }
        //}

        public ICommand NewCommand
        {
            get
            {
                if (resetCommand == null)
                {
                    resetCommand = new NonGenericCommand(gameLogic.ResetGame);
                }
                return resetCommand;
            }
        }
    }
}
