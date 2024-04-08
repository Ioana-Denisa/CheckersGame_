using Checkers.Commands;
using Checkers.Models;
using Checkers.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Checkers.ViewModels
{
    class GameCommandsVM:BaseNotification
    {
        private GameBusinessLogic businessLogic;
        private Cell cellVM;
        private ICommand selectPieceCommand;
        private ICommand movePieceCommand;

        public GameCommandsVM(GameBusinessLogic businessLogic, Cell cell)
        {
            this.businessLogic = businessLogic;
            this.cellVM = cell;
        }


        public Cell CellVM
        {
            get { return cellVM; }
            set
            {
                this.cellVM=value;
                NotifyPropertyChanged("CellVM");
            }
        }

        public ICommand SelectPieceCommand
        {
            get
            {
                if (selectPieceCommand == null)
                {
                    selectPieceCommand = new RelayCommand<Cell>(businessLogic.SelectPiece);
                }
                return selectPieceCommand;
            }
        }


        public ICommand MovePieceCommand
        {
            get
            {
                if (movePieceCommand == null)
                {
                    movePieceCommand = new RelayCommand<Cell>(businessLogic.MovePiece);
                }
                return movePieceCommand;
            }
        }
    }
}
