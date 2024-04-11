using Checkers.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.Models
{
    class Player:BaseNotification
    {
        private PieceColor color;
        private string imagePath;

        public Player(PieceColor color)
        {
            this.color = color;
            if (color == PieceColor.White)
                imagePath = Paths.whitePiece;
            else
                imagePath = Paths.redPiece;
        }

        public PieceColor PlayerColor
        {
            get { return color; }
            set
            {
                color = value;
                NotifyPropertyChanged("PlayerColor");
            }
        }

        public string ImagePath
        {
            get { return imagePath; }
            set
            {
                this.imagePath = value;
                NotifyPropertyChanged("ImagePath");
            }
        }
    }
}
