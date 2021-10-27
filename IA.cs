using System;
using System.Collections.Generic;

namespace Miniville
{
    public class IA
    {
        private string difficulty;
        public Player player;
        private Random random = new Random();

        public IA(string _difficulty, Player _player)
        {
            
            difficulty = _difficulty;
            player = _player;
        }

        public Card IAPlay(Pile[] _piles)
        {
            Card choosenCard = _piles[Choose(_piles)].Draw();
            player.AddCard(choosenCard);
            return choosenCard;
        }

        public int Choose(Pile[] _piles)
        {
            //Déclaration du choix et de la liste des choix possibles;
            int choice;
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
            //choisir un index aléatoire parmi ceux enregistrés
            choice = possibleIndex[random.Next(possibleIndex.Count)];
            
            //Retourner le choix
            return choice;
        }
    }
}