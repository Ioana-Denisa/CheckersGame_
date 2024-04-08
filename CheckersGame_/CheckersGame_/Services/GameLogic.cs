using Checkers.Models;
using Checkers.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersGame_.Services
{
    class GameLogic
    {
        private ObservableCollection<ObservableCollection<Cell>> board;

        public GameLogic(ObservableCollection<ObservableCollection<Cell>>cells)
        {
            this.board = cells;
        }

        public void ClickAction(Cell cell)
        {
            cell.BackgroundCell = Paths.availableMoves;
        }
    }
}
