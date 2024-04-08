using Checkers.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Checkers.Services
{
    class GameBusinessLogic
    {
        private ObservableCollection<ObservableCollection<Cell>> board;
        private Player playerTurn;
        private Score score;
        public GameBusinessLogic(ObservableCollection<ObservableCollection<Cell>> cells, Player player, Score winner)
        {
            this.board = cells;
            this.playerTurn = player;
            this.score = winner;
        }

        public GameBusinessLogic(ObservableCollection<ObservableCollection<Cell>> cells)
        {
            this.board = cells;

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
        }

        private bool VerifyCoordinateFirstCell(Cell cell, Position position)
        {
            if (cell.Position.x + position.x > 0 && cell.Position.x + position.x < 8 && cell.Position.y + position.y > 0 && cell.Position.y + position.y < 8)
                return true;
            return false;
        }

        private bool VerifyCoordinateSecondCell(Cell cell, Position position)
        {
            if (cell.Position.x + position.x * 2 > 0 && cell.Position.x + position.x * 2 < 8 && cell.Position.y + position.y * 2 > 0 && cell.Position.y + position.y * 2 < 8)
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


        private void FindPosibleMoves(Cell cell)
        {
            List<Position> neighboursCell = new List<Position>();
            Helper.AllNeighboursCell(cell, neighboursCell);
            foreach (Position position in neighboursCell)
            {
                if (VerifyCoordinateFirstCell(cell, position))
                {
                    if (VerifyNoPieceAtTheFirstCell(cell, position))
                        Helper.CurrentNeighboursCell.Add(board[cell.Position.x + position.x][cell.Position.y + position.y], null);
                }
                else if (VerifyCoordinateSecondCell(cell, position) && board[cell.Position.x + position.x][cell.Position.y + position.y].Piece.ColorPiece != cell.Piece.ColorPiece)
                {
                    if (VerifyNoPieceAtTheSecondCell(cell, position))
                        Helper.CurrentNeighboursCell.Add(board[cell.Position.x + position.x * 2][cell.Position.y + position.y * 2], board[cell.Position.x + position.x][cell.Position.y + position.y]);
                }
            }
        }

        private void ShowMoves(Cell cell)
        {
            if (Helper.currentCell != cell)
            {
                if (Helper.currentCell != null)
                {

                    foreach (Cell selected in Helper.CurrentNeighboursCell.Keys)
                    {
                        selected.BackgroundCell = Paths.backgroundDark;
                    }
                    Helper.currentNeighboursCell.Clear();
                }

                FindPosibleMoves(cell);

                if (Helper.MultipleJumps)
                {
                    Helper.MultipleJumps = false;
                    SwitchPlayer(cell);
                }
                else
                {
                    foreach (Cell value in Helper.CurrentNeighboursCell.Keys)
                    {
                        board[value.Position.x][value.Position.y].BackgroundCell = Paths.availableMoves;
                    }

                    Helper.currentCell = cell;
                }
            }
            else
            {
                foreach (Cell selected in Helper.CurrentNeighboursCell.Keys)
                {
                    selected.BackgroundCell = Paths.backgroundDark;
                }
                Helper.currentNeighboursCell.Clear();
                Helper.currentCell = null;
            }
        }

        public void SelectPiece(Cell cell)
        {
            if (Helper.PlayerTurn.PlayerColor == PieceColor.Red && cell.Piece.ColorPiece == PieceColor.Red || Helper.PlayerTurn.PlayerColor == PieceColor.White && cell.Piece.ColorPiece == PieceColor.White)
            {
                ShowMoves(cell);
            }
        }

        public void MovePiece(Cell cellDestination)
        {
            cellDestination.Piece = Helper.currentCell.Piece;
            board[cellDestination.Position.x][cellDestination.Position.y].Piece =Helper.currentCell.Piece;
            

            if (Helper.CurrentNeighboursCell[cellDestination] != null)
            {
                Helper.CurrentNeighboursCell[cellDestination].Piece = null;
                Helper.MultipleJumps = true;
            }
            else
            {
                Helper.MultipleJumps = false;
                SwitchPlayer(Helper.currentCell);
            }

            foreach (Cell selected in Helper.CurrentNeighboursCell.Keys)
            {
                selected.BackgroundCell = Paths.backgroundDark;
            }
            Helper.CurrentNeighboursCell.Clear();
            Helper.currentCell.Piece = null;
            Helper.currentCell = null;

            if (cellDestination.Piece.TypePiece == PieceType.Regular)
            {
                if (cellDestination.Position.x == Helper.boardSize - 1 && cellDestination.Piece.ColorPiece == PieceColor.White)
                {
                    cellDestination.Piece.TypePiece = PieceType.King;
                    cellDestination.Piece.ImagePath = Paths.whiteKingPiece;
                }
                else if (cellDestination.Position.x == 0 && cellDestination.Piece.ColorPiece == PieceColor.Red)
                {
                    cellDestination.Piece.TypePiece = PieceType.King;
                    cellDestination.Piece.ImagePath = Paths.redKingPiece;
                }
            }

            if (Helper.MultipleJumps)
            {
                if (playerTurn.ImagePath == Paths.whitePiece)
                {
                    Helper.RestOfTheWhitePiece--;
                }
                if (playerTurn.ImagePath == Paths.redPiece)
                {
                    Helper.RestOfTheRedPiece--;
                }

                ShowMoves(cellDestination);
            }
            if (Helper.RestOfTheRedPiece == 0 || Helper.RestOfTheWhitePiece == 0)
            {

            }
        }


        public void WriteScore()
        {
            Score score = Helper.GetScore();
            if (Helper.RestOfTheRedPiece == 0 || Helper.RestOfTheRedPiece < Helper.RestOfTheWhitePiece)
            {
                Helper.WriteScore(score.RedWinner, ++score.WhiteWinner);
            }
            if (Helper.RestOfTheWhitePiece == 0 || Helper.RestOfTheWhitePiece < Helper.RestOfTheRedPiece)
            {
                Helper.WriteScore(++score.RedWinner, score.WhiteWinner);
            }

            if (Helper.RestOfTheWhitePiece > Helper.RestOfTheRedPiece)
                MessageBox.Show("Player white win!");
            else
                MessageBox.Show("Player red win!");
            score.RedWinner = score.RedWinner;
            score.WhiteWinner = score.WhiteWinner;
        }
        public void EndGame()
        {
            WriteScore();
            Helper.RestOfTheRedPiece = 16;
            Helper.RestOfTheWhitePiece = 16;
            Helper.ResetGame(board);
        }
        public void About()
        {
            Helper.About();
        }

        public void SaveGame()
        {
            Helper.SaveGame(board);
        }

        public void LoadGame()
        {
            Helper.LoadGame(board);
            playerTurn.ImagePath = Helper.PlayerTurn.ImagePath;
        }

        public void ResetGame()
        {
            Helper.ResetGame(board);
        }
    }
}

