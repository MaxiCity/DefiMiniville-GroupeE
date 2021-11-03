using System.Collections.Generic;
namespace Miniville
{
    public class IASafe : IA
    {
        private bool expert;
        public IASafe(Player _player, bool _expert) : base(_player)
        {
            expert = _expert;
        }

        public new int IANbDice()
        {
            int oneDiceScore = 0;
            int twoDiceScore = 0;
            int nbDice;
            foreach (Card card in player.city)
            { 
                foreach (int i in card.dieCondition)
                {
                    //Si inférieur à , renforcer le score de choix pour un dé, sinon pour 2
                    if (i < 7)
                        oneDiceScore++;
                    else
                        twoDiceScore++;
                }
            }
            nbDice = oneDiceScore > twoDiceScore ? 1 : 2;
            return nbDice;
        }

        protected new Pile Choose(List<Pile> _possiblePiles)
        {
            //Si on a plus de 14 pièces, économiser
            if (player.pieces > 14)
                return null;

            foreach (Pile pile in _possiblePiles)
            {
                foreach (int i in pile.card.dieCondition)
                {
                    //Si on ne couvre pas le lancé de dé, choisir cette pile
                    if (CoveredDiceRoll()[i-1] == 0)
                    {
                        return pile;
                    }
                }
            }
            return null;
        }
    }
}