using Checkers.Models;
using Checkers.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace CheckersGame_.Services
{
    class GameLogic : BaseNotification
    {
        private ObservableCollection<ObservableCollection<Cell>> board;
        private Player playerTurn;
        public GameLogic(ObservableCollection<ObservableCollection<Cell>> cells, Player turn)
        {
            this.board = cells;
            this.playerTurn = turn;

        }

        public Player PlayerTurn
        {
            get { return playerTurn; }
            set
            {
                this.PlayerTurn = value;
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


        private void MoveOverPiece(Cell destination)
        {
            if (Helper.CurrentCell.Position.x + 2 == destination.Position.x && Helper.CurrentCell.Position.y+2 == destination.Position.y)
                board[Helper.CurrentCell.Position.x + 1][Helper.CurrentCell.Position.y + 1].Piece = null;
            else if(Helper.CurrentCell.Position.x + 2 == destination.Position.x && Helper.CurrentCell.Position.y-2 == destination.Position.y)
                board[Helper.CurrentCell.Position.x + 1][Helper.CurrentCell.Position.y - 1].Piece = null;
            else if (Helper.CurrentCell.Position.x - 2 == destination.Position.x && Helper.CurrentCell.Position.y - 2 == destination.Position.y)
                board[Helper.CurrentCell.Position.x -1][Helper.CurrentCell.Position.y - 1].Piece = null;
            else if (Helper.CurrentCell.Position.x - 2 == destination.Position.x && Helper.CurrentCell.Position.y + 2 == destination.Position.y)
                board[Helper.CurrentCell.Position.x - 1][Helper.CurrentCell.Position.y + 1].Piece = null;

        }
        private static void SetKingPiece(Cell cell)
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
            else
            {
                cell.Piece = Helper.CurrentCell.Piece;
            }
        }

        public void ClickAction(Cell cell)
        {
            if (cell != Helper.CurrentCell && cell.Piece != null && cell.Piece.ColorPiece == playerTurn.PlayerColor)
            {
                Helper.CurrentCell = cell;
                SearchPosibleMoves(cell);
                SwitchPlayer(cell);

            }
            else if (cell.Piece == null)
            {
                if (MovePiece(cell))
                {
                    cell.Piece = Helper.CurrentCell.Piece;
                    MoveOverPiece(cell);
                    SetKingPiece(cell);
                    Helper.CurrentCell.Piece = null;
                    Helper.CurrentNeighbourns.Clear();
                }

            }

        }

        public void ResetGame()
        {
            Helper.ResetGame(board);
        }
    }
}
