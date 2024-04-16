using Checkers.Models;
using Checkers.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace CheckersGame_.Services
{
    class Helper
    {
        public static Cell CurrentCell  { get; set; }
        public static List<Cell> currentNeighbours = new List<Cell>();
        public static Player playerTurn=new Player(PieceColor.Red);
        public const int boardSize = 8;
        public static bool multiple;


        public static ObservableCollection<ObservableCollection<Cell>> InitGameBoard()
        {
            ObservableCollection<ObservableCollection<Cell>> gameBoard = new ObservableCollection<ObservableCollection<Cell>>();
            for (int row = 0; row < boardSize; row++)
            {
                ObservableCollection<Cell> rowCells = new ObservableCollection<Cell>();
                for (int col = 0; col < boardSize; col++)
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
                return Paths.backgroundDark;
            else
                return Paths.backgroundLight;

        }
       
        public static List<Cell> CurrentNeighbourns
        {
            get { return currentNeighbours; }
            set {  currentNeighbours = value; }
        }

        public static Player PlayerTurn
        {
            get { return playerTurn; }
            set { playerTurn=value;
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


        public static void ResetBoardGame(ObservableCollection<ObservableCollection<Cell>> board)
        {
            for (int row = 0; row < boardSize; row++)
            {
                for (int col = 0; col < boardSize; col++)
                {
                    Piece piece = GetPiece(row, col);
                    board[row][col].Piece = piece;
                }
            }
        }

        public static void ResetGame(ObservableCollection<ObservableCollection<Cell>> cells,PieceService gameServices)
        {
            currentNeighbours.Clear();
            CurrentCell = null;
            gameServices.WhitePieces = 12;
            gameServices.RedPieces = 12;
            playerTurn.Color = PieceColor.Red;
            ResetBoardGame(cells);

        }
        public static void About()
        {
            string pathFile = Paths.aboutFile;

            using (var reader = new StreamReader(pathFile))
            {
                MessageBox.Show(reader.ReadToEnd(), "About 😊", MessageBoxButton.OK);
            }
        }


        public static void WriteScore(int red, int white)
        {
            string scoreFile = Paths.scoreFile;
            using (var writer = new StreamWriter(scoreFile))
            {
                writer.WriteLine(red+" "+white);
            }
        }

        public static Score GetScore()
        {
            Score score = new Score(0, 0);
            string scoreFile = Paths.scoreFile;

            using (var reader = new StreamReader(scoreFile))
            {
                string line = reader.ReadLine();
                string[] scores = line.Split(' ');
                score.RedWinner = int.Parse(scores[0]);
                score.WhiteWinner = int.Parse(scores[1]);
            }
            return score;
        }

        public static void SaveGame(ObservableCollection<ObservableCollection<Cell>> board,PieceService gameServices, bool multipleJump)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            bool? answer = saveFileDialog.ShowDialog();

            if(answer==true)
            {
                var pathFile=saveFileDialog.FileName;
                using(var writer =new StreamWriter(pathFile))
                {
                    writer.WriteLine(multipleJump);
                    writer.WriteLine(playerTurn.Color);
                    writer.WriteLine(gameServices.RedPieces);
                    writer.WriteLine(gameServices.WhitePieces);

                    foreach(var line in board)
                    {
                        foreach(var cell in line)
                        {
                            writer.Write(cell.Position.x + "," + cell.Position.y + ",");
                            if (cell.BackgroundCell == Paths.backgroundLight)
                                writer.Write("light");
                            else
                                writer.Write("dark");

                            if (cell.Piece != null)
                            {
                                writer.Write("," + cell.Piece.TypePiece + "," + cell.Piece.ColorPiece);
                            }
                            else
                                writer.Write(",null");
                            writer.WriteLine();
                        }
                    }

                }
            }
        }

        public static PieceColor GetColor( string color)
        {
            if (color == "Red")
                return PieceColor.Red;
            else
                return PieceColor.White;
        }

        public static PieceType GetType(string type)
        {
            if (type == "King")
                return PieceType.King;
            else
                return PieceType.Regular;
        }

        public static string GetPathBackground(string text)
        {
            if (text == "dark")
               return Paths.backgroundDark;
            else
                return Paths.backgroundLight;
        }
        public static string GetPathPiece(string type, string color)
        {
            if (type == "Regular")
            {
                if (color == "Red")
                    return Paths.redPiece;
                else
                    return Paths.whitePiece;
            }
            else
            {
                if (color == "Red")
                    return Paths.redKingPiece;
                else
                    return Paths.whiteKingPiece;
            }
        }

        public static int GetInt(string text)
        {
            return int.Parse(text);
        }


        public static void  OpenGame(ObservableCollection<ObservableCollection<Cell>> board , PieceService gameServices, Player turn)
        {
           
            OpenFileDialog openFileDialog=new OpenFileDialog();
            bool? answer = openFileDialog.ShowDialog();

            if(answer== true)
            {
                string pathFile=openFileDialog.FileName;

                using(var reader=new StreamReader(pathFile)) 
                {

                    string text;
                    text = reader.ReadLine();
                    if(text =="True")
                        multiple = true;
                    else
                        multiple= false;

                    text = reader.ReadLine();
                    if (text == "Red")
                    {
                        playerTurn.Color = PieceColor.Red;
                        playerTurn.ImagePath=Paths.redPiece;
                        turn.ImagePath = playerTurn.ImagePath;
                        turn.Color = playerTurn.Color;
                    }    
                    else
                    {
                        playerTurn.Color = PieceColor.White;
                        playerTurn.ImagePath = Paths.whitePiece;
                        turn.ImagePath = playerTurn.ImagePath;
                        turn.Color = playerTurn.Color;
                    }

                    text=reader.ReadLine();
                    gameServices.RedPieces=int.Parse(text);
                    text=reader.ReadLine();
                    gameServices.WhitePieces=int.Parse(text);

                    for(int i=0;i<boardSize;i++)
                    {
                        for (int j=0;j<boardSize;j++)
                        {
                            text = reader.ReadLine();
                            string[] splitted = text.Split(',');
                            if(splitted.Length==4)
                            {
                                if (splitted[3] == "null")
                                {
                                    board[i][j].Position.x= i;
                                    board[i][j].Position.y= j;
                                    board[i][j].BackgroundCell = GetPathBackground(splitted[2]);
                                    board[i][j].Piece = null;
                                }
                            }
                            else
                            {
                                Piece piece = new Piece(GetType(splitted[3]), GetColor(splitted[4]), GetPathPiece(splitted[3], splitted[4]));
                                board[i][j].Position.x = i;
                                board[i][j].Position.y = j;
                                board[i][j].BackgroundCell = GetPathBackground(splitted[2]);
                                board[i][j].Piece = piece;
                            }
                        }

                    }
                }
            }
         
        }

    }
}
