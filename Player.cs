using System;
using System.Collections.Generic;
using System.Linq;

namespace Miniville
{
    public class Player
    {
        #region Private

        /// <summary>
        /// Afficher le nom du joueur 
        /// </summary>
        private string PlayerName;

        /// <summary>
        /// Nombre de pieces du joueur
        /// </summary>
        private int Pieces;

        /// <summary>
        /// Main du joueur (List<Card>)
        /// </summary>
        private List<Card> City = new List<Card>();
        #endregion

        #region Properties
        /// <summary>
        /// Propriété en accès public, modification privée
        /// </summary>
        public string playerName
        {
            get { return playerName;}
            private set {playerName = PlayerName;} 
        }
        
        /// <summary>
        /// Propriété en accès public, modification privée
        /// </summary>
        public int pieces
        {
            get { return pieces; }
            private set { pieces = Pieces; }
        }
        
        /// <summary>
        /// Propriété en accès public, modification privée
        /// </summary>
        public List<Card> city
        {
            get { return city; }
            private set { city = City; }
        }

        #endregion
        
        #region Player Methods

        /// <summary>
        /// Ajoute la carte choisie à la main du joueur (city)
        /// </summary>
        /// <param name="_carte">Instance de la classe Card</param>
        public void AddCard(Card _carte)
        {
            City.Add(_carte);
        }

        /// <summary>
        /// Mise à jour de l'argent du joueur (pieces)
        /// </summary>
        /// <param name="earning">Revenu calculé par la méthode UseCards()</param>
        private void UpdateMoney(int earning)
        {
            Pieces += earning;
        }

        /// <summary>
        /// Vérifie le tour actuelle, la couleur des cartes dans la main du joueur et les effets de ces dernières
        /// </summary>
        /// <param name="doPlay">Tour du joueur ?</param>
        /// <param name="dieResult">Resultat du dé ?</param>
        /// <returns>Le total de pièces que le joueur vole à l'adversaire ce tour-ci (steal)</returns>
        public int UseCards(bool doPlay, int dieResult)
        {
            int steal = 0;
            int reward = 0;
            foreach (Card card in City)
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