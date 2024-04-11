using Checkers.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.Models
{
    class Piece : BaseNotification
    {
        private PieceType typePice;
        private PieceColor colorPiece;
        private string imagePath;

        public Piece(PieceType typePice, PieceColor colorPiece, string imagePath)
        {
            this.typePice = typePice;
            this.colorPiece = colorPiece;
            this.imagePath = imagePath;
        }

        public Piece()
        {
            this.typePice = PieceType.None;
            this.colorPiece = PieceColor.None;
            this.imagePath = "";
        }

   
        public PieceType TypePiece
        {
            get { return typePice; }
            set
            {
                this.typePice = value;
                NotifyPropertyChanged("PieceType");
            }
        }

        public PieceColor ColorPiece
        {
            get { return colorPiece; }
            set
            {
                colorPiece = value;
                NotifyPropertyChanged("ColorPiece");
            }
        }

        public string ImagePath
        {
            get { return imagePath; }
            set
            {
                imagePath = value;
                NotifyPropertyChanged("ImagePath");
            }
        }
    }
}
