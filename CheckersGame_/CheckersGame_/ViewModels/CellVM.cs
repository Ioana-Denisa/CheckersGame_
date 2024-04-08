using Checkers.Commands;
using Checkers.Models;
using CheckersGame_.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CheckersGame_.ViewModels
{
    class CellVM:BaseNotification
    {
        public GameLogic gameLogicVM;
        public Cell cellVM;

        private ICommand clickCommand;

        public CellVM(GameLogic game, Cell cell)
        {
           gameLogicVM=game;
            cellVM=cell;
        }


        public Cell CellViewModel
        {
            get
            {
                return cellVM;
            }
            set
            {
                this.cellVM = value;
                NotifyPropertyChanged("CellViewModel");
            }
        }

        public ICommand ClickCommand

        {
            get
            {
                if (clickCommand == null)
                {
                    clickCommand = new RelayCommand<Cell>(gameLogicVM.ClickAction);
                }
                return clickCommand;
            }
        }
    }
}
