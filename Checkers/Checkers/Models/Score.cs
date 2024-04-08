using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.Models
{
    class Score:BaseNotification
    {
        private int winnersRed;
        private int winnersWhite;

        public Score(int red, int white )        {
            this.winnersRed =red;
            this.winnersWhite = white;
        }

        public int RedWinner
        {
            get { return this.winnersRed; }
            set { this.winnersRed = value;
                NotifyPropertyChanged("RedWinner");
            }
        }

        public int WhiteWinner
        {
            get { return this.winnersWhite; }
            set { this.winnersWhite=value;
                NotifyPropertyChanged("WhiteWinner");
            }
        }

    }
}
