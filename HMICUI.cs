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
        #region attributs
        ///<summary> Référence du contrôleur de l'application. </summary>
        private Game ctrl;
        ///<summary> Le nom du joueur à aff </summary>
        private string playerName;

        ///<summary> Le nombre de caractères à l'intérieur d'une carte. </summary>
        private int maxLength = 15;
        ///<summary> Les nombres de lignes lorsqu'on affiche les piles de cartes. </summary>
        private int nbLines = 10;
        ///<summary> Les nombres de lignes lorsqu'on affiche les villes. </summary>
        private int nbLinesCity = 5;
        ///<summary> La couleur actuelle d'écriture dans la console. </summary>
        private ConsoleColor writingColor = ConsoleColor.Gray;
        ///<summary> La liste des noms féminins parmi les cartes. </summary>
        private string[] femNames = { "Ferme", "Forêt", "Superette", "Boulangerie" };

        ///<summary> Les lignes qui composent les différentes faces d'un dé en ascii art. </summary>
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
        ///<summary> Les lignes qui composent les deux messages de fin. </summary>
        private List<string[]> asciiEndMessage = new List<string[]>() { new string[] {" __     __  __              __                __",
                                                                                      "/  |   /  |/  |            /  |              /  |",
                                                                                      "$$ |   $$ |$$/   _______  _$$ |_     ______  $$/   ______    ______  ",
                                                                                      "$$ |   $$ |/  | /       |/ $$   |   /      \\ /  | /      \\  /      \\ ",
                                                                                      "$$  \\ /$$/ $$ |/$$$$$$$/ $$$$$$/   /$$$$$$  |$$ |/$$$$$$  |/$$$$$$  |",
                                                                                      " $$  /$$/  $$ |$$ |        $$ | __ $$ |  $$ |$$ |$$ |  $$/ $$    $$ |",
                                                                                      "  $$ $$/   $$ |$$ \\_____   $$ |/  |$$ \\__$$ |$$ |$$ |      $$$$$$$$/",
                                                                                      "   $$$/    $$ |$$       |  $$  $$/ $$    $$/ $$ |$$ |      $$       |",
                                                                                      "    $/     $$/  $$$$$$$/    $$$$/   $$$$$$/  $$/ $$/        $$$$$$$/ "},
                                                                        new string[] {" _______              ______           __    __",
                                                                                      "/       \\            /      \\         /  |  /  |",
                                                                                      "$$$$$$$  |  ______  /$$$$$$  |______  $$/  _$$ |_     ______  ",
                                                                                      "$$ |  $$ | /      \\ $$ |_ $$//      \\ /  |/ $$   |   /      \\ ",
                                                                                      "$$ |  $$ |/$$$$$$  |$$   |   $$$$$$  |$$ |$$$$$$/   /$$$$$$  |",
                                                                                      "$$ |  $$ |$$    $$ |$$$$/    /    $$ |$$ |  $$ | __ $$    $$ |",
                                                                                      "$$ |__$$ |$$$$$$$$/ $$ |    /$$$$$$$ |$$ |  $$ |/  |$$$$$$$$/",
                                                                                      "$$    $$/ $$       |$$ |    $$    $$ |$$ |  $$  $$/ $$       |",
                                                                                      "$$$$$$$/   $$$$$$$/ $$/      $$$$$$$/ $$/    $$$$/   $$$$$$$/"}
        };
        #endregion attributs

        /// <summary> Enregistre la référence du contrôleur et initialise la taille de la console. </summary>
        /// <param name="_ctrl"> La référence au contrôleur. </param>
        public HMICUI(Game _ctrl)
        {
            ctrl = _ctrl;
            Console.WindowWidth = 145;
            Console.CursorVisible = false;
        }

        ///<summary> Permet de récupérer l'index de la pile choisie par l'utilisateur. </summary>
        ///<param name="piles">Le tableau des piles provenant du contrôleur. </param>
        ///<returns> L'index de la pile choisie par le joueur. </returns>
        public int Choose( Pile[] piles, Player humanPlayer)
        {
            int selection = 0;
            // On décale le curseur au démarrage si la première pile est vide.
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
            Console.SetCursorPosition(cursorPositionX, cursorPositionY + 1);
            WriteInColor(humanPlayer.pieces + "$".PadLeft(2, ' '), ConsoleColor.Yellow);

            bool choice = false;
            
            // Enregistrement de la touche pressée par l'utilisateur.
            do
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

                    case ConsoleKey.Delete: // Si le joueur ne souhaite pas piocher de carte.
                        // On siginifie au contrôleur que le joueur n'a rien choisi.
                        selection = -1;
                        // On sort de la boucle.
                        choice = true;
                        break;

                    case ConsoleKey.Enter: // Le joueur a choisi une pile depuis laquelle piocher.
                        // On sort de la boucle.
                        choice = true;
                        break;
                }

                // Clear de la partie basse de l'affichage;
                Console.SetCursorPosition(0, cursorPositionY);
                Console.Write(new string(' ', 8*18));
                Console.SetCursorPosition(0, cursorPositionY+1);
                Console.Write(new string(' ', 8 * 18));

                // Nouvelle position du curseur sous la pile suivante.
                cursorPositionX = 8 + 18 * selection;
                if (selection >= 0)
                {
                    Console.SetCursorPosition(cursorPositionX, cursorPositionY);
                    WriteInColor("/\\", ConsoleColor.White);
                    Console.SetCursorPosition(cursorPositionX, cursorPositionY+1);
                    WriteInColor(humanPlayer.pieces+"$".PadLeft(2,' '), ConsoleColor.Yellow);
                }
            }

            while (!choice);
            // On renvoit le choix du joueur sous forme d'index.
            Console.Clear();
            return selection;
        }

        ///<summary> Permet d'afficher les cartes sur le plateau des deux joueurs. </summary>
        ///<param name="players"> Un tableau contenant tous les joueurs. </param>
        ///<param name="playerTurn"> L'index du joueur à qui c'est le tour. </param>
        ///<param name="dieRoll"> le résultat du lancé de dé, si il n'est pas spécifié c'est qu'on souhaite tout afficher. </param>
        public void DisplayCities(Player[] players, int playerTurn = 0, int dieRoll = 0)
        {
            string sep = "+-----+";
            string space = "|     |";

            bool show = false;
            for (int playerIndex = 1; playerIndex >= 0; playerIndex--)
            {
                for (int i = 0; i < nbLinesCity; i++)
                {
                    foreach (Card c in players[playerIndex].city)
                    {
                        writingColor = c.color;
                        Console.ForegroundColor = writingColor;
                        
                        // Condition selon laquelle on montre l'effet d'une carte ou non.
                        show = dieRoll == 0 ||
                              (dieRoll == c.dieCondition && (playerTurn == playerIndex && (c.color.Equals(ConsoleColor.Green) || c.color.Equals(ConsoleColor.Cyan))
                              || (playerTurn != playerIndex && (c.color.Equals(ConsoleColor.Red) || c.color.Equals(ConsoleColor.Cyan)))));
                        
                        switch (i)
                        {
                            case 0: Console.Write(sep); break;
                            case 1:
                                if (show)
                                {
                                    Console.Write("|  ");
                                    WriteInColor($"{c.dieCondition}", ConsoleColor.White);
                                    Console.Write("  |");
                                }
                                else Console.Write(space);
                                break;
                            case 2:
                                if (show)
                                {
                                    Console.Write("| ");
                                    if (c.color.Equals(ConsoleColor.Red)) WriteInColor($"-{c.moneyToEarn}$", ConsoleColor.Yellow);
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
                    if (i == nbLinesCity - 1 && dieRoll == 0)
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
            Console.WriteLine();
            writingColor = ConsoleColor.Gray;
            Console.ForegroundColor = writingColor;
        }

        ///<summary>Permet d'afficher de manière textuelle les résultats d'un tour. </summary>
        ///<param name="playerResult"> les gains et vols du joueur. </param>
        ///<param name="IAResult"> les gains et vols de l'IA. </param>
        public void DisplayTurnResult(int[] actualPlayer, int[] otherPlayer, bool humanPlayer)
        {
            int playerTotal;
            int IATotal;

            if (humanPlayer)
            {
                playerTotal = actualPlayer[0] - otherPlayer[1];
                IATotal = otherPlayer[0];
            }
            else
            {
                playerTotal = otherPlayer[0];
                IATotal = actualPlayer[0] - otherPlayer[1];
            }

            // Message en rapport avec les gains et pertes du joueur.
            string entryMsg = "Durant ce tour, ";
            string msg;

            if (playerTotal == 0)
            {
                Console.Write(entryMsg);
                WriteInColor(playerName, ConsoleColor.Blue);
                Console.WriteLine(" n'a rien gagné ni perdu. ");
            }
            else
            {
                if (playerTotal > 0) msg = " a gagné ";
                else msg = " a perdu ";

                Console.Write(entryMsg);
                WriteInColor(playerName, ConsoleColor.Blue);
                Console.Write(msg);
                WriteInColor(playerTotal + "$", ConsoleColor.Yellow);
                Console.WriteLine(".");
            }

            // Message en rapport avec les gains et pertes de l'IA.
            entryMsg = "Et, ";
            if (IATotal == 0)
            {
                Console.Write(entryMsg);
                WriteInColor("l'IA", ConsoleColor.DarkRed);
                Console.WriteLine(" n'a rien gagné ni perdu. ");
            }
            else
            {
                if (IATotal > 0) msg = " a gagné ";
                else msg = " a perdu ";

                Console.Write(entryMsg);
                WriteInColor("l'IA", ConsoleColor.DarkRed);
                Console.Write(msg);
                WriteInColor(IATotal + "$", ConsoleColor.Yellow);
                Console.WriteLine(".");
            }
        }

        /// <summary> Permet de récupérer le nom du joueur. </summary>
        public void ChooseName()
        {
            Console.Write("Comment souhaitez vous que l'on vous appelle ? :");
            WriteInColor(" ", ConsoleColor.Blue);
            playerName = Console.ReadLine();
        }
        /// <summary> Permet d'afficher ce que l'IA </summary>
        /// <param name="card"> La carte piochée par l'IA. </param>
        public void DisplayIADraw(Card card)
        {
            Console.Write("Ce tour ci, ");
            WriteInColor("l'IA", ConsoleColor.DarkRed);

            if (card != null)
            {
                string determinant = "un ";
                foreach (string name in femNames) if (card.name == name) determinant = "une ";
                Console.Write(" a choisi d'ajouter à sa ville " + determinant);
                WriteInColor(card.name, card.color);
                Console.WriteLine(".");
            }
            else Console.WriteLine(" a choisi de ne rien ajouter à sa ville.");

            Console.WriteLine();
            
        }
        ///<summary>Permettant d'afficher le message de fin de partie. </summary>
        ///<param name="win"> Un booléen spécifiant si le joueur a gagné. </param>
        public void DisplayEndingMessage(bool win)
        {
            if(win) foreach (string line in asciiEndMessage[0]) WriteInColor(line + "\n",ConsoleColor.Green);
            else foreach (string line in asciiEndMessage[1]) WriteInColor(line + "\n", ConsoleColor.Red);
        }

        ///<summary>Permet d'afficher les piles de cartes avec un curseur en dessous pour la sélection. </summary>
        ///<param name="selection"> l'endroit actuel où le curseur doit être affiché. </param>
        ///<param name="piles"> Un tableau contenant toutes les piles de cartes. </param>
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
            writingColor = ConsoleColor.Gray;
            Console.ForegroundColor = writingColor;
            Console.WriteLine("\n\n\n Si vous ne souhaitez pas acheter de bâtiments appuyez sur Suppr/Delete");
        }

        #region Méthodes utilitaires
        ///<summary> Permet d'écrire en couleur dans la console. </summary>
        ///<param name="toWrite"> La chaîne de caractère à écrire en couleur. </param>
        ///<param name="color"> La couleur avec laquelle écrire. </param>
        private void WriteInColor(string toWrite, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(toWrite);
            Console.ForegroundColor = writingColor;
        }
        ///<summary> Permet d'aligner une chaîne de caractère par rapport. </summary>
        ///<param name="toAlign"> La chaîne de caractères à aligner. </param>
        private string AlignString(string toAlign)
        {
            if (toAlign.Length >= maxLength) return toAlign;

            int leftPadding = (maxLength - toAlign.Length) / 2;
            int rightPadding = maxLength - toAlign.Length - leftPadding;

            return new string(' ', leftPadding) + toAlign + new string(' ', rightPadding);
        }
        #endregion Méthodes utilitaires
    }
}
