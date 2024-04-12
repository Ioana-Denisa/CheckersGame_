using Checkers.Models;
using Checkers.Services;
using CheckersGame_.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CheckersGame_.Services
{
    class GameLogic:BaseNotification
    {
        private ObservableCollection<ObservableCollection<Cell>> board;
        private Player playerTurn;
        private GameServices gameServices;
        private Score score;
        private bool extraJump ;
        private bool multipleJump;
        private static int MAX_PIECE_RED = 0;
        private static int MAX_PIECE_WHITE = 0;

        public GameLogic(ObservableCollection<ObservableCollection<Cell>> cells, Player turn, GameServices game)
        {
            this.board = cells;
            this.playerTurn = turn;
            this.gameServices = game;
            this.score = Helper.GetScore();
            extraJump = false;
            SetMaxPieces();
            Statistics();
        }

        public Player PlayerTurn
        {
            get { return playerTurn; }
            set
            {
                this.PlayerTurn = value;
            }
        }

        public bool MultipleJump
        {
            get { return multipleJump; }
            set
            {
                multipleJump = value;
                NotifyPropertyChanged("MultipleJump");
            }
        }

        public Score Score
        {
            get { return score; }
            set
            {
                this.score = value;
            }
        }



        private bool VerifyCoordinateFirstCell(Cell cell, Position position)
        {
            if (cell.Position.x + position.x >= 0 && cell.Position.x + position.x < 8 && cell.Position.y + position.y >= 0 && cell.Position.y + position.y < 8)
                return true;
            return false;
        }

        private bool VerifyCoordinateSecondCell(Cell cell, Position position)
        {
            if (cell.Position.x + position.x * 2 >= 0 && cell.Position.x + position.x * 2 < 8 && cell.Position.y + position.y * 2 >= 0 && cell.Position.y + position.y * 2 < 8)
                return true;
            return false;
        }


        private bool VerifyNoPieceAtTheFirstCell(Cell cell, Position position)
        {
            if (board[cell.Position.x + position.x][cell.Position.y + position.y].Piece == null)
                return true;
            return false;
        }

        private bool VerifyNoPieceAtTheSecondCell(Cell cell, Position position)
        {
            if (board[cell.Position.x + position.x * 2][cell.Position.y + position.y * 2].Piece == null)
                return true;
            return false;
        }

        private void SearchPosibleMoves(Cell cell)
        {
            List<Position> neighboursCell = new List<Position>();
            Helper.SearchAllNeighboursForCell(cell, neighboursCell);

            foreach (Position position in neighboursCell)
            {
                if (VerifyCoordinateFirstCell(cell, position) && VerifyNoPieceAtTheFirstCell(cell, position))
                {
                    Helper.currentNeighbours.Add(board[cell.Position.x + position.x][cell.Position.y + position.y]);
                }
                else if (VerifyCoordinateSecondCell(cell, position) && board[cell.Position.x + position.x][cell.Position.y + position.y].Piece.ColorPiece != cell.Piece.ColorPiece)
                {
                    if (VerifyNoPieceAtTheSecondCell(cell, position))
                    {
                        Helper.currentNeighbours.Add(board[cell.Position.x + 2 * position.x][cell.Position.y + 2 * position.y]);
                    }
                }
            }
        }

        public void SearchMultipleJump(Cell cell)
        {
            Helper.currentNeighbours.Clear();
            List<Position> neighboursCell = new List<Position>();
            Helper.SearchAllNeighboursForCell(cell, neighboursCell);

            foreach (Position position in neighboursCell)
            {
                if (VerifyCoordinateSecondCell(cell, position) && board[cell.Position.x + position.x][cell.Position.y + position.y].Piece != null && board[cell.Position.x + position.x][cell.Position.y + position.y].Piece.ColorPiece != cell.Piece.ColorPiece)
                {
                    if (VerifyNoPieceAtTheSecondCell(cell, position))
                    {
                        Helper.currentNeighbours.Add(board[cell.Position.x + 2 * position.x][cell.Position.y + 2 * position.y]);
                    }
                }
            }
        }

        public bool VerifyMultipleJump(Cell cell)
        {
            SearchMultipleJump(cell);

            return Helper.currentNeighbours.Count != 0;
        }


        private void SwitchPlayer(Cell cell)
        {
            if (cell.Piece.ColorPiece == PieceColor.White)
            {
                Helper.PlayerTurn.PlayerColor = PieceColor.Red;
                Helper.PlayerTurn.ImagePath = Paths.redPiece;
                playerTurn.PlayerColor = PieceColor.Red;
                playerTurn.ImagePath = Paths.redPiece;

            }
            else
            {
                Helper.PlayerTurn.PlayerColor = PieceColor.White;
                Helper.PlayerTurn.ImagePath = Paths.whitePiece;
                playerTurn.PlayerColor = PieceColor.White;
                playerTurn.ImagePath = Paths.whitePiece;
            }
            Helper.currentNeighbours.Clear();
        }

        private bool MovePiece(Cell destination)
        {
            foreach (Cell selected in Helper.CurrentNeighbourns)
            {
                if (destination.Position == selected.Position)
                {
                    return true;
                }
            }
            return false;
        }

        private void DropPieces()
        {
            if (Helper.CurrentCell.Piece.ColorPiece == PieceColor.Red)
            {
                gameServices.WhitePieces--;
            }
            else
            {
                gameServices.RedPieces--;
            }
        }


        public void WriteScore()
        {
            score = Helper.GetScore();

            if (gameServices.RedPieces == 0 || gameServices.RedPieces < gameServices.WhitePieces)
            {
                score.WhiteWinner = score.WhiteWinner + 1;
                Helper.WriteScore(score.RedWinner, score.WhiteWinner);
            }
            else if (gameServices.WhitePieces == 0 || gameServices.WhitePieces < gameServices.RedPieces)
            {
                score.RedWinner = score.RedWinner + 1;
                Helper.WriteScore(score.RedWinner, score.WhiteWinner);
            }

            if (gameServices.RedPieces > MAX_PIECE_RED)
                MAX_PIECE_RED = gameServices.RedPieces;
            if (gameServices.WhitePieces > MAX_PIECE_WHITE)
                MAX_PIECE_WHITE = gameServices.WhitePieces;

            if (gameServices.WhitePieces > gameServices.RedPieces)
                MessageBox.Show("Player white win!");
            else
                MessageBox.Show("Player red win!");

            Statistics();
            WriteMaxPieces();

        }
        private void MoveOverPiece(Cell destination)
        {

            if (Helper.CurrentCell.Position.x + 2 == destination.Position.x && Helper.CurrentCell.Position.y + 2 == destination.Position.y)
            {
                extraJump = true;
                DropPieces();
                board[Helper.CurrentCell.Position.x + 1][Helper.CurrentCell.Position.y + 1].Piece = null;
            }
            else if (Helper.CurrentCell.Position.x + 2 == destination.Position.x && Helper.CurrentCell.Position.y - 2 == destination.Position.y)
            {
                extraJump = true;
                DropPieces();
                board[Helper.CurrentCell.Position.x + 1][Helper.CurrentCell.Position.y - 1].Piece = null;
            }
            else if (Helper.CurrentCell.Position.x - 2 == destination.Position.x && Helper.CurrentCell.Position.y - 2 == destination.Position.y)
            {
                extraJump = true;
                DropPieces();
                board[Helper.CurrentCell.Position.x - 1][Helper.CurrentCell.Position.y - 1].Piece = null;
            }
            else if (Helper.CurrentCell.Position.x - 2 == destination.Position.x && Helper.CurrentCell.Position.y + 2 == destination.Position.y)
            {
                extraJump = true;
                DropPieces();
                board[Helper.CurrentCell.Position.x - 1][Helper.CurrentCell.Position.y + 1].Piece = null;
            }
            if (!multipleJump || !VerifyMultipleJump(destination) )
                extraJump = false;

        }



        private static void SetKingPiece(Cell cell)
        {
            if (cell.Piece.TypePiece != PieceType.King)
            {
                if (cell.Position.x == 0 && cell.Piece.ColorPiece == PieceColor.Red)
                {
                    cell.Piece.TypePiece = PieceType.King;
                    cell.Piece.ImagePath = Paths.redKingPiece;
                }
                else if (cell.Position.x == 7 && cell.Piece.ColorPiece == PieceColor.White)
                {
                    cell.Piece.TypePiece = PieceType.King;
                    cell.Piece.ImagePath = Paths.whiteKingPiece;
                }
            }

        }

        public void ClickAction(Cell cell)
        {
            if (cell != Helper.CurrentCell && cell.Piece != null && cell.Piece.ColorPiece == playerTurn.PlayerColor && !extraJump)
            {
                Helper.CurrentCell = cell;
                SearchPosibleMoves(cell);
            }
            else if (cell.Piece == null)
            {
                if (MovePiece(cell))
                {
                    cell.Piece = Helper.CurrentCell.Piece;
                    MoveOverPiece(cell);
                    SetKingPiece(cell);
                    Helper.CurrentCell.Piece = null;


                    if (!extraJump)
                    {
                        SwitchPlayer(cell);
                    }
                    else
                    {
                        Helper.CurrentNeighbourns.Clear();
                        Helper.CurrentCell = cell;
                        SearchMultipleJump(cell);
                    }


                    if (gameServices.RedPieces == 0 || gameServices.WhitePieces == 0)
                    {
                        WriteScore();
                        ResetGame();
                    }
                }

            }
        }


        public void ResetGame()
        {
            playerTurn.PlayerColor = PieceColor.Red;
            playerTurn.ImagePath = Paths.redPiece;
            score = Helper.GetScore();
            Helper.ResetGame(board, gameServices);
        }

        public void Open()
        {
            Helper.OpenGame(board, gameServices, playerTurn);
        }


        public void SaveGame()
        {
            Helper.SaveGame(board, gameServices);
        }

        public void About()
        {
            Helper.About();
        }

        public void Statistics()
        {
            using (StreamWriter writer = new StreamWriter(Paths.statisticsFile, false))
            {
                writer.WriteLine("Score: Red " + score.RedWinner + " White: " + score.WhiteWinner);
                writer.WriteLine("Maximum number of white pieces: " + MAX_PIECE_WHITE);
                writer.WriteLine("Maximum number of red pieces: " + MAX_PIECE_RED);
            }
        }

        public void ShowStatistics()
        {
            Statistics();

            using (var reader = new StreamReader(Paths.statisticsFile))
            {
                MessageBox.Show(reader.ReadToEnd(), "Statistics", MessageBoxButton.OK);
            }
        }


        public void SetMaxPieces()
        {
            using (var reader = new StreamReader(Paths.maxPiecesFile))
            {
                string line = reader.ReadLine();
                string[] pieces = line.Split(' ');
                MAX_PIECE_RED = int.Parse(pieces[0]);
                MAX_PIECE_WHITE = int.Parse(pieces[1]);
            }
        }

        public void WriteMaxPieces()
        {
            using (var writer = new StreamWriter(Paths.maxPiecesFile))
            {
                writer.WriteLine(MAX_PIECE_RED + " " + MAX_PIECE_WHITE);
            }
        }
    }
}
