using Checkers.Models;
using Checkers.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.ViewModels
{
    class ScoreVM:BaseNotification
    {
        private GameBusinessLogic gameBusinessLogic;
        private Score score;

        public ScoreVM(GameBusinessLogic gameBusinessLogic, Score winners)
        {
            this.gameBusinessLogic = gameBusinessLogic;
            this.score = winners;
        }

        public GameBusinessLogic GameBusinessLogic
        {
            get { return gameBusinessLogic; }
            set
            {
                gameBusinessLogic = value;
                NotifyPropertyChanged("GameBussinessLogic");
            }
        }

        public Score Score
        {
            get { return score; }
            set
            {
                score = value;
                NotifyPropertyChanged("Score");
            }
        }
    }
}
