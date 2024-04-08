using Checkers.Models;
using Checkers.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.ViewModels
{
    class PlayerVM:BaseNotification
    {
        private GameBusinessLogic gameBusinessLogic;
        private Player player;

        public PlayerVM(GameBusinessLogic gameBusinessLogic, Player player)
        {
            this.gameBusinessLogic = gameBusinessLogic;
            this.player = player;
        }

        public Player Player
        {
            get { return player; }
            set
            {
                player = value;
                NotifyPropertyChanged("Player");
            }
        }

        public GameBusinessLogic GameBussinessLogic
        {
            get { return gameBusinessLogic; }
            set
            {
                gameBusinessLogic = value;
                NotifyPropertyChanged("GameBussinessLogic");
            }
        }
    }
}
