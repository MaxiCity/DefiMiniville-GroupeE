using System.Collections.Generic;

namespace Miniville
{
    public class IASafe : IA
    {
        private bool expert;
        private int moneyToWin;
        public IASafe(Player _player, int _mode) : base(_player)
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
            
            int[] cover = CoveredDiceRoll();
            for(int i =0; i < cover.Length; i++)
            {
                //Pour les index 0 à 4, renforcer le score de choix pour un dé, sinon pour 2
                if (i < 5) 
                    oneDiceScore+=cover[i];
                else 
                    twoDiceScore+=cover[i];

            }
            int nbDice = oneDiceScore > twoDiceScore ? 1 : 2;
            return nbDice;
        }

        protected override Pile Choose(List<Pile> _possiblePiles)
        {
            foreach (Pile pile in _possiblePiles)
            {
                //Si on n'a pas la carte et qu'on est en expert, l'acheter
                if (!player.city.Contains(pile.card) && expert)
                    return pile;
                
                //Sinon, si on a beaucoup d'argent, économiser
                else if (player.pieces >= moneyToWin*0.65f)
                    return null;
                
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