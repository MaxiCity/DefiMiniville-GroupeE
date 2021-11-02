using System;
using System.Collections.Generic;

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
        /// //Liste des cartes du jeu selon le gamemode choisi
        /// </summary>
        private List<Card> currentDeck;
        
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
        int otherplayer;

        /// <summary>
        /// bool arrêtant la boucle de jeu
        /// </summary>
        private bool endGame;

        private bool gamemode;

        public int difficulty { get; private set; }

        #endregion
        
        public Game()
        {
            display = new HMICUI(this);
            gamemode = display.ChooseMenu(0) == 0;
            if (gamemode)
            {
                //Piles de cartes du jeu de base
                List<Card> vanillaDeck = new()
                {
                    new Card("Champs de blé", 1, new int[]{1,1}, 1, ConsoleColor.Cyan),
                    new Card("Boulangerie", 2, new int[] { 2, 2 }, 1, ConsoleColor.Green),
                    new Card("Ferme", 1, new int[]{1,1} , 2, ConsoleColor.Cyan),
                    new Card("Café", 1, new int[]{3,3}, 2, ConsoleColor.Red),
                    new Card("Superette", 3, new int[]{4,4}, 2, ConsoleColor.Green),
                    new Card("Forêt", 1, new int[]{5,5}, 2, ConsoleColor.Cyan),
                    new Card("Restaurant", 2, new int[]{5,5}, 4, ConsoleColor.Red),
                    new Card("Stade", 4, new int[]{6,6}, 6, ConsoleColor.Cyan),
                };
                
                currentDeck = vanillaDeck;
            }
            else
            {
                List<Card> customDeck = new()
                {
                    new Card("Champs de blé", 1, new int[]{1,1}, 1, ConsoleColor.Cyan),
                    new Card("Boulangerie", 2, new int[] { 2, 2 }, 1, ConsoleColor.Green),
                    new Card("Ferme", 1, new int[]{1,2} , 2, ConsoleColor.Cyan),
                    new Card("Superette", 3, new int[] { 4, 4 }, 2, ConsoleColor.Green),
                    new Card("Forêt", 1, new int[] { 5, 5 }, 2, ConsoleColor.Cyan),
                    new Card("Café", 1, new int[] { 6, 7 }, 2, ConsoleColor.Red),
                    new Card("Restaurant", 2, new int[] { 5, 5 }, 4, ConsoleColor.Red),
                    new Card("Stade", 4, new int[]{3,3}, 6, ConsoleColor.Cyan),
                    new Card("PMU", 3, new int[] { 8, 9 }, 6, ConsoleColor.Green),
                    new Card("Konbini", 5, new int[] { 12, 12 }, 7, ConsoleColor.Cyan),
                    new Card("Strip-Club", 5, new int[] { 10, 11 }, 8, ConsoleColor.Red),
                };

                currentDeck = customDeck;
            }

            difficulty = display.ChooseMenu(1);
            //Création du tableau des piles de carte
            piles = new Pile[currentDeck.Count];

            //Création de piles pour chaque carte et ajout dans le tableau
            for (int i = 0; i < piles.Length; i++)
            {
                piles[i] = new Pile(currentDeck[i]);
            }
            
            startGame();
        }
        
        /// <summary>
        /// Méthode créant les cartes, les piles, les joueurs, donnant les cartes de départ
        /// </summary>
        private void startGame()
        {
            #region création joueur

            Player player = new Player();
            player.city.Add(currentDeck[0]);
            player.city.Add(currentDeck[1]);
            player.UpdateMoney(3);
            players[0] = player;
            
            
            Player ia = new Player();
            adversaire = new IA(ia);
            adversaire.player.city.Add(currentDeck[0]);
            adversaire.player.city.Add(currentDeck[1]);
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
        private void PlayNextTurn()
        {
            actualPlayer = actualPlayer == 0 ? 1 : 0;
            otherplayer = otherplayer == 1 ? 0 : 1;

            bool humanPlayer = actualPlayer == 0;
            int dieResult;
            int[] dieRolls;
            dieRolls = new int[2];
            
            if(gamemode) dieResult = Die.Lancer();
            else
            {
                int nbDice; 
                if (actualPlayer == 0) nbDice = display.ChooseNbDice();
                else nbDice = 2;

                for (int i = 0; i < nbDice; i++) dieRolls[i] = Die.Lancer();
                dieResult = dieRolls[0] + dieRolls[1];
            }
            Console.Clear();
            //Début du tour lancer de dés

            //Résolution des effets de cartes
            int[] resultActualPlayer = players[actualPlayer].UseCards(true, dieResult);
            int[] resultOtherPlayer = players[otherplayer].UseCards(false, dieResult);
            int stealDifference = resultOtherPlayer[1]-players[actualPlayer].pieces;
            
            //Si on vole plus que ce que le joueur a...
            if (stealDifference > 0)
            {
                //On retire ce supplément
                players[otherplayer].UpdateMoney(-(stealDifference));
                resultOtherPlayer[0] -= stealDifference;
                
                //On diminue la quantité volée
                resultOtherPlayer[1] -= stealDifference;
            }
            
            //On retire ce qui a été volé au joueur
            players[actualPlayer].UpdateMoney(-resultOtherPlayer[1]);
            
 
            
            //Affiche les villes des joueurs selon qui est le joueur actuelle et le résultat du dé.
            display.DisplayCities(players, actualPlayer, dieResult, dieRolls);

            display.DisplayTurnResult(resultActualPlayer, resultOtherPlayer, humanPlayer);
            Console.ReadLine();
            Console.Clear();

            //Affiche et permet de choisir parmi toutes les piles

            Card cardChoice = null; 
            
            if (actualPlayer == 0)
            {
                int selection = display.Choose(piles,players[0]);

                if (selection >= 0)
                {
                    Card choosedCard = currentDeck[selection];

                    while (choosedCard.cost > players[0].pieces)
                    {
                        Console.Clear();
                        selection = display.Choose(piles, players[0]);
                        if (selection >= 0)
                        {
                            choosedCard = currentDeck[selection];
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (selection >= 0)
                    {
                        players[0].city.Add(piles[selection].Draw());
                        players[0].UpdateMoney(-choosedCard.cost);
                    }
                }
            }
            
            else if (actualPlayer == 1)
            {
                cardChoice = adversaire.IAPlay(piles);
            }

            //Affichage des villes des deux joueurs
            display.DisplayCities(players);
            if(actualPlayer == 1) display.DisplayIADraw(cardChoice);
            Console.ReadLine();
            Console.Clear();
            return;
        }

        private void RunGame()
        {
            while (!endGame)
            {
                //On lance le tour du joueur 
                PlayNextTurn();

                if (EndGame(players[0]))
                {
                    display.DisplayEndingMessage(true);
                    Console.ReadLine();
                    break;
                }
            
                //playnextturn IA
                PlayNextTurn();
               
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
        private bool EndGame(Player actualPlayer)
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