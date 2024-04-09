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
    class Helper
    {
        public static Cell CurrentCell  { get; set; }
        public static List<Cell> currentNeighbours = new List<Cell>();
        public static Player playerTurn=new Player(PieceColor.Red);
        public const int boardSize = 8;
        public static bool extraMove;
        public static ObservableCollection<ObservableCollection<Cell>> InitGameBoard()
        {
            ObservableCollection<ObservableCollection<Cell>> gameBoard = new ObservableCollection<ObservableCollection<Cell>>();
            for (int row = 0; row < 8; row++)
            {
                ObservableCollection<Cell> rowCells = new ObservableCollection<Cell>();
                for (int col = 0; col < 8; col++)
                {
                    string imagePath = GetBackgroundForRowCol(row, col);
                    Piece piece = GetPiece(row, col);
                    rowCells.Add(new Cell(row, col, imagePath, piece));
                }

                gameBoard.Add(rowCells);
            }

            return gameBoard;
        }

        private static Piece GetPiece(int row, int col)
        {
            Piece piece = new Piece();
            if ((row + col) % 2 != 0 && row <= 2)
            {
                piece.TypePiece = PieceType.Regular;
                piece.ColorPiece = PieceColor.White;
                piece.ImagePath = Paths.whitePiece;

                return piece;
            }
            else if ((row + col) % 2 != 0 && row > 4)
            {
                piece.TypePiece = PieceType.Regular;
                piece.ColorPiece = PieceColor.Red;
                piece.ImagePath = Paths.redPiece;

                return piece;
            }
            else
                return null;
        }



        private static string GetBackgroundForRowCol(int row, int col)
        {
            if ((row + col) % 2 == 0)
                return Paths.backgroundLight;
            else
                return Paths.backgroundDark;

        }
       
        public static List<Cell> CurrentNeighbourns
        {
            get { return currentNeighbours; }
            set {  currentNeighbours = value; }
        }

        public static Player PlayerTurn
        {
            get { return playerTurn; }
            set { playerTurn=value; }
        }

        public static bool ExtraMove
        {
            get { return extraMove; }
            set
            {
                extraMove = value;
            }
        }

        public static void SearchAllNeighboursForCell(Cell cell, List<Position>allNeighbours)
        {
            if(cell.Piece!=null)
            {
                if (cell.Piece.TypePiece == PieceType.King)
                {
                    allNeighbours.Add(new Position(-1, -1));
                    allNeighbours.Add(new Position(-1, 1));
                    allNeighbours.Add(new Position(1, -1));
                    allNeighbours.Add(new Position(1, 1));
                }
                else
                {
                    if (cell.Piece.ColorPiece == PieceColor.Red)
                    {
                        allNeighbours.Add(new Position(-1, -1));
                        allNeighbours.Add(new Position(-1, 1));
                    }
                    else
                    {
                        allNeighbours.Add(new Position(1, -1));
                        allNeighbours.Add(new Position(1, 1));
                    }
                }
            }
        }


        public static void ResetGame(ObservableCollection<ObservableCollection<Cell>> board)
        { 
            board=new ObservableCollection<ObservableCollection<Cell>>();
            for (int row = 0; row < 8; row++)
            {
                ObservableCollection<Cell> rowCells = new ObservableCollection<Cell>();
                for (int col = 0; col < 8; col++)
                {
                    string imagePath = GetBackgroundForRowCol(row, col);
                    Piece piece = GetPiece(row, col);
                    rowCells.Add(new Cell(row, col, imagePath, piece));
                }

                board.Add(rowCells);
            }
        }


    }
}
