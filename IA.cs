using System;
using System.Collections.Generic;

namespace Miniville
{
    public class IA
    {
        private enum Difficulties
        {
            Random,
            Safe,
            Offensive,
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
            int oneDiceScore = 0;
            int twoDiceScore = 0;
            int nbDice;
            
            //Si la difficulté est random, choisir aléatoirement entre 1 ou 2 dé
            if (difficulty == Difficulties.Random)
            {
                nbDice = random.Next(1, 3);
            }
            else
            {
                foreach (Card card in player.city)
                {
                    foreach (int i in card.dieCondition)
                    {
                        
                        //Si inférieur 
                        if (i < 7)
                        {
                            //Si la difficulté est offensive, prendre en compte le gain pour le score du choix
                            oneDiceScore += difficulty == Difficulties.Offensive ? card.moneyToEarn : 1;
                        }
                        else
                        {
                            twoDiceScore += difficulty == Difficulties.Offensive ? card.moneyToEarn : 1;
                        }
                    }
                }
                nbDice = oneDiceScore > twoDiceScore ? 1 : 2; 
            }
            return nbDice;
        }

        public Card IAPlay(Pile[] _piles)
        {
            //Récupération de la liste des piles possibles
            List<Pile> possiblePiles = SelectPossiblePiles(_piles);
            
            //S'il y a au moins une pile disponible...
            if (possiblePiles.Count > 0)
            {
                //Piocher la carte
                Card choosenCard = Choose(possiblePiles).Draw();

                if (choosenCard != null)
                {
                    //L'ajouter au joueur IA
                    player.AddCard(choosenCard);
                    
                    //Ajuster son argent
                    player.UpdateMoney(-choosenCard.cost);
                }
                return choosenCard;
            }
            
            //Sinon, renvoyer null
            return null;
        }

        public Pile Choose(List<Pile> _possibleIndex)
        {
            Pile choice;

            switch (difficulty)
            {
                default:
                    
                    //1 chance sur 4 de ne rien retourner
                    if (random.Next(0,4) == 0)
                    {
                        return null;
                    }
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