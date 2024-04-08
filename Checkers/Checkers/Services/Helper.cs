using Checkers.Models;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Checkers.Services
{
    class Helper
    {
        public static Cell currentCell { get; set; }
        public static List<Cell> currentNeighbourns = new List<Cell>();
        //public static Dictionary<Cell,Cell> currentNeighboursCell=new Dictionary<Cell, Cell>();
        private static Player playerTurn=new Player(PieceColor.Red);
        private static bool multipleJumps = false;
        //private static int restOfTheRedPieces=16;
        //private static int restOfTheWhitePieces=16;
        //public const int boardSize = 8;
        public static ObservableCollection<ObservableCollection<Cell>> InitGameBord()
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

        public static List<Cell> CurrentNeighboursCell
        {
            get { return currentNeighbourns; }
            set
            {
                currentNeighbourns = value;
            }
        }



        //public static Dictionary<Cell, Cell> CurrentNeighboursCell
        //{
        //    get { return currentNeighboursCell; }
        //    set
        //    {
        //        currentNeighboursCell = value;
        //    }
        //}

        public static Player PlayerTurn
        {
            get { return playerTurn; }
            set { playerTurn = value; }
        }

        public static bool MultipleJumps
        {
            get { return multipleJumps; }
            set { multipleJumps = value; }
        }

        //public static int RestOfTheWhitePiece
        //{
        //    get { return restOfTheWhitePieces; }
        //    set { restOfTheWhitePieces = value; }
        //}

        //public static int RestOfTheRedPiece
        //{
        //    get { return restOfTheRedPieces; }
        //    set { restOfTheRedPieces = value; }
        //}

        //public static void ResetBoardGame(ObservableCollection<ObservableCollection<Cell>> gameBoard)
        //{
        //    gameBoard.Clear();
        //    ObservableCollection<ObservableCollection<Cell>> newGameBoard = InitGameBord();

        //    foreach (var row in newGameBoard)
        //    {
        //        ObservableCollection<Cell> newRow = new ObservableCollection<Cell>();
        //        foreach (var cell in row)
        //        {
        //            newRow.Add(new Cell(cell.Position.x, cell.Position.y, cell.BackgroundCell, cell.Piece));
        //        }
        //        gameBoard.Add(newRow);
        //    }
        //}


        public static void AllNeighboursCell(Cell cell, List<Position> neighbours)
        {
            if (cell.Piece.TypePiece == PieceType.King)
            {
                neighbours.Add(new Position(-1, -1));
                neighbours.Add(new Position(-1, 1));
                neighbours.Add(new Position(1, -1));
                neighbours.Add(new Position(1, 1));
            }
            else if (cell.Piece.ColorPiece == PieceColor.White)
            {
                neighbours.Add(new Position(1, -1));
                neighbours.Add(new Position(1, 1));
            }
            else
            {
                neighbours.Add(new Position(-1, -1));
                neighbours.Add(new Position(-1, 1));
            }
        }

        //public static void LoadGame(ObservableCollection<ObservableCollection<Cell>> cells)
        //{
        //}


        //public static void SaveGame(ObservableCollection<ObservableCollection<Cell>> cells)
        //{
        //}

        //public static void ResetGame(ObservableCollection<ObservableCollection<Cell>> cells)
        //{
        //}

        //public static void About()
        //{
        //    string pathFile = Paths.aboutFile;

        //    using (var reader = new StreamReader(pathFile))
        //    {
        //        MessageBox.Show(reader.ReadToEnd(), "About", MessageBoxButton.OKCancel);
        //    }
        //}

        //public static void WriteScore(int red, int white)
        //{
        //    string scoreFile = Paths.scoreFile;
        //    using (var writer = new StreamWriter(scoreFile))
        //    {
        //        writer.WriteLine("The scor is-> Red: " + red + " White: " + white);
        //    }
        //}

        //public static Score GetScore()
        //{
        //    Score winner = new Score(0, 0);
        //    string scoreFile = Paths.scoreFile;

        //    using (var reader = new StreamReader(scoreFile))
        //    {
        //        string line = reader.ReadLine();
        //        MatchCollection mathes = Regex.Matches(line, @"\d+");
        //        winner.RedWinner = int.Parse(mathes[0].Value);
        //        winner.WhiteWinner = int.Parse(mathes[1].Value);

        //    }

        //    return winner;
        //}

    }
}

