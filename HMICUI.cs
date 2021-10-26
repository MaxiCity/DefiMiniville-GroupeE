using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miniville
{
    class HMICUI
    {
        private Game ctrl;

        private int maxLength = 15;
        private int nbLines = 10;
        private ConsoleColor writingColor = ConsoleColor.Gray;

        private int WIPempty = 2;
        bool[] cardEmpty = { true, true, false, true, false, false, false, false };
        string[] cardActCost = { "1", "1", "2", "3", "4", "5", "5", "6" };
        ConsoleColor[] cardColors = { ConsoleColor.Cyan, ConsoleColor.Cyan, ConsoleColor.Green, ConsoleColor.Red, ConsoleColor.Green,ConsoleColor.Cyan, ConsoleColor.Red, ConsoleColor.Cyan };
        string[] cardNames = { "Champs de blé", "Ferme", "Boulangerie", "Café", "Superette", "Forêt", "Restaurant", "Stade" };
        string[] cardGains = { "1", "1", "2", "1", "3", "1", "2", "4" };
        string[] cardCosts = { "1", "2", "1", "2", "2", "2", "4", "6" };
        string[] cardDesc1 = { "Gagnez 1$" , "Gagnez 1$", "Gagnez 2$", "Volez 1$", "Gagnez 3$", "Gagnez 1$", "Volez 2$", "Gagnez 4$" };
        string[] cardDesc2 = { "S'active tout", "S'active tout", "S'active à", "S'active au", "S'active à", "S'active tout", "S'active au", "S'active tout"};
        string[] cardDesc3 = { " le temps", " le temps","votre tour", "tour adverse", "votre tour", "le temps", "tour adverse","le temps" };


        public HMICUI(Game _ctrl)
        {
            ctrl = _ctrl;
            Console.WindowWidth = 145;
            Console.CursorVisible = false;
        }

        public int Choose(/*, _Pile[] piles = new Pile[0];*/)
        {
            int selection = 0;
            while (cardEmpty[selection])
                        {
                            if (selection - 1 <= cardNames.Length - 1) ++selection;
                            else selection = 0;
                        }

            int cursorPositionX = 0;
            int cursorPositionY = 10;
            int cursorOffset = 8;

            // Affichage du curseur en première position.
            DisplayCardStacks(selection);
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
                        if (selection == cardNames.Length-1) selection = 0;
                        // Sinon il passe simplement à droite.
                        else selection += 1;
                        // On passe toutes les piles de cartes vides.
                        while (cardEmpty[selection])
                        {
                            if (selection - 1 <= cardNames.Length - 1) ++selection;
                            else selection = 0;
                        }
                        break;

                    case ConsoleKey.LeftArrow: // Fait passer le curseur vers la gauche.
                        // Si le curseur est tout à gauche il va en dernière position à droite.
                        if (selection == 0) selection = cardNames.Length-1;
                        // Sinon il passe simplement à gauche.
                        else selection -= 1;
                        // On passe toutes les piles de cartes vides.
                        while (cardEmpty[selection])
                        {
                            if (selection - 1 >= 0) --selection;
                            else selection = cardNames.Length - 1;
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
            return selection;
        }

        public void DisplayCities()
        {

        }

        private void DisplayCardStacks(int selection/*, _Pile[] piles*/)
        {
            string sep = "+---------------+";
            string space = "|               |";

            for (int i = 0; i < nbLines; i++)
            {
                for (int j = 0; j < cardNames.Length; j++)
                {
                    if (cardEmpty[j]) writingColor = ConsoleColor.Gray;// WIP pile.nbCards
                    else writingColor = cardColors[j];
                    Console.ForegroundColor = writingColor;

                    switch (i)
                    {
                        case 0: Console.Write(sep); break;
                        case 1: // Coût d'activation par les dés.
                            if (!cardEmpty[j])
                            {
                                Console.Write("|");
                                WriteInColor(AlignString(cardActCost[j]), ConsoleColor.White);
                                Console.Write($"|");
                            }
                            else Console.Write(space);
                            break;
                        case 2: // Coût et gain en pièces.
                            if (!cardEmpty[j])
                            {
                                Console.Write($"| ");
                                WriteInColor($"+{ cardGains[j]}", ConsoleColor.Yellow);
                                Console.Write("        ");
                                WriteInColor($" {cardCosts[j]}$", ConsoleColor.Yellow);
                                Console.Write(" |");
                            }
                            else Console.Write(space);
                            break;
                        case 3: Console.Write(space); break;
                        case 4: // Nom de la carte.
                            if (!cardEmpty[j])
                            {
                                Console.Write("|");
                                WriteInColor(AlignString(cardNames[j]), ConsoleColor.White);
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
                            if (!cardEmpty[j])
                            {
                                Console.Write("|");
                                WriteInColor(AlignString(cardDesc1[j]), ConsoleColor.Yellow);
                                Console.Write("|");
                            }
                            else Console.Write(space);
                            break;
                        case 7: // Première ligne de l'activation.
                            if (!cardEmpty[j])
                            {
                                Console.Write("|");
                                WriteInColor(AlignString(cardDesc2[j]), ConsoleColor.Gray);
                                Console.Write("|");
                            }
                            else Console.Write(space);
                            break;
                        case 8: // Deuxième ligne de l'activation.
                            if (!cardEmpty[j])
                            {
                                Console.Write("|");
                                WriteInColor(AlignString(cardDesc3[j]), ConsoleColor.Gray);
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

        private void DisplayRoll(int face) { Console.Write($"+---+\n| {face} |\n +---+\n"); }

        private void WriteInColor(string toWrite, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(toWrite);
            Console.ForegroundColor = writingColor;
        }

        private string AlignString(string toAlign)
        {
            if (toAlign.Length >= maxLength) return toAlign;

            int leftPadding = (maxLength - toAlign.Length) / 2;
            int rightPadding = maxLength - toAlign.Length - leftPadding;

            return new string(' ', leftPadding) + toAlign + new string(' ', rightPadding);
        }

        /*private int SelectionShift(int selection)
        {
            int newSelection = 0;
            Console.WriteLine(cardEmpty[selection]);
            
            
            return newSelection;
        }*/
    }
}
