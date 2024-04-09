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
        private GameLogic gameLogicVM;
        private Cell cellViewModel;

        private ICommand clickCommand;

        public CellVM(GameLogic game, Cell cell)
        {
           gameLogicVM=game;
            cellViewModel=cell;
        }


        public Cell CellViewModel
        {
            get
            {
                return cellViewModel;
            }
            set
            {
                this.cellViewModel = value;
                NotifyPropertyChanged("CellViewModel");
            }
        }

        public string IsAvailable
        {
            get { return cellViewModel.IsAvailable; }
            set
            {
                this.IsAvailable = value;
                NotifyPropertyChanged();
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
