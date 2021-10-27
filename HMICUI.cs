using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miniville
{
    /** Classe qui gère toute la partie interface homme-machine. */
    class HMICUI
    {
        /** Référence du contrôleur de l'application. */
        private Game ctrl;

        /** Le nombre de caractères à l'intérieur d'une carte.*/
        private int maxLength = 15;
        /** Les nombres de lignes en fonction de la taille de l'aperçu de carte.*/
        private int nbLines = 10;
        private int nbLinesCity = 5;
        /** La couleur actuelle d'écriture dans la console.*/
        private ConsoleColor writingColor = ConsoleColor.Gray;
        /** La liste des noms féminins parmi les cartes. */
        private string[] femNames = { "Ferme", "Forêt", "Superette", "Boulangerie" };

        /** Les lignes qui composent les différentes faces d'un dé en ascii art. */
        private List<string[]> asciiDiceFaces = new List<string[]>() { new string[] { "+-------+",
                                                                                      "|       |",
                                                                                      "|   o   |",
                                                                                      "|       |",
                                                                                      "+-------+"},
                                                                       new string[] { "+-------+",
                                                                                      "| o     |",
                                                                                      "|       |",
                                                                                      "|     o |",
                                                                                      "+-------+"},
                                                                       new string[] { "+-------+",
                                                                                      "| o     |",
                                                                                      "|   o   |",
                                                                                      "|     o |",
                                                                                      "+-------+"},
                                                                       new string[] { "+-------+",
                                                                                      "| o   o |",
                                                                                      "|       |",
                                                                                      "| o   o |",
                                                                                      "+-------+"},
                                                                       new string[] { "+-------+",
                                                                                      "| o   o |",
                                                                                      "|   o   |",
                                                                                      "| o   o |",
                                                                                      "+-------+"},
                                                                       new string[] { "+-------+",
                                                                                      "| o   o |",
                                                                                      "| o   o |",
                                                                                      "| o   o |",
                                                                                      "+-------+"},


        };

        /** Constructeur de la classe
         * <param name="_ctrl"> La référence du contrôleur. </param>
         */
        public HMICUI(Game _ctrl)
        {
            ctrl = _ctrl;
            Console.WindowWidth = 145;
            Console.CursorVisible = false;
        }
       
        /** Méthode permettant de récupérer le choix de l'utilisateur.
         * Utilise la méthode DisplayCardStacks pour afficher les piles de cartes.
         * <param name="piles"> Le tableau des piles provenant du contrôleur. </param>
         */
        public int Choose( Pile[] piles)
        {
            int selection = 0;
            // On décale le curseur au démarrage si la première pile est 
            while (piles[selection].nbCard <= 0)
            {
                if (selection - 1 <= piles.Length) ++selection;
                else selection = 0;
            }

            // Variables relatives à la position du curseur.
            int cursorPositionX = 0;
            int cursorPositionY = 10;
            int cursorOffset = 8;

            // Affichage du curseur.
            DisplayCardStacks(selection,piles);
            cursorPositionX = cursorOffset + 18 * selection;
            Console.SetCursorPosition(cursorPositionX, cursorPositionY);
            WriteInColor("/\\", ConsoleColor.White);

            bool choice = false;
            do // Enregistrement de la touche pressée par l'utilisateur.
            {
                ConsoleKeyInfo keyPressed = Console.ReadKey();
                switch (keyPressed.Key)
                {
                    case ConsoleKey.RightArrow: // Fait passer le curseur vers la droite.
                        // Si le curseur est tout à droite il revient en première position à gauche.
                        if (selection == piles.Length - 1) selection = 0;
                        // Sinon il passe simplement à droite.
                        else selection += 1;
                        // On passe toutes les piles de cartes vides.
                        while (piles[selection].nbCard <= 0)
                        {
                            if (selection + 1 < piles.Length) ++selection;
                            else selection = 0;
                        }
                        break;

                    case ConsoleKey.LeftArrow: // Fait passer le curseur vers la gauche.
                        // Si le curseur est tout à gauche il va en dernière position à droite.
                        if (selection == 0) selection = piles.Length - 1;
                        // Sinon il passe simplement à gauche.
                        else selection -= 1;
                        // On passe toutes les piles de cartes vides.
                        while (piles[selection].nbCard <= 0)
                        {
                            if (selection - 1 >= 0) --selection;
                            else selection = piles.Length - 1;
                        }
                        break;

                    case ConsoleKey.Enter: // Le joueur a choisi.
                        Console.Clear();
                        // On sort de la boucle.
                        choice = true;
                        break;
                }
                

                // Clear de la partie basse de l'affichage;
                Console.SetCursorPosition(0, cursorPositionY);
                Console.Write(new string(' ', 8*18));

                // Nouvelle position du curseur sous la pile suivante.
                cursorPositionX = 8 + 18 * selection;
                Console.SetCursorPosition(cursorPositionX, cursorPositionY);
                WriteInColor("/\\",ConsoleColor.White);
            }
            while (!choice);
            // On renvoit le choix du joueur sous forme d'index.
            return selection;
        }

        /** Méthode permettant d'afficher les cartes sur le plateau des deux joueurs. */
        public void DisplayCities(Player[] players, int playerTurn, int dieRoll = 0)
        {
            string sep = "+-----+";
            string space = "|     |";
            for (int playerIndex = 1; playerIndex >= 0; playerIndex--)
            {
                for (int i = 0; i < nbLinesCity; i++)
                {
                    foreach (Card c in players[playerIndex].city)
                    {
                        writingColor = c.color;
                        Console.ForegroundColor = writingColor;

                        switch (i)
                        {
                            case 0: Console.Write(sep); break;
                            case 1:
                                if(dieRoll == c.dieCondition || dieRoll == 0)
                                {
                                    Console.Write("|  ");
                                    WriteInColor($"{c.dieCondition}", ConsoleColor.White);
                                    Console.Write("  |");
                                }
                                else Console.Write(space);
                                break;
                            case 2:
                                if(dieRoll == c.dieCondition || dieRoll == 0)
                                {
                                    Console.Write("| ");
                                    if (c.color.Equals(ConsoleColor.Red) && playerIndex == 1) WriteInColor($"-{c.moneyToEarn}$", ConsoleColor.Yellow);
                                    else WriteInColor($"+{c.moneyToEarn}$", ConsoleColor.Yellow);
                                    Console.Write(" |");
                                }
                                else Console.Write(space);
                                break;
                            case 3: Console.Write(space); break;
                            case 4: Console.Write(sep); break;
                        }
                        Console.Write(" ");
                    }
                    if (playerIndex == playerTurn && dieRoll > 0)
                    {
                        writingColor = ConsoleColor.White;
                        Console.ForegroundColor = writingColor;
                        Console.Write("          " + asciiDiceFaces[dieRoll-1][i]);
                    }
                    if (i == nbLinesCity - 1 && playerIndex == 0 && playerTurn == 0)
                    {
                        writingColor = ConsoleColor.Gray;
                        Console.ForegroundColor = writingColor;
                        Console.Write("      Vos pièces : ");
                        WriteInColor($"{players[0].pieces }$", ConsoleColor.Yellow);
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("\n\n\n\n");
            }
        }

        public void DisplayEndingMessage(bool win)
        {

        }

        public void DisplayTurnResult(Player[] players)
        {
            
            Console.Write("Durant ce tour, l'IA a gagné ");
        }
       
        /** Méthode permettant d'afficher les piles de cartes.
         * <param name="selection"> l'endroit actuel où le curseur doit être affiché. </param>
         */
        private void DisplayCardStacks(int selection, Pile[] piles)
        {
            // Bords supérieurs et inférieurs de la carte.
            string sep = "+---------------+";
            // espaces vides à l'intérieur de la carte.
            string space = "|               |";

            for (int i = 0; i < nbLines; i++)
            {
                for (int j = 0; j < piles.Length; j++)
                {
                    // Si la pile est vide on écrit en gris.
                    if (piles[j].nbCard <= 0) writingColor = ConsoleColor.Gray;
                    // Sinon on récupère la couleur de la carte pour écrire.
                    else writingColor = piles[j].card.color;
                    Console.ForegroundColor = writingColor;

                    // En fonction de la ligne actuelle.
                    switch (i)
                    {
                        case 0: Console.Write(sep); break;
                        case 1: // Coût d'activation par les dés.
                            if (piles[j].nbCard > 0)
                            {
                                Console.Write("|");
                                WriteInColor(AlignString("" + piles[j].card.dieCondition), ConsoleColor.White);
                                Console.Write($"|");
                            }
                            else Console.Write(space);
                            break;
                        case 2: // Coût et gain en pièces.
                            if (piles[j].nbCard > 0)
                            {
                                Console.Write($"| ");
                                WriteInColor($"+{ piles[j].card.moneyToEarn }", ConsoleColor.Yellow);
                                Console.Write("        ");
                                WriteInColor($" { piles[j].card.cost }$", ConsoleColor.Yellow);
                                Console.Write(" |");
                            }
                            else Console.Write(space);
                            break;
                        case 3: Console.Write(space); break;
                        case 4: // Nom de la carte.
                            if (piles[j].nbCard > 0)
                            {
                                Console.Write("|");
                                WriteInColor(AlignString(piles[j].card.name), ConsoleColor.White);
                                Console.Write("|");
                            }
                            else
                            {
                                Console.Write("|");
                                Console.Write(AlignString("Empty"));
                                Console.Write("|");
                            }
                            break;
                        case 5: Console.Write(space); break;
                        case 6: // Description de l'effet.
                            if (piles[j].nbCard > 0)
                            {
                                Console.Write("|");
                                WriteInColor(AlignString(piles[j].card.description[0]), ConsoleColor.Yellow);
                                Console.Write("|");
                            }
                            else Console.Write(space);
                            break;
                        case 7: // Première ligne de l'activation.
                            if (piles[j].nbCard > 0)
                            {
                                Console.Write("|");
                                WriteInColor(AlignString(piles[j].card.description[1]), ConsoleColor.Gray);
                                Console.Write("|");
                            }
                            else Console.Write(space);
                            break;
                        case 8: // Deuxième ligne de l'activation.
                            if (piles[j].nbCard > 0)
                            {
                                Console.Write("|");
                                WriteInColor(AlignString(piles[j].card.description[2]), ConsoleColor.Gray);
                                Console.Write("|");
                            }
                            else Console.Write(space);
                            break;
                        case 9: Console.Write(sep); break;
                    }
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }
        /** Méthode permettant d'affiche ce que l'IA a pioché. */
        public void DisplayIADraw(Card card)
        {
            string determinant = "un ";
            foreach (string name in femNames) if (card.name == name) determinant = "une ";
            Console.Write("Ce tour ci, l'IA a choisi d'ajouter à sa ville " + determinant);
            WriteInColor(card.name, card.color);
            Console.WriteLine(".");
        }
        /** Méthode permettant d'écrire en couleur dans la console.
        * <param name="toWrite"> La chaîne de caractère à écrire en couleur. </param>
        * <param name="color"> La couleur avec laquelle écrire. </param>
        */
        private void WriteInColor(string toWrite, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(toWrite);
            Console.ForegroundColor = writingColor;
        }
        /** Méthode permettant d'aligner une chaîne de caractère par rapport. 
         * <param name="toAlign"> La chaîne de caractères à aligner. </param>
         */
        private string AlignString(string toAlign)
        {
            if (toAlign.Length >= maxLength) return toAlign;

            int leftPadding = (maxLength - toAlign.Length) / 2;
            int rightPadding = maxLength - toAlign.Length - leftPadding;

            return new string(' ', leftPadding) + toAlign + new string(' ', rightPadding);
        }
    }
}
