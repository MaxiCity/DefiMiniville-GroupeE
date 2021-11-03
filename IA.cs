using System;
using System.Collections.Generic;
using System.Linq;

namespace Miniville
{
    /** Classe qui gère les comportements et les choix du joueur IA. */
    public abstract class IA
    {
        /// <summary> La référence aux informations de player de l'IA. </summary>
        public Player player;
        protected Random random = new Random();

        /// <summary>  Le constructeur de l'IA. </summary>
        /// <param name="_player"> Le joueur que va contrôler l'IA. </param>
        public IA(Player _player)
        {
            player = _player;
        }

        /// <summary> Représente le choix de l'IA de lancer 1 ou 2 dés. </summary>
        /// <returns> Le nombre de dé que l'IA va lancer. </returns>
        public abstract int IANbDice();

        /// <summary> Représente la pioche et l'achat des cartes. </summary>
        /// <param name="_piles"> La liste des piles de cartes. </param>
        /// <returns> La carte qui a été piochée. </returns>
        public Card IAPlay(Pile[] _piles)
        {
            //Récupération de la liste des piles possibles
            List<Pile> possiblePiles = SelectPossiblePiles(_piles);
            
            //S'il y a au moins une pile disponible...
            if (possiblePiles.Count > 0)
            {
                //Choisir une pile
                Pile choosenPile = Choose(possiblePiles);
                
                //Si la pile n'est pas null...
                if (choosenPile != null)
                {
                    //Piocher la carte
                    Card choosenCard = choosenPile.Draw();

                    //L'ajouter au joueur IA
                    player.AddCard(choosenCard);
                    
                    //Ajuster son argent
                    player.UpdateMoney(-choosenCard.cost);
                    
                    return choosenCard;
                }
            }
            //Sinon, renvoyer null
            return null;
        }

        /// <summary> Représente le choix de l'IA parmi les piles disponibles. </summary>
        /// <param name="_possiblePiles"> La liste des piles dans lesquelles l'IA peut piocher. </param>
        /// <returns> La pile choisie par l'IA pour piocher. </returns>
        protected abstract Pile Choose(List<Pile> _possiblePiles);

    

        /// <summary> Permet de savoir quelles piles sont actuellement disponible à la pioche pour l'IA. </summary>
        /// <param name="_piles"> Toutes les piles de cartes. </param>
        /// <returns> une liste des piles disponibles. </returns>
        protected List<Pile> SelectPossiblePiles(Pile[] _piles)
        {
            List<Pile> possiblePile = new List<Pile>();
            
            //On parcours les piles du tableau...
            foreach (Pile pile in _piles)
            {
                //Si la pile n'est pas vide et que le prix de la carte est inférieur à l'argent du joueur IA...
                if (pile.nbCard != 0 && pile.card.cost <= player.pieces)
                {
                    //Ajouter la pile aux choix possibles
                    possiblePile.Add(pile);
                }
            }
            return possiblePile;
        }

        /// <summary> Permet de savoir quelles coûts d'activations sont présents sur les cartes de l'IA. </summary>
        /// <returns> Un tableau contenant tous les coûts des cartes de l'IA. </returns>
        protected int[] CoveredDiceRoll()
        {
            int[] coveredDiceRoll = new int[12];
            foreach (Card card in player.city)
            {
                foreach (int i in card.dieCondition)
                {
                    coveredDiceRoll[i - 1]++;
                }
            }
            return coveredDiceRoll;
        }
    }
}