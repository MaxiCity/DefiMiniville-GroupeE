using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace Miniville
{
    public class Game
    {
        /// <summary>
        /// Méthode créant les cartes, les piles, les joueurs, donnant les cartes de départ
        /// </summary>
        public void startGame()
        {
            #region Carte
            
            // Création de toutes les cartes et de la liste les contenant
            List<Card> cardsListe = new List<Card>();
            
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
            Pile[] piles = new Pile[cardsListe.Count];

            //Création de piles pour chaque carte et ajout dans le tableau
            for (int i = 0; i < piles.Length; i++)
            {
                piles[i] = new Pile(cardsListe[i]);
            }
            
            #endregion

            Player player = new Player();
            player.city.Add(champsDeBlé);
            player.city.Add(boulangerie);
            player.UpdateMoney(3);
            
            
        }
    }
}