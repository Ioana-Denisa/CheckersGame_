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
        private PieceService pieceServices;
        private Score score;
        private bool extraJump;
        private bool multipleJump;
        private static int MAX_PIECE_RED = 0;
        private static int MAX_PIECE_WHITE = 0;

        public GameLogic(ObservableCollection<ObservableCollection<Cell>> cells, Player turn, PieceService pieceService)
        {
            this.board = cells;
            this.playerTurn = turn;
            this.pieceServices = pieceService;
            this.score = Helper.GetScore();
            extraJump = false;
            multipleJump = false;
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
                Helper.PlayerTurn.Color = PieceColor.Red;
                Helper.PlayerTurn.ImagePath = Paths.redPiece;
                playerTurn.Color = PieceColor.Red;
                playerTurn.ImagePath = Paths.redPiece;

            }
            else
            {
                Helper.PlayerTurn.Color = PieceColor.White;
                Helper.PlayerTurn.ImagePath = Paths.whitePiece;
                playerTurn.Color = PieceColor.White;
                playerTurn.ImagePath = Paths.whitePiece;
            }
            Helper.currentNeighbours.Clear();
        }

        private bool CanMovePiece(Cell destination)
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

        private void DropNumberOfPieces()
        {
            if (Helper.CurrentCell.Piece.ColorPiece == PieceColor.Red)
            {
                pieceServices.WhitePieces--;
            }
            else
            {
                pieceServices.RedPieces--;
            }
        }


        public void WriteScore()
        {
            score = Helper.GetScore();

            if (pieceServices.RedPieces == 0 || pieceServices.RedPieces < pieceServices.WhitePieces)
            {
                score.WhiteWinner = score.WhiteWinner + 1;
                Helper.WriteScore(score.RedWinner, score.WhiteWinner);
            }
            else if (pieceServices.WhitePieces == 0 || pieceServices.WhitePieces < pieceServices.RedPieces)
            {
                score.RedWinner = score.RedWinner + 1;
                Helper.WriteScore(score.RedWinner, score.WhiteWinner);
            }

            if (pieceServices.RedPieces > MAX_PIECE_RED)
                MAX_PIECE_RED = pieceServices.RedPieces;
            if (pieceServices.WhitePieces > MAX_PIECE_WHITE)
                MAX_PIECE_WHITE = pieceServices.WhitePieces;

            if (pieceServices.WhitePieces > pieceServices.RedPieces)
                MessageBox.Show("End game. \U0001F632\nCongratulations WHITE player, you won!\U0001F44F\U0001F60A");
            else
                MessageBox.Show("End game. \U0001F632\nCongratulations RED player, you won! \U0001F44F\U0001F60A");

            Statistics();
            WriteMaxPieces();

        }
        private void MoveOverPiece(Cell destination)
        {

            if (Helper.CurrentCell.Position.x + 2 == destination.Position.x && Helper.CurrentCell.Position.y + 2 == destination.Position.y)
            {
                extraJump = true;
                DropNumberOfPieces();
                board[Helper.CurrentCell.Position.x + 1][Helper.CurrentCell.Position.y + 1].Piece = null;
            }
            else if (Helper.CurrentCell.Position.x + 2 == destination.Position.x && Helper.CurrentCell.Position.y - 2 == destination.Position.y)
            {
                extraJump = true;
                DropNumberOfPieces();
                board[Helper.CurrentCell.Position.x + 1][Helper.CurrentCell.Position.y - 1].Piece = null;
            }
            else if (Helper.CurrentCell.Position.x - 2 == destination.Position.x && Helper.CurrentCell.Position.y - 2 == destination.Position.y)
            {
                extraJump = true;
                DropNumberOfPieces();
                board[Helper.CurrentCell.Position.x - 1][Helper.CurrentCell.Position.y - 1].Piece = null;
            }
            else if (Helper.CurrentCell.Position.x - 2 == destination.Position.x && Helper.CurrentCell.Position.y + 2 == destination.Position.y)
            {
                extraJump = true;
                DropNumberOfPieces();
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
            if (cell != Helper.CurrentCell && cell.Piece != null && cell.Piece.ColorPiece == playerTurn.Color && !extraJump)
            {
                Helper.CurrentCell = cell;
                SearchPosibleMoves(cell);
            }
            else if (cell.Piece == null)
            {
                if (CanMovePiece(cell))
                {
                    cell.Piece = Helper.CurrentCell.Piece;
                    SetKingPiece(cell);
                    MoveOverPiece(cell);
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


                    if (pieceServices.RedPieces == 0 || pieceServices.WhitePieces == 0)
                    {
                        WriteScore();
                        ResetGame();
                    }
                }

            }
        }


        public void ResetGame()
        {
            playerTurn.Color = PieceColor.Red;
            playerTurn.ImagePath = Paths.redPiece;
            score = Helper.GetScore();
            Helper.ResetGame(board, pieceServices);
        }



        public void SaveGame()
        {
            Helper.SaveGame(board, pieceServices, multipleJump);
        }
        public void Open()
        {
            Helper.OpenGame(board, pieceServices, playerTurn);
            MultipleJump = Helper.multiple;
   
        }

        public void About()
        {
            Helper.About();
        }

        public void Statistics()
        {
            using (StreamWriter writer = new StreamWriter(Paths.statisticsFile, false))
            {
                writer.WriteLine("Score:\nRed " + score.RedWinner + "  VS  White: " + score.WhiteWinner);
                writer.WriteLine("Maximum number of white pieces: " + MAX_PIECE_WHITE+ ". 😊");
                writer.WriteLine("Maximum number of red pieces: " + MAX_PIECE_RED+ ". 😊");
            }
        }

        public void ShowStatistics()
        {
            Statistics();

            using (var reader = new StreamReader(Paths.statisticsFile))
            {
                MessageBox.Show(reader.ReadToEnd(), "Statistics 😊", MessageBoxButton.OK);
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
