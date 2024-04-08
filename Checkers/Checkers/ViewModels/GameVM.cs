using Checkers.Models;
using Checkers.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Checkers.ViewModels
{
    class GameVM
    {
        private GameBusinessLogic bl;
        public MenuCommandsVM menuCommands {  get; set; }
        public ObservableCollection<ObservableCollection<GameCommandsVM>> gameBoard {  get; set; }
        public ScoreVM scoreVM { get; set; }
        public PlayerVM playerVM { get; set; }
        
        public GameVM()
        {
            ObservableCollection<ObservableCollection<Cell>> board = Helper.InitGameBord();
            Player player = new Player(PieceColor.Red);
            Score score = new Score(0, 0);
            bl = new GameBusinessLogic(board);
            gameBoard = CellBoardToCellVMBoard(board);
            playerVM = new PlayerVM(bl, player);
            scoreVM = new ScoreVM(bl, score);
            menuCommands = new MenuCommandsVM(bl);
        }

        private ObservableCollection<ObservableCollection<GameCommandsVM>> CellBoardToCellVMBoard(ObservableCollection<ObservableCollection<Cell>> board)
        {
            ObservableCollection<ObservableCollection<GameCommandsVM>> result = new ObservableCollection<ObservableCollection<GameCommandsVM>>();
            for (int i = 0; i < board.Count; i++)
            {
                ObservableCollection<GameCommandsVM> line = new ObservableCollection<GameCommandsVM>();
                for (int j = 0; j < board[i].Count; j++)
                {
                    Cell c = board[i][j];
                    GameCommandsVM cellVM = new GameCommandsVM( bl,c);
                    line.Add(cellVM);
                }
                result.Add(line);
            }
            return result;
        }
    }
}
