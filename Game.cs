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

        private List<string[]> asciiArtworks = new List<string[]>(){ new string[]{"       ", //0 : Champ de blé.
                                                                                  "       ",
                                                                                  "   ,,. ",
                                                                                  "|{;||)|"},
                                                                     new string[]{"    __ ", //1 : Boulangerie.
                                                                                  "  __|| ",
                                                                                 @" /Pain\",
                                                                                  " [()__]"},
                                                                     new string[]{"       ", //2 : Ferme.
                                                                                  " __ ( )",
                                                                                 @"/  \| |",
                                                                                  "|__||_|"},
                                                                     new string[]{"       ", //3 : Café.
                                                                                  "_______",
                                                                                  "|_Café|",
                                                                                  "|)[___]"},
                                                                     new string[]{" _____ ", //4 : Superette.
                                                                                 @"/  $  \",
                                                                                  "|-+   |",
                                                                                  "|_]___|"},
                                                                     new string[]{"   _   ", //5 : Forêt.
                                                                                  " _{ }_ ",
                                                                                  "{ }|{ }",
                                                                                  " | | | "},
                                                                     new string[]{" _____ ", //6 : Stade.
                                                                                  "(Stade)",
                                                                                 @"\  _  /",
                                                                                  "|_(_)_|"},
                                                                     new string[]{" _____ ", //7 : PMU.
                                                                                 @"/ PMU \",
                                                                                  "| +-+ |",
                                                                                  "|_|_|_|"},
                                                                     new string[]{"       ", //8 : Restaurant.
                                                                                  "_______",
                                                                                  "|Resto|",
                                                                                  "|nTn_[|"},
                                                                     new string[]{"    ___", //9 : Konbini.
                                                                                  "____|7|",
                                                                                  "|___  |",
                                                                                  "|[|]__|"},
                                                                     new string[]{" _____ ", //10 : Strip-club.
                                                                                 @"/Strip\",
                                                                                  "|___$_|",
                                                                                  "|[|___|"}};

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

        /// <summary>
        /// Déclaration de l'adversaire IA
        /// </summary>
        private IA adversaire;

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
        
        /// <summary>
        /// Bool pour le gamemode vanilla ou custom
        /// </summary>
        private bool gamemode;
        
        /// <summary>
        /// Indique la difficulté de l'IA
        /// </summary>
        public int difficulty { get; private set; }
        
        /// <summary>
        /// Indique la condition de victoire choisie
        /// </summary>
        public int winCondition { get; private set; }

        #endregion
        
        /// <summary>
        /// gère la créatioon des piles selon le gamemode choisi, la difficulté ainsi que la conditon de victorie et lance le jeu
        /// </summary>
        public Game()
        {
            display = new HMICUI(this);

            display.DisplayTitle();
            Console.ReadLine();
            Console.Clear();

            gamemode = display.ChooseMenu(0) == 0;
            if (gamemode)
            {
                //liste de cartes du jeu de base
                List<Card> vanillaDeck = new()
                {
                    new Card("Champs de blé", asciiArtworks[0], 1, new int[]{1,1}, 1, ConsoleColor.Cyan),
                    new Card("Boulangerie", asciiArtworks[1], 2, new int[] { 2, 2 }, 1, ConsoleColor.Green),
                    new Card("Ferme", asciiArtworks[2], 1, new int[]{1,1} , 2, ConsoleColor.Cyan),
                    new Card("Café", asciiArtworks[3], 1, new int[]{3,3}, 2, ConsoleColor.Red),
                    new Card("Superette", asciiArtworks[4], 3, new int[]{4,4}, 2, ConsoleColor.Green),
                    new Card("Forêt", asciiArtworks[5], 1, new int[]{5,5}, 2, ConsoleColor.Cyan),
                    new Card("Restaurant", asciiArtworks[8], 2, new int[]{5,5}, 4, ConsoleColor.Red),
                    new Card("Stade", asciiArtworks[6], 4, new int[]{6,6}, 6, ConsoleColor.Cyan),
                };
                
                currentDeck = vanillaDeck;
            }
            else
            {
                //Liste des cartes custom
                List<Card> customDeck = new()
                {
                    new Card("Champs de blé", asciiArtworks[0], 1, new int[]{1,1}, 1, ConsoleColor.Cyan),
                    new Card("Boulangerie", asciiArtworks[1], 2, new int[] { 2,2 }, 1, ConsoleColor.Green),
                    new Card("Ferme", asciiArtworks[2], 1, new int[]{1,2} , 2, ConsoleColor.Cyan),
                    new Card("Superette", asciiArtworks[4], 3, new int[] { 4, 4 }, 2, ConsoleColor.Green),
                    new Card("Forêt", asciiArtworks[5], 1, new int[] { 5, 5 }, 2, ConsoleColor.Cyan),
                    new Card("Café", asciiArtworks[3], 1, new int[] { 6, 7 }, 2, ConsoleColor.Red),
                    new Card("Restaurant", asciiArtworks[8], 2, new int[] { 5, 5 }, 4, ConsoleColor.Red),
                    new Card("Stade", asciiArtworks[6], 4, new int[]{3,3}, 6, ConsoleColor.Cyan),
                    new Card("PMU", asciiArtworks[7], 3, new int[] { 8, 9 }, 6, ConsoleColor.Green),
                    new Card("Konbini", asciiArtworks[9], 5, new int[] { 12, 12 }, 7, ConsoleColor.Cyan),
                    new Card("Strip-Club", asciiArtworks[10], 5, new int[] { 10, 11 }, 8, ConsoleColor.Red),
                };

                currentDeck = customDeck;
            }
            
            difficulty = display.ChooseMenu(1);
            winCondition = display.ChooseMenu(2);
            
            
            //Création du tableau des piles de carte
            piles = new Pile[currentDeck.Count];

            //Création de piles pour chaque carte et ajout dans le tableau
            for (int i = 0; i < piles.Length; i++)
            {
                piles[i] = new Pile(currentDeck[i]);
            }

            Console.Clear();
            startGame();
        }
        
        /// <summary>
        /// Méthode créant les cartes, les piles, les joueurs et donnant les cartes de départ
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
            switch (difficulty)
            {
                default:
                    adversaire = new IARandom(ia);
                    break;
                case 1 :
                    adversaire = new IASafe(ia, winCondition);
                    break;
                case 2:
                    adversaire = new IAOffensive(ia, winCondition);
                    break;
            }
            
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
        
        /// <summary>
        /// Méthode qui gère les tours successifs du joueur et de l'IA
        /// </summary>
        private void RunGame()
        {
            while (!endGame)
            {
                //On lance le tour du joueur 
                PlayNextTurn();
                
                //test des conditions de victoire joueur et affichage du message de victoire
                if (EndGame(players[0]))
                {
                    display.DisplayEndingMessage(true);
                    Console.ReadLine();
                    break;
                }
            
                //playnextturn IA
                PlayNextTurn();
               
                //test des conditions de victoire IA et affichage du message de défaite
                if (EndGame(players[1]))
                {
                    display.DisplayEndingMessage(false);
                    Console.ReadLine();
                    break;
                }
            }
            
        }
        
        
        /// <summary>
        /// Tour d'un joueur
        /// </summary>
        private void PlayNextTurn()
        {
            actualPlayer = actualPlayer == 0 ? 1 : 0;
            otherplayer = otherplayer == 1 ? 0 : 1;
            
            bool humanPlayer = actualPlayer == 0;
            int dieResult;
            int[] dieRolls;
            dieRolls = new int[2];
            
            //On gère le dé selon le mode de jeu (1 ou 2 dés)
            if (gamemode) dieResult = Die.Lancer();
            else
            {
                int nbDice; 
                if (actualPlayer == 0) nbDice = display.ChooseNbDice();
                else nbDice = adversaire.IANbDice();

                for (int i = 0; i < nbDice; i++) dieRolls[i] = Die.Lancer();
                dieResult = dieRolls[0] + dieRolls[1];
            }
            Console.Clear();

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
            if (EndGame(players[actualPlayer]) || EndGame(players[otherplayer]))
            {
                return;
            }

            //Affiche et permet de choisir parmi toutes les piles
            Card cardChoice = null; 
            
            //Choix du joueur humain
            if (actualPlayer == 0)
            {
                int selection = display.Choose(piles,players[0]);

                if (selection >= 0)
                {
                    Card choosedCard = currentDeck[selection];
                    
                    //Boucle tant que le joueur choisis une carte trop chère
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
            
            //Choix de l'IA
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

        /// <summary>
        /// Fonction vérifiant la fin du jeu selon la win condition choisie
        /// </summary>
        /// <param name="actualPlayer"></param>
        /// <returns>Bool qui si est true met fin au jeu</returns>
        private bool EndGame(Player actualPlayer)
        {
            endGame = false;

            switch (winCondition)
            {
                case  0 :
                    if (actualPlayer.pieces >= 10)
                    {
                        endGame = true;
                    }
                    break;
                    
                case  1 :
                    if (actualPlayer.pieces >= 20)
                    {
                        endGame = true;
                    }
                    break;
                
                case  2 :
                    if (actualPlayer.pieces >= 30)
                    {
                        endGame = true;
                    }
                    break;
                
                case  3 :
                    if (actualPlayer.pieces >= 20)
                    {
                        foreach ( Card card in currentDeck)
                        {
                            if (actualPlayer.city.Contains(card) == false)
                            {
                                return (endGame);
                            }
                        }
                        endGame = true;
                    }
                    break;
            }
            
            return(endGame);
        }
    }
}