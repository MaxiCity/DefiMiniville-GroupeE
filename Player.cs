using System;
using System.Collections.Generic;
using System.Linq;

namespace Miniville
{
    public class Player
    {
        #region Private
        private string playerName;
        private int pieces;
        private List<Card> city = new();
        #endregion

        #region Constructor Card class
        private Card card = new();
        #endregion
        
        #region Player Methods

        public void AddCard(Card _carte)
        {
            city.Add(_carte);
        }

        public void UpdateMoney(int earning)
        {
            pieces += card.moneyToEarn;
        }

        public int UseCards(bool doPlay, int dieResult)
        {
            int reward = 0;
            foreach (Card card in city)
            {
                if (card.dieCondition == dieResult)
                {
                    switch (card.color)
                    {
                        case ConsoleColor.Cyan :
                            reward += card.moneyToEarn;
                            break;
                        case ConsoleColor.Green :
                            if (doPlay)
                            {
                                reward += card.moneyToEarn;
                            }
                            break;
                        case ConsoleColor.Red:
                            if (!doPlay)
                            {
                                reward += card.moneyToEarn;
                            }
                            break;
                    }
                }
            }
            return reward;
        }
        
        #endregion

    }
}