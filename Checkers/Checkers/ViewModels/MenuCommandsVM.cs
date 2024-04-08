using Checkers.Commands;
using Checkers.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Checkers.ViewModels
{
    class MenuCommandsVM
    {
        private GameBusinessLogic gameBusinessLogic;
        private ICommand aboutCommand;
        private ICommand resetCommand;
        private ICommand loadCommand;
        private ICommand saveCommand;

        public MenuCommandsVM(GameBusinessLogic businessLogic)
        {
            this.gameBusinessLogic = businessLogic;
        }

        //public ICommand AboutCommand
        //{
        //    get
        //    {
        //        if (aboutCommand == null)
        //        {
        //            aboutCommand = new NonGenericCommand(gameBusinessLogic.About);
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

        //public ICommand NewCommand
        //{
        //    get
        //    {
        //        if (resetCommand == null)
        //        {
        //            resetCommand = new NonGenericCommand(gameBusinessLogic.ResetGame);
        //        }
        //        return resetCommand;
        //    }
        //}
    }
}
    

