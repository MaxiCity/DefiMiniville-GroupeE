using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection.Metadata.Ecma335;

namespace Miniville
{
    public class Game
    {
        #region Attributs
        
        /// <summary>
        /// display pour afficher et gérer le jeu
        /// </summary>
        HMICUI display;
        
        
        /// <summary>
        /// //Liste des cartes du jeu
        /// </summary>
        List<Card> cardsListe = new List<Card>();
        
        /// <summary>
        /// Piles des cartes à acheter
        /// </summary>
        Pile[] piles;
        
        /// <summary>
        /// Tableau des joueurs
        /// </summary>
        private Player[] players = new Player[2];

        private IA adversaire;
        
        /// <summary>
        /// Pou gérer le dés
        /// </summary>
        private Die die = new Die();

        /// <summary>
        /// int indiquant l'index du joueur dont c'est le tour
        /// </summary>
        int actualPlayer = 1;

        /// <summary>
        /// int indiquant l'index du joueur dont ce n'est pas le tour
        /// </summary>
        int otherplayer = 0;

        /// <summary>
        /// bool arrêtant la boucle de jeu
        /// </summary>
        private bool endGame;

        #endregion

        
        public Game()
        {
            display = new HMICUI(this);
            
            Card champsDeBlé = new Card("Champs de blé", 1, 1, 1, ConsoleColor.Cyan);
            cardsListe.Add(champsDeBlé);

            Card ferme = new Card("Ferme", 1, 1, 2, ConsoleColor.Cyan);
            cardsListe.Add(ferme);
            
            Card boulangerie = new Card("Boulangerie", 2, 2, 1, ConsoleColor.Green);
            cardsListe.Add(boulangerie);
            
            Card café = new Card("Café", 1, 3, 2, ConsoleColor.Red);
            cardsListe.Add(café);

            Card superette = new Card("Superette", 3, 4, 2, ConsoleColor.Green);
            cardsListe.Add(superette);
            
            Card forêt = new Card("Forêt", 1, 5, 2, ConsoleColor.Cyan);
            cardsListe.Add(forêt);
            
            Card restaurant = new Card("Restaurant", 2, 5, 4, ConsoleColor.Red);
            cardsListe.Add(restaurant);
            
            Card stade = new Card("Stade", 4, 6, 6, ConsoleColor.Cyan);
            cardsListe.Add(stade);
            
            //Création du tableau des piles de carte
            piles = new Pile[cardsListe.Count];

            //Création de piles pour chaque carte et ajout dans le tableau
            for (int i = 0; i < piles.Length; i++)
            {
                piles[i] = new Pile(cardsListe[i]);
            }
            
            startGame();
        }
        
        /// <summary>
        /// Méthode créant les cartes, les piles, les joueurs, donnant les cartes de départ
        /// </summary>
        public void startGame()
        {
            #region création joueur

            Player player = new Player();
            player.city.Add(cardsListe[0]);
            player.city.Add(cardsListe[2]);
            player.UpdateMoney(3);
            players[0] = player;
            
            
            Player ia = new Player();
            adversaire = new IA(ia);
            adversaire.player.city.Add(cardsListe[0]);
            adversaire.player.city.Add(cardsListe[2]);
            adversaire.player.UpdateMoney(3);
            players[1] = ia;
            
            #endregion
            
            display.ChooseName();
            Console.Clear();
            
            display.DisplayCities(players);
            Console.ReadLine();
            Console.Clear();
            
            RunGame();
            
            return;
        }
        //Un tour de joueur
        public void PlayNextTurn(Player[] players)
        {
            actualPlayer = actualPlayer == 0 ? 1 : 0;
            otherplayer = otherplayer == 1 ? 0 : 1;

            bool humanPlayer = actualPlayer == 0;

            //Début du tour lancer de dés
            int dieResult = Die.Lancer();

            //Résolution des effets de cartes
            int[] resultActualPlayer = players[actualPlayer].UseCards(true, dieResult);
            int[] resultOtherPlayer = players[otherplayer].UseCards(false, dieResult);
            players[otherplayer].UpdateMoney(-resultOtherPlayer[1]);
            
            //Affiche les villes des joueurs selon qui est le joueur actuelle et le résultat du dé
            display.DisplayCities(players, actualPlayer, dieResult);

            display.DisplayTurnResult(resultActualPlayer, resultOtherPlayer, humanPlayer);
            Console.ReadLine();
            Console.Clear();

            //Affiche et permet de choisir parmi toutes les piles
            
            if (actualPlayer == 0)
            {
                int selection = display.Choose(piles);
                
                if (selection >= 0)
                {
                    players[0].city.Add(piles[selection].Draw());
                }
            }
            else if (actualPlayer == 1)
            {
                Card cardChoice = adversaire.IAPlay(piles);
                display.DisplayIADraw(cardChoice);
                if (cardChoice != null)
                {
                    players[1].city.Add(cardChoice);
                }
            }

            //Affichage des villes des deux joueurs
            display.DisplayCities(players);
            Console.ReadLine();
            Console.Clear();
            return;
        }

        public void RunGame()
        {
            while (endGame == false)
            {
                //On lance le tour du joueur 
                PlayNextTurn(players);
                EndGame(players[0]);
            
                if (EndGame(players[0]))
                {
                    display.DisplayEndingMessage(true);
                    Console.ReadLine();
                    break;
                }
            
                //playnextturn IA
                PlayNextTurn(players);
                EndGame(players[1]);
            
                if (EndGame(players[1]))
                {
                    display.DisplayEndingMessage(false);
                    Console.ReadLine();
                    break;
                }
            }
            
        }
        
        
        /// <summary>
        /// Fonction vérifiant la fin du jeu à 20 pièces
        /// </summary>
        /// <param name="actualPlayer"></param>
        /// <returns>Bool qui si est true met fin au jeu</returns>
        public bool EndGame(Player actualPlayer)
        {
            if (actualPlayer.pieces >= 20)
            {
                endGame = true;
            }
            else
            {
                endGame = false;
            }

            return(endGame);
        }
    }
}