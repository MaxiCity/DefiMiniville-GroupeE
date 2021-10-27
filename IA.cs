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

        public Card IAPlay(Pile[] _piles)
        {
            if (random.Next(1) == 1)
            {
                List<int> possiblePileIndex = SelectPossibleIndex(_piles);
                if (possiblePileIndex.Count > 0)
                {
                    Card choosenCard = _piles[Choose(possiblePileIndex)].Draw();
                    player.AddCard(choosenCard);
                    return choosenCard;
                }
            }

            return null;
        }

        public int Choose(List<int> _possibleIndex)
        {
            int choice;

            switch (difficulty)
            {
                default:
                    //choisir un index aléatoire parmi ceux enregistrés
                    choice = _possibleIndex[random.Next(_possibleIndex.Count)];
                    break;
            }
            //Retourner le choix
            return choice;
        }

        private List<int> SelectPossibleIndex(Pile[] _piles)
        {
            List<int> possibleIndex = new List<int>();
            
            //On parcours les piles du tableau...
            for (int i = 0; i < _piles.Length; i++)
            {
                //Si la pile n'est pas vide et que le prix de la carte est inférieur à l'argent du joueur IA...
                if (_piles[i].nbCard != 0 && _piles[i].card.cost <= player.pieces)
                {
                    //Ajouter l'index correspondant au choix possible
                    possibleIndex.Add(i);
                }
            }

            return possibleIndex;
        }
    }
}