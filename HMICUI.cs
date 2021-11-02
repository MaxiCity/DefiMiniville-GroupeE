using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Miniville
{
    /** Classe qui gère toute la partie interface homme-machine. */
    class HMICUI
    {
        #region Attributs
        ///<summary> Référence du contrôleur de l'application. </summary>
        private Game ctrl;
        ///<summary> Le nom du joueur à aff </summary>
        private string playerName;

        ///<summary> Le nombre de caractères à l'intérieur d'une carte. </summary>
        private const int innerLength = 15;
        ///<summary> Le nombre de caractères composant une carte. </summary>
        private const int totalLength = 18;
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

        #endregion Attributs

        /// <summary> Enregistre la référence du contrôleur et initialise la taille de la console. </summary>
        /// <param name="_ctrl"> La référence au contrôleur. </param>
        public HMICUI(Game _ctrl)
        {
            ctrl = _ctrl;
            // 18 est
            Console.WindowWidth = 18*11;
            Console.CursorVisible = false;
        }

        #region Interaction

        /// <summary> Permet de récupérer l'index correspondant au choix dans le menu. </summary>
        /// <returns> L'index correspondant au choix du menu. </returns>
        public int ChooseMenu(int menuId)
        {
            int selection = 0;
            int nbItems = 0;

            bool choice = false;
            do
            {
                Console.Clear();
                switch (menuId)
                {
                    // Choix du mode de jeu.
                    case 0: 
                        DisplayGamemodeMenu(selection);
                        nbItems = 2;
                        break;
                    // Choix de la difficulté de l'IA.
                    case 1: DisplayDifficultyMenu(selection);
                        nbItems = 3;
                        break;
                    // Choix des conditions de victoire.
                    case 2: break;
                }

                ConsoleKeyInfo keyPressed = Console.ReadKey();
                switch (keyPressed.Key)
                {
                    case ConsoleKey.RightArrow: // Fait passer le curseur vers la droite.
                        // Si le curseur est tout à droite il revient en première position à gauche.
                        if (selection == nbItems-1) selection = 0;
                        // Sinon il passe simplement à droite.
                        else selection += 1;
                        break;

                    case ConsoleKey.LeftArrow: // Fait passer le curseur vers la gauche.
                        // Si le curseur est tout à gauche il va en dernière position à droite.
                        if (selection == 0) selection = nbItems-1;
                        // Sinon il passe simplement à gauche.
                        else selection -= 1;
                        break;
                    case ConsoleKey.Enter: // Le joueur a choisi une pile depuis laquelle piocher.
                        // On sort de la boucle.
                        choice = true;
                        break;
                }
            }
            while (!choice);

            return selection;
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

            if (humanPlayer.pieces < piles[selection].card.cost && selection >= 0)
            {
                writingColor = ConsoleColor.Red;
                Console.ForegroundColor = writingColor;
                Console.SetCursorPosition(cursorPositionX, 12);
                ErrorOverBudget(humanPlayer.pieces, piles[selection].card.cost);
                Console.SetCursorPosition(0, 0);
            }

            // Affichage du curseur.
            DisplayCardStacks(selection,piles);
            cursorPositionX = cursorOffset + totalLength * selection;
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
                        int randEasterEgg = new Random().Next(0,10);
                        if (randEasterEgg == 0)
                        {
                            Console.Clear();
                            Console.WriteLine("C'est bien d'être radin aussi !");
                            Thread.Sleep(200);
                        }

                        // On sort de la boucle.
                        choice = true;
                        break;

                    case ConsoleKey.Enter: // Le joueur a choisi une pile depuis laquelle piocher.
                        // On sort de la boucle.
                        choice = true;
                        break;
                }
                Console.WriteLine();
                if(selection >= 0)
                {
                    if (humanPlayer.pieces < piles[selection].card.cost )
                    {
                        writingColor = ConsoleColor.Red;
                        Console.ForegroundColor = writingColor;
                        ErrorOverBudget(humanPlayer.pieces, piles[selection].card.cost);
                    }
                    else
                    {
                        Console.SetCursorPosition(0, cursorPositionY + 2);
                        Console.Write(new string(' ', cursorOffset * totalLength));
                    }

                    // Clear de la partie basse de l'affichage;
                    Console.SetCursorPosition(0, cursorPositionY);
                    Console.Write(new string(' ', piles.Length * totalLength));
                    Console.SetCursorPosition(0, cursorPositionY + 1);
                    Console.Write(new string(' ', piles.Length * totalLength));


                    // Nouvelle position du curseur sous la pile suivante.
                    cursorPositionX = cursorOffset + totalLength * selection;
                    if (selection >= 0)
                    {
                        Console.SetCursorPosition(cursorPositionX, cursorPositionY);
                        WriteInColor("/\\", ConsoleColor.White);
                        Console.SetCursorPosition(cursorPositionX, cursorPositionY + 1);
                        WriteInColor(humanPlayer.pieces + "$".PadLeft(2, ' '), ConsoleColor.Yellow);
                    }
                }
                
            }
            while (!choice);
            // On renvoit le choix du joueur sous forme d'index.
            Console.Clear();
            return selection;
        }
        /// <summary> Permet de récupérer le nombre de dé que souhaite lancer le joueur. </summary>
        /// <returns> Le nombre de dés à lancer. </returns>
        public int ChooseNbDice()
        {
            int selection = 1;
            bool choice = false;
            // Enregistrement de la touche pressée par l'utilisateur.
            do
            {
                Console.Clear();
                Console.WriteLine("Combien de dés voulez vous lancer ?");
                DisplayDiceChoices(selection);
                ConsoleKeyInfo keyPressed = Console.ReadKey();
                switch (keyPressed.Key)
                {
                    case ConsoleKey.RightArrow: // Fait passer le curseur vers la droite.
                        // Si le curseur est tout à droite il revient en première position à gauche.
                        if (selection == 2) selection = 1;
                        // Sinon il passe simplement à droite.
                        else selection += 1;
                        break;

                    case ConsoleKey.LeftArrow: // Fait passer le curseur vers la gauche.
                        // Si le curseur est tout à gauche il va en dernière position à droite.
                        if (selection == 1) selection = 2;
                        // Sinon il passe simplement à gauche.
                        else selection -= 1;
                        break;

                    case ConsoleKey.Enter: // Le joueur a choisi une pile depuis laquelle piocher.
                        // On sort de la boucle.
                        choice = true;
                        break;
                }
            }
            while (!choice);

            return selection;
        }
        /// <summary> Permet de récupérer le nom du joueur. </summary>
        public void ChooseName()
        {
            Console.Write("Comment souhaitez vous que l'on vous appelle ? :");
            WriteInColor(" ", ConsoleColor.Blue);
            playerName = Console.ReadLine();
        }

        #endregion Interaction

        #region Affichage de cartes

        ///<summary> Permet d'afficher les cartes sur le plateau des deux joueurs. </summary>
        ///<param name="players"> Un tableau contenant tous les joueurs. </param>
        ///<param name="playerTurn"> L'index du joueur à qui c'est le tour. </param>
        ///<param name="dieRoll"> le résultat du lancé de dé, si il n'est pas spécifié c'est qu'on souhaite tout afficher. </param>
        public void DisplayCities(Player[] players, int playerTurn = 0, int dieRoll = 0, int[] dieRolls = null)
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
                              (dieRoll == c.dieCondition[0] || dieRoll == c.dieCondition[1] && (playerTurn == playerIndex && (c.color.Equals(ConsoleColor.Green) 
                              || c.color.Equals(ConsoleColor.Cyan))|| (playerTurn != playerIndex && (c.color.Equals(ConsoleColor.Red) || c.color.Equals(ConsoleColor.Cyan)))));
                        
                        switch (i)
                        {
                            case 0: Console.Write(sep); break;
                            case 1:
                                if (show)
                                {
                                    Console.Write("| ");
                                    if(c.dieCondition[0] == c.dieCondition[1]) WriteInColor($" {c.dieCondition[0]} ", ConsoleColor.White);
                                    else WriteInColor($"{c.dieCondition[0]}-{c.dieCondition[1]}", ConsoleColor.White);
                                    Console.Write(" |");
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

                        if (dieRolls[1] != 0)
                        {
                            Console.Write("          " + asciiDiceFaces[dieRolls[0] - 1][i]);
                            Console.Write("  " + asciiDiceFaces[dieRolls[1] - 1][i]);
                        }
                        else Console.Write("          " + asciiDiceFaces[dieRoll - 1][i]);

                    }
                    if (i == nbLinesCity - 1 && dieRoll == 0)
                    {
                        writingColor = ConsoleColor.Gray;
                        Console.ForegroundColor = writingColor;
                        if (playerTurn == playerIndex)
                        {
                            Console.Write("      Vos pièces : ");
                            WriteInColor($"{players[0].pieces }$", ConsoleColor.Yellow);
                        }
                        else
                        {
                            Console.Write("      Les pièces adverses : ");
                            WriteInColor($"{players[1].pieces }$", ConsoleColor.Yellow);
                        }
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("\n\n\n\n");
            }
            Console.WriteLine();
            writingColor = ConsoleColor.Gray;
            Console.ForegroundColor = writingColor;
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
                                if (piles[j].card.dieCondition[0] == piles[j].card.dieCondition[1]) WriteInColor(AlignString("[" + piles[j].card.dieCondition[0] + "]"), ConsoleColor.White);
                                else WriteInColor(AlignString("[" + piles[j].card.dieCondition[0] + "-" + piles[j].card.dieCondition[1] + "]"), ConsoleColor.White);
                                Console.Write($"|");

                            }
                            else Console.Write(space);
                            break;
                        case 2: // Nom de la carte
                            if (piles[j].nbCard > 0)
                            {
                                Console.Write("|");
                                WriteInColor(AlignString(piles[j].card.name), ConsoleColor.White);
                                Console.Write("|");
                            }
                            else Console.Write(space);
                            break;
                        case 3: Console.Write(space); break;
                        case 4: // Description de l'effet.
                            if (piles[j].nbCard > 0)
                            {
                                Console.Write("|");
                                WriteInColor(AlignString(piles[j].card.description[0]), ConsoleColor.Yellow);
                                Console.Write("|");
                            }
                            else
                            {
                                Console.Write("|");
                                Console.Write(AlignString("Empty"));
                                Console.Write("|");
                            }
                            break;
                        case 5: // Deuxième ligne de l'activation.
                            if (piles[j].nbCard > 0)
                            {
                                Console.Write("|");
                                WriteInColor(AlignString(piles[j].card.description[1]), ConsoleColor.Gray);
                                Console.Write("|");
                            }
                            else Console.Write(space);
                            break;
                        case 6: // Deuxième ligne de l'activation.
                            if (piles[j].nbCard > 0)
                            {
                                Console.Write("|");
                                WriteInColor(AlignString(piles[j].card.description[2]), ConsoleColor.Gray);
                                Console.Write("|");
                            }
                            else Console.Write(space);
                            break;
                        case 7: Console.Write(space); break;
                        case 8:
                            if (piles[j].nbCard > 0)
                            {
                                Console.Write("|");
                                WriteInColor($" { piles[j].card.cost }$", ConsoleColor.Yellow);
                                Console.Write("            |");
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

        #endregion Affichage de cartes
        
        #region Affichage de messages

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
                else msg = " a perdu";

                Console.Write(entryMsg);
                WriteInColor("l'IA", ConsoleColor.DarkRed);
                Console.Write(msg);
                WriteInColor((""+IATotal).Replace('-',' ') + "$", ConsoleColor.Yellow);
                Console.WriteLine(".");
            }
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

        #endregion Affichage de messages

        #region Affichage des menus

        /// <summary> Permet d'afficher le menu de sélection du nombre de dés. </summary>
        /// <param name="selection"> Le choix actuellement sélectionné. </param>
        private void DisplayDiceChoices(int selection)
        {
            for (int i = 0; i < asciiDiceFaces[0].Length; i++)
            {
                if (selection == 1) WriteInColor(asciiDiceFaces[0][i], ConsoleColor.Green);
                else Console.Write(asciiDiceFaces[0][i]);

                Console.Write("  |  ");

                if (selection == 2)
                {
                    WriteInColor(asciiDiceFaces[0][i], ConsoleColor.Green);
                    WriteInColor("  " + asciiDiceFaces[1][i], ConsoleColor.Green);
                }
                else
                {
                    Console.Write(asciiDiceFaces[0][i]);
                    Console.Write("  " + asciiDiceFaces[1][i]);
                }
                Console.WriteLine();
            }
        }

        /// <summary> Permet d'afficher le menu du choix du mode de jeu. </summary>
        /// <param name="selection"> Le menu actuel sélectionné. </param>
        private void DisplayGamemodeMenu(int selection)
        {
            string sep = "+---------------------+";
            string space = "|                     |";
            for (int i=0; i<10; i++)
            {
                for(int j=0; j<2; j++)
                {
                    if (selection == j)
                    {
                        writingColor = ConsoleColor.Green;
                        Console.ForegroundColor = writingColor;
                    }

                    switch (i)
                    {
                        case 0: Console.Write(sep); break;
                        case 1:
                            if (j == 0)
                            {
                                Console.Write("|    ");
                                if (selection == j) WriteInColor("Mode Standard", ConsoleColor.Yellow);
                                else Console.Write("Mode Standard");
                                Console.Write("    |");
                            }
                            else
                            {
                                Console.Write("|  ");
                                if (selection == j) WriteInColor("Mode Personnalisé", ConsoleColor.Yellow);
                                else Console.Write("Mode Personnalisé");
                                Console.Write("  |");
                            }
                            break;
                        case 2: Console.Write(sep); break;
                        case 3:
                            if (j == 0)
                            {
                                Console.Write("|   ");
                                if (selection == j) WriteInColor("Mode de jeu sans", ConsoleColor.White);
                                else Console.Write("Mode de jeu sans");
                                Console.Write("  |");
                            }
                            else
                            {
                                Console.Write("|   ");
                                if (selection == j) WriteInColor("Mode de jeu avec", ConsoleColor.White);
                                else Console.Write("Mode de jeu avec");
                                Console.Write("  |");
                            }
                            break;
                        case 4:
                            if (j == 0)
                            {
                                Console.Write("|     ");
                                if (selection == j) WriteInColor("aucun bonus.", ConsoleColor.White);
                                else Console.Write("aucun bonus.");
                                Console.Write("    |");
                            }
                            else
                            {
                                Console.Write("| ");
                                if (selection == j) WriteInColor("de nouvelles cartes", ConsoleColor.White);
                                else Console.Write("de nouvelles cartes");
                                Console.Write(" |");
                            }
                            break;

                        case 5:
                            if (j == 0) Console.Write(space);
                            else
                            {
                                Console.Write("|   ");
                                if (selection == j) WriteInColor("et un dé en plus.", ConsoleColor.White);
                                else Console.Write("et un dé en plus.");
                                Console.Write(" |");
                            }
                            break;
                        case 6: Console.Write(sep); break;
                    }
                    writingColor = ConsoleColor.Gray;
                    Console.ForegroundColor = writingColor;
                    Console.Write(" ");
                }
                Console.WriteLine();
            } 
        }

        /// <summary> Permet d'afficher le menu de choix de la difficulté. </summary>
        /// <param name="selection"> Le menu actuellement sélectionné. </param>
        private void DisplayDifficultyMenu(int selection)
        {
            string sep = "+---------------------+";
            string space = "|                     |";
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (selection == j)
                    {
                        writingColor = ConsoleColor.Green;
                        Console.ForegroundColor = writingColor;
                    }

                    switch (i)
                    {
                        case 0: Console.Write(sep); break;
                        case 1:
                            if (j == 0)
                            {
                                Console.Write("|    ");
                                if (selection == j) WriteInColor("   Facile    ", ConsoleColor.Yellow);
                                else Console.Write("   Facile    ");
                                Console.Write("    |");
                            }
                            else if (j == 1)
                            {
                                Console.Write("|    ");
                                if (selection == j) WriteInColor("    Moyen    ", ConsoleColor.Yellow);
                                else Console.Write("    Moyen    ");
                                Console.Write("    |");
                            }
                            else
                            {
                                Console.Write("|    ");
                                if (selection == j) WriteInColor("  Difficile  ", ConsoleColor.Yellow);
                                else Console.Write("  Difficile  ");
                                Console.Write("    |");
                            }
                            break;
                        case 2: Console.Write(sep); break;
                        case 3:
                            if (j == 0) Console.Write(space);
                            else if(j == 1)
                            {
                                Console.Write("| ");
                                if (selection == j) WriteInColor("   L'IA aura un    ", ConsoleColor.White);
                                else Console.Write("   L'IA aura un    ");
                                Console.Write(" |");
                            }
                            else
                            {
                                Console.Write("| ");
                                if (selection == j) WriteInColor(" L'IA sera capable ", ConsoleColor.White);
                                else Console.Write(" L'IA sera capable ");
                                Console.Write(" |");
                            }
                            break;
                        case 4:
                            if (j == 0)
                            {
                                Console.Write("| ");
                                if (selection == j) WriteInColor("   L'IA fera des   ", ConsoleColor.White);
                                else Console.Write("   L'IA fera des   ");
                                Console.Write(" |");
                            }
                            else if (j == 1)
                            {
                                Console.Write("| ");
                                if (selection == j) WriteInColor("comportement qui ne", ConsoleColor.White);
                                else Console.Write("comportement qui ne");
                                Console.Write(" |");
                            }
                            else
                            {
                                Console.Write("| ");
                                if (selection == j) WriteInColor(" de s'adapter à la ", ConsoleColor.White);
                                else Console.Write(" de s'adapter à la ");
                                Console.Write(" |");
                            }
                            break;
                        case 5:
                            if (j == 0)
                            {
                                Console.Write("| ");
                                if (selection == j) WriteInColor(" choix aléatoires. ", ConsoleColor.White);
                                else Console.Write(" choix aléatoires. ");
                                Console.Write(" |");
                            }
                            else if(j == 1)
                            {
                                Console.Write("| ");
                                if (selection == j) WriteInColor(" changera pas tout ", ConsoleColor.White);
                                else Console.Write(" changera pas tout ");
                                Console.Write(" |");
                            }
                            else
                            {
                                Console.Write("| ");
                                if (selection == j) WriteInColor(" situation afin de ", ConsoleColor.White);
                                else Console.Write(" situation afin de ");
                                Console.Write(" |");
                            }
                            break;
                        case 6:
                            if (j == 0) Console.Write(space);
                            else if (j == 1)
                            {
                                Console.Write("| ");
                                if (selection == j) WriteInColor("  au long du jeu.  ", ConsoleColor.White);
                                else Console.Write("  au long du jeu.  ");
                                Console.Write(" |");
                            }
                            else
                            {
                                Console.Write("| ");
                                if (selection == j) WriteInColor(" gérer vos actions ", ConsoleColor.White);
                                else Console.Write(" gérer vos actions ");
                                Console.Write(" |");
                            }
                            break;
                        case 7: Console.Write(sep); break;
                    }
                    writingColor = ConsoleColor.Gray;
                    Console.ForegroundColor = writingColor;
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }

        #endregion Affichage des menus

        #region Méthodes utilitaires

        /// <summary> Permet d'éfficher un message au joueur pour lui indiquer qu'il ne peut pas acheter la carte. </summary>
        /// <param name="playerCoin"> Les pièces du joueur. </param>
        /// <param name="cardCost"> Le coût de la carte. </param>
        private void ErrorOverBudget(int playerCoin, int cardCost)
        {
            Console.Write(" Vous ne pouvez pas acheter ce bâtiment, il vous manque ");
            WriteInColor(cardCost - playerCoin + "$", ConsoleColor.Yellow);
            Console.WriteLine(" pour qu'il soit dans votre budget.");
        }
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
            if (toAlign.Length >= innerLength) return toAlign;

            int leftPadding = (innerLength - toAlign.Length) / 2;
            int rightPadding = innerLength - toAlign.Length - leftPadding;

            return new string(' ', leftPadding) + toAlign + new string(' ', rightPadding);
        }

        #endregion Méthodes utilitaires
    }
}
