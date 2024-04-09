using Checkers.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Checkers.Models
{
    class Cell : BaseNotification
    {
        private Position position;
        private string backgroundCell;
        private Piece piece;
        private string isAvailable;
     

        public Cell(int row, int column, string image, Piece piece)
        {
            this.position = new Position(row, column);
            this.backgroundCell = image;
            this.piece = piece;
        }
        public Position Position
        {
            get { return position; }
            set
            {
                position = value;
                NotifyPropertyChanged("Position");
            }
        }

        public string IsAvailable
        {
            get { return isAvailable; }
            set { isAvailable = value;
                NotifyPropertyChanged();
            }

        }

        public string BackgroundCell
        {
            get { return backgroundCell; }
            set
            {
                backgroundCell = value;
                NotifyPropertyChanged("BackgroundCell");
            }
        }

        public Piece Piece
        {
            get { return piece; }
            set
            {
                piece = value;
                NotifyPropertyChanged("Piece");
            }
        }


    }
}
