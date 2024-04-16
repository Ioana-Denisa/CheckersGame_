using Checkers.Models;
using CheckersGame_.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CheckersGame_.ViewModels
{
    class GameVM : BaseNotification
    {
        private GameLogic game;
        private Player playerTurn;
        private PieceService pieceService;
        private int redPiece;
        private int whitePiece;

        public MenuCommandsVM menuCommands { get; set; }
        public GameVM()
        {
            ObservableCollection<ObservableCollection<Cell>> board = Helper.InitGameBoard();
            PlayerTurn = new Player(PieceColor.Red);
            PieceService = new PieceService(this);
            game = new GameLogic(board, PlayerTurn, pieceService);
            gameBoard = CellBoardToCellVMBoard(board);
            menuCommands = new MenuCommandsVM(game);
            redPiece = Helper.GetScore().RedWinner;
            whitePiece = Helper.GetScore().WhiteWinner;
        }

        private ObservableCollection<ObservableCollection<CellVM>> CellBoardToCellVMBoard(ObservableCollection<ObservableCollection<Cell>> board)
        {
            ObservableCollection<ObservableCollection<CellVM>> result = new ObservableCollection<ObservableCollection<CellVM>>();
            for (int i = 0; i < board.Count; i++)
            {
                ObservableCollection<CellVM> line = new ObservableCollection<CellVM>();
                for (int j = 0; j < board[i].Count; j++)
                {
                    Cell c = board[i][j];
                    CellVM cellVM = new CellVM(game, c);
                    line.Add(cellVM);
                }
                result.Add(line);
            }
            return result;
        }

        public int RedPieces
        {
            get { return redPiece; }
            set
            {
                redPiece = value;
                NotifyPropertyChanged("RedPieces");
            }
        }

        public int WhitePieces
        {
            get { return whitePiece; }
            set
            {
                whitePiece = value;
                NotifyPropertyChanged("WhitePieces");
            }
        }

        public Player PlayerTurn
        {
            get { return playerTurn; }
            set
            {
                playerTurn = value;
                NotifyPropertyChanged("PlayerTurn");
            }
        }

        public PieceService PieceService
        {
            get { return pieceService; }
            set
            {
                pieceService = value;
                NotifyPropertyChanged("PieceService");
            }
        }

        public GameLogic GameLogic
        {
            get { return game; }
            set
            {
                this.game = value;
                NotifyPropertyChanged("GameLogic");
            }
        }

        public ObservableCollection<ObservableCollection<CellVM>> GameBoard
        {
            get { return gameBoard; }
            set
            {
                gameBoard = value;
                NotifyPropertyChanged("GameBoard");
            }
        }

        public ObservableCollection<ObservableCollection<CellVM>> gameBoard;
    }
}
