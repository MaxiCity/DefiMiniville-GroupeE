using System;
using System.Collections.Generic;

namespace Miniville
{
    public class IA
    {
        private enum Difficulties
        {
            Random,
            Greedy,
            Buyer,
        }

        private Difficulties difficulty = Difficulties.Random;

        public Player player;
        private Random random = new Random();

        public IA(Player _player)
        {
            player = _player;
        }
        public IA(int _difficulty, Player _player)
        {
            difficulty = (Difficulties)_difficulty;
            player = _player;
        }

        public int IANbDice()
        {
            int nbOneDiceCard = 0;
            int nbTwoDiceCard = 0;

            foreach (Card card in player.city)
            {
                foreach (int i in card.dieCondition)
                {
                    if (i<7)
                    {
                        nbOneDiceCard++;
                    }
                    else
                    {
                        nbTwoDiceCard++;
                    }
                }
            }

            int nbDice = nbOneDiceCard < nbTwoDiceCard ? 2 : 1;
            return nbDice;
        }

        public Card IAPlay(Pile[] _piles)
        {
            // 1/2 chances de ne pas tirer du tout
            if (random.Next(4) != 1)
            {
                //Récupération de la liste des piles possibles
                List<Pile> possiblePiles = SelectPossiblePiles(_piles);
                //S'il y a au moins une pile disponible...
                if (possiblePiles.Count > 0)
                {
                    //Piocher la carte
                    Card choosenCard = Choose(possiblePiles).Draw();
                    
                    //L'ajouter au joueur IA
                    player.AddCard(choosenCard);
                    
                    //Ajuster son argent
                    player.UpdateMoney(-choosenCard.cost);
                    
                    return choosenCard;
                }
            }

                return null;
        }

        public Pile Choose(List<Pile> _possibleIndex)
        {
            Pile choice;

            switch (difficulty)
            {
                default:
                    //choisir une pile aléatoire parmi la liste des choix possibles
                    choice = _possibleIndex[random.Next(_possibleIndex.Count)];
                    break;
                /*case Difficulties.Buyer:
                    choice = Buyer(_possibleIndex);
                    break;
                case Difficulties.Greedy:
                    choice = Greedy(_possibleIndex);
                    break;*/
            }
            //Retourner le choix
            return choice;
        }

        private List<Pile> SelectPossiblePiles(Pile[] _piles)
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

        /*private Pile Greedy(List<Pile> _possiblePile)
        {
            return null;
        }*/
        
    }
}