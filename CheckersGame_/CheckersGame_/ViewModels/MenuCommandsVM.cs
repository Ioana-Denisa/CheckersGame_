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
        private ICommand openCommand;
        private ICommand saveCommand;
        private ICommand statisticsCommand;

        public MenuCommandsVM(GameLogic game)
        {
            this.gameLogic = game;
        }

        public ICommand AboutCommand
        {
            get
            {
                if (aboutCommand == null)
                {
                    aboutCommand = new NonGenericCommand(gameLogic.About);
                }
                return this.aboutCommand;
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                if (saveCommand == null)
                {
                    saveCommand = new NonGenericCommand(gameLogic.SaveGame);
                }
                return saveCommand;
            }
        }

        public ICommand OpenCommand
        {
            get
            {
                if (openCommand == null)
                {
                    openCommand = new NonGenericCommand(gameLogic.Open);
                }
                return openCommand;
            }
        }

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

        public ICommand StatisticsCommand
        {
            get
            {
                if(statisticsCommand==null)
                {
                    statisticsCommand=new NonGenericCommand(gameLogic.ShowStatistics);
                }
                return statisticsCommand;
            }
        }
    }
}
