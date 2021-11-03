using System;
using System.Collections.Generic;
using System.Linq;

namespace Miniville
{
    public class IAOffensive : IA
    {
        private bool expert;
        private int moneyToWin;
        public IAOffensive(Player _player, int _mode) : base(_player)
        {
            expert = _mode == 3;
            if (expert)
            {
                moneyToWin = 20;
            }
            else
            {
                moneyToWin = (_mode + 1) * 10;
            }
        }

        public override int IANbDice()
        {
            int oneDiceScore = 0;
            int twoDiceScore = 0;

            foreach (Card card in player.city)
            { 
                foreach (int i in card.dieCondition)
                {
                    //Si inférieur à 7, renforcer le score de choix pour un dé selon le gain possible, sinon pour 2
                    if (i < 7)
                        oneDiceScore += card.moneyToEarn;
                    else
                        twoDiceScore += card.moneyToEarn;
                }
            }
            int nbDice = oneDiceScore > twoDiceScore ? 1 : 2;
            return nbDice;
        }

        protected override Pile Choose(List<Pile> _possiblePiles)
        {
            //Si on a plus de 17 pièces, économiser
            if (player.pieces > moneyToWin*0.8f && !expert)
                return null;
            
            foreach (Pile pile in _possiblePiles)
            {
                if (!player.city.Contains(pile.card) && expert)
                    return pile;

                foreach (int i in pile.card.dieCondition)
                {
                    //Si on ne couvre pas le lancé de dé, choisir cette pile
                    //(Retourne directement cette pile)
                    if (CoveredDiceRoll()[i-1] == 0)
                    {
                        return pile;
                    }
                }
            }
            //Une chance sur 2 de ne pas acheter
            if (random.Next(0,2)==0)
            {
                return null;
            }
                    
            //S'il n'y a pas eu de retour précédent, acheter une carte aléatoire
            return _possiblePiles[random.Next(_possiblePiles.Count)];
        }
    }
}