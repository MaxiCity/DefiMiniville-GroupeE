using System;
using System.Collections.Generic;
using System.Linq;

namespace Miniville
{
    public class IA
    {
        private enum PlayStyle
        {
            Random,
            Safe,
            Offensive,
        }

        private PlayStyle difficulty = PlayStyle.Safe;

        public Player player;
        private Random random = new Random();

        public IA(Player _player)
        {
            player = _player;
        }
        public IA(int _difficulty, Player _player)
        {
            difficulty = (PlayStyle)_difficulty;
            player = _player;
        }

        public int IANbDice()
        {
            int oneDiceScore = 0;
            int twoDiceScore = 0;
            int nbDice;
            
            //Si la difficulté est random, choisir aléatoirement entre 1 ou 2 dé
            if (difficulty == PlayStyle.Random)
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
                            oneDiceScore += difficulty == PlayStyle.Offensive ? card.moneyToEarn : 1;
                        }
                        else
                        {
                            twoDiceScore += difficulty == PlayStyle.Offensive ? card.moneyToEarn : 1;
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

        public Pile Choose(List<Pile> _possiblePiles)
        {
            Pile choice = null;

            switch (difficulty)
            {
                default:
                    //1 chance sur 4 de ne rien piocher du tout
                    if (random.Next(0,4)==0)
                    {
                        return null;
                    }
                    choice = _possiblePiles[random.Next(_possiblePiles.Count)];
                    //choisir une pile aléatoire parmi la liste des choix possibles
                    
                    break;
                case PlayStyle.Safe:
                    //Si on a plus de 13 pièces, économiser
                    if (player.pieces > 13)
                    {
                        return null;
                    }
                    
                    foreach (Pile pile in _possiblePiles)
                    {
                        foreach (int i in pile.card.dieCondition)
                        {
                            //Si on ne couvre pas le lancé de dé, choisir cette pile
                            //(Choisira la dernière pile dont on ne couvre pas la valeur)
                            if (CoveredDiceRoll()[i] == 0)
                                choice = pile;
                        }
                    }
                    break;
                
                case PlayStyle.Offensive:
                    //Si on a plus de 17 pièces, économiser
                    if (player.pieces > 17)
                    {
                        return null;
                    }
                    foreach (Pile pile in _possiblePiles)
                    {
                        foreach (int i in pile.card.dieCondition)
                        {
                            //Si on ne couvre pas le lancé de dé, choisir cette pile
                            //(Retourne directement cette pile)
                            if (CoveredDiceRoll()[i-1] == 0)
                            {
                                choice = pile;
                                return choice;
                            }
                        }
                    }
                    //S'il n'y a pas eu de retour précédent, acheter une carte aléatoire
                    choice = _possiblePiles[random.Next(_possiblePiles.Count)];
                    break;
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

        private int[] CoveredDiceRoll()
        {
            int[] coveredDiceRoll = new int[12];
            foreach (Card card in player.city)
            {
                foreach (int i in card.dieCondition)
                {
                    coveredDiceRoll[i - 1]++;
                }
            }

            foreach (int i in coveredDiceRoll)
            {
                Console.Write($"{i} ");
            }
            return coveredDiceRoll;
        }
    }
}