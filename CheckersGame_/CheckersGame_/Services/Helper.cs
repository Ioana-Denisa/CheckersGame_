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
        public static Cell currentCell {  get; set; }
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
    }
}
