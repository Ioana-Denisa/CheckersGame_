using Checkers.Models;
using CheckersGame_.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersGame_.Services
{
    class GameServices : BaseNotification
    {
        GameVM game;
        private int redPieces;
        private int whitePieces;

        public event EventHandler PiecesChanged;

       public GameServices( GameVM game)
        {
            this.game = game;
            this.redPieces = 12;
            this.whitePieces = 12;
        }

        public int RedPieces
        {
            get { return redPieces; }
            set
            {
                redPieces = value;
                NotifyPropertyChanged(nameof(RedPieces));
            }
        }

        public int WhitePieces
        {
            get { return whitePieces; }
            set
            {
                whitePieces = value;
                NotifyPropertyChanged(nameof(WhitePieces));   
            }
        }
    }
}
