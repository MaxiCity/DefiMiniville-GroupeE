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
        
        #region Player Methods

        public void AddCard(Card _carte)
        {
            city.Add(_carte);
        }

        private void UpdateMoney(int earning)
        {
            pieces += earning;
        }

        public int UseCards(bool doPlay, int dieResult)
        {
            int steal = 0;
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
                                steal += card.moneyToEarn;
                            }
                            break;
                    }
                }
            }
            UpdateMoney(reward);
            return steal;
        }
        
        #endregion

    }
}