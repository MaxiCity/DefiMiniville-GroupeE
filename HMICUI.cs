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
        ///<summary> Le nom du joueur à afficher. </summary>
        private string playerName;
        ///<summary> Le nom de l'IA à afficher. </summary>
        private string IAName;
        /// <summary> Le nombre de pièces à ammasser pour gagner. </summary>
        private string nbPiecesToWin = "20";
        /// <summary> Le nombre de types différents de carte dans la partie. </summary>
        private string nbCardTypes;
        ///<summary> Un booléen renseignant si le jeu est en mode expert ou non. </summary>
        private bool expertMode = false;
        /// <summary> Le nombre de cartes différentes que possède le joueur. </summary>
        private string playerCardTypes = "2";
        /// <summary> Le nombre de cartes différentes que possède l'IA. </summary>
        private string IACardTypes = "2";
        ///<summary> Le nombre de caractères à l'intérieur d'une carte. </summary>
        private const int innerLength = 15;
        ///<summary> Le nombre de caractères composant une carte. </summary>
        private const int totalLength = 18;
        ///<summary> Les nombres de lignes lorsqu'on affiche les piles de cartes. </summary>
        private int nbLines = 14;
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
        private List<string[]> asciiEndMessage = new List<string[]>() { new string[] {@" __     __  __              __                __",
                                                                                      @"/  |   /  |/  |            /  |              /  |",
                                                                                      @"$$ |   $$ |$$/   _______  _$$ |_     ______  $$/   ______    ______  ",
                                                                                      @"$$ |   $$ |/  | /       |/ $$   |   /      \ /  | /      \  /      \ ",
                                                                                      @"$$  \ /$$/ $$ |/$$$$$$$/ $$$$$$/   /$$$$$$  |$$ |/$$$$$$  |/$$$$$$  |",
                                                                                      @" $$  /$$/  $$ |$$ |        $$ | __ $$ |  $$ |$$ |$$ |  $$/ $$    $$ |",
                                                                                      @"  $$ $$/   $$ |$$ \_____   $$ |/  |$$ \__$$ |$$ |$$ |      $$$$$$$$/",
                                                                                      @"   $$$/    $$ |$$       |  $$  $$/ $$    $$/ $$ |$$ |      $$       |",
                                                                                      @"    $/     $$/  $$$$$$$/    $$$$/   $$$$$$/  $$/ $$/        $$$$$$$/ "},
                                                                        new string[] {@" _______              ______           __    __",
                                                                                      @"/       \            /      \         /  |  /  |",
                                                                                      @"$$$$$$$  |  ______  /$$$$$$  |______  $$/  _$$ |_     ______  ",
                                                                                      @"$$ |  $$ | /      \ $$ |_ $$//      \ /  |/ $$   |   /      \ ",
                                                                                      @"$$ |  $$ |/$$$$$$  |$$   |   $$$$$$  |$$ |$$$$$$/   /$$$$$$  |",
                                                                                      @"$$ |  $$ |$$    $$ |$$$$/    /    $$ |$$ |  $$ | __ $$    $$ |",
                                                                                      @"$$ |__$$ |$$$$$$$$/ $$ |    /$$$$$$$ |$$ |  $$ |/  |$$$$$$$$/",
                                                                                      @"$$    $$/ $$       |$$ |    $$    $$ |$$ |  $$  $$/ $$       |",
                                                                                      @"$$$$$$$/   $$$$$$$/ $$/      $$$$$$$/ $$/    $$$$/   $$$$$$$/"}
        };
        /// <summary> Les lignes qui composent le titre du jeu. </summary>
        private string[] asciiTitle = new string[] {@"  __       __                      __         ______   __    __              ",
                                                    @"/  \     /  |                    /  |       /      \ /  |  /  |              ",
                                                    @"$$  \   /$$ |  ______   __    __ $$/       /$$$$$$  |$$/  _$$ |_    __    __ ",
                                                    @"$$$  \ /$$$ | /      \ /  \  /  |/  |      $$ |  $$/ /  |/ $$   |  /  |  /  |",
                                                    @"$$$$  /$$$$ | $$$$$$  |$$  \/$$/ $$ |      $$ |      $$ |$$$$$$/   $$ |  $$ |",
                                                    @"$$ $$ $$/$$ | /    $$ | $$  $$<  $$ |      $$ |   __ $$ |  $$ | __ $$ |  $$ |",
                                                    @"$$ |$$$/ $$ |/$$$$$$$ | /$$$$  \ $$ |      $$ \__/  |$$ |  $$ |/  |$$ \__$$ |",
                                                    @"$$ | $/  $$ |$$    $$ |/$$/ $$  |$$ |      $$    $$/ $$ |  $$  $$/ $$    $$ |",
                                                    @"$$/      $$/  $$$$$$$/ $$/   $$/ $$/        $$$$$$/  $$/    $$$$/   $$$$$$$ |",
                                                    @"                                                                   /  \__$$ |",
                                                    @"                                                                   $$    $$/ ",
                                                    @"                                                                    $$$$$$/  "};
        /// <summary> Les lignes qui composent la description des règles. </summary>
        private string[] ruleLines = new string[] {  "Le but de MaxiCity est de construire une ville qui soit rentable.",
                                                     "Le joueur qui atteint un nombre de pièces donné en premier gagne la partie.",
                                                     "Le nombre de pièces changera en fonction de la longueur de jeu souhaitée et",
                                                     "la partie pourra être plus difficile en fonction de l'adversaire IA choisi.", " ",
                                                     "Pour faire fortune dans l'immobilier, chaque joueur va pouvoir acheter des",
                                                     "avec ses pièces des bâtiments sous forme de cartes qu'il ajoutera à sa ville",
                                                     "tout au long de la partie. Ces bâtiments produiront des pièces lorsque leur",
                                                     "valeur de dé sera activée, c'est à dire quand le dé de l'un des joueur tombe",
                                                     "sur la face égale à cette valeur.", " ",
                                                     "Il existe plusieurs types de cartes, les types étant indiqués par la couleur",
                                                     "de la carte. Les cartes vertes s'activent uniquement avec vos dés pendant",
                                                     "votre tour, les rouges uniquement pendant le tour de votre adversaire et les",
                                                     "bleues peuvent être activées à tous les tours.", " " };

        #endregion Attributs

        /// <summary> Enregistre la référence du contrôleur et initialise la taille de la console. </summary>
        public HMICUI()
        {
            Console.SetWindowPosition(0, 0);
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            Console.CursorVisible = false;
        }
        
        #region Interaction 

        /// <summary> Permet de récupérer l'index correspondant au choix dans le menu. </summary>
        /// <returns> L'index correspondant au choix du menu. </returns>
        public int ChooseMenu(int menuId)
        {
            int selection = 0;

            bool choice = false;
            List<string[]> items;
            switch(menuId)
            {
                case 0: items = new List<string[]>() { new string[] {"    Mode Standard    ", "   Mode de jeu sans  ", "     aucun bonus.    ", "                     "},
                                                       new string[] {"  Mode Personnalisé  ", "   Mode de jeu avec  ", " de nouvelles cartes ", "  et un dé en plus.  "}};
                    break;
                case 1: items = new List<string[]>() { new string[] {"   Adversaire Billy  ", "                     ", "    Billy fait des   ", "  choix aléatoires.  ", "                     "},
                                                       new string[] {" Adversaire Gertrude ", " Gertrude tentera de ", "  limiter ses achats ", " et de garder toute  ", "     sa fortune.     "},
                                                       new string[] {" Adversaire Donatien ", "  Donatien achètera  ", "   dès qu'il pourra  ", " et essaiera d'avoir ", "  toutes les cartes. "}};
                    break;
                case 2: items = new List<string[]>() { new string[] {"        Rapide       ", "                     ", " Pour gagner il faut ", "  avoir amassé 10$.  ", "                     "},
                                                       new string[] {"        Normal       ", "                     ", " Pour gagner il faut ", "  avoir amassé 20$.  ", "                     "},
                                                       new string[] {"        Longue       ", "                     ", " Pour gagner il faut ", "  avoir amassé 30$.  ", "                     "},
                                                       new string[] {"        Expert       ", " Pour gagner il faut ", " avoir amassé 20$ et ", "  avoir chaque type  ", "   de carte du jeu.  "}};
            break;
                default: items = new List<string[]>(); break;
            }

            int nbItems = items.Count;

            do
            {
                Console.Clear();
                DisplayMenu(selection, items);
                ConsoleKeyInfo keyPressed = Console.ReadKey();
                switch (keyPressed.Key)
                {
                    case ConsoleKey.RightArrow or ConsoleKey.D: // Fait passer le curseur vers la droite.
                        // Si le curseur est tout à droite il revient en première position à gauche.
                        if (selection == nbItems-1) selection = 0;
                        // Sinon il passe simplement à droite.
                        else selection += 1;
                        break;

                    case ConsoleKey.LeftArrow or ConsoleKey.Q: // Fait passer le curseur vers la gauche.
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

            switch(menuId)
            {
                case 0: nbCardTypes = selection == 0? "8":"11"; break;
                case 1:
                    switch (selection)
                    {
                        case 0: IAName = "Billy"; break;
                        case 1: IAName = "Gertrude"; break;
                        case 2: IAName = "Donatien"; break;
                    }
                    break;
                case 2:
                    if (selection == 3)
                    {
                        nbPiecesToWin = "20";
                        expertMode = true;
                    }
                    else nbPiecesToWin = " " + (selection + 1) * 10;
                    break;
            }

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
            int cursorPositionY = 14;
            int cursorOffset = 8;

            if (humanPlayer.pieces < piles[selection].card.cost && selection >= 0)
            {
                writingColor = ConsoleColor.Red;
                Console.ForegroundColor = writingColor;
                Console.SetCursorPosition(cursorPositionX, cursorPositionY+2);
                ErrorOverBudget(humanPlayer.pieces, piles[selection].card.cost);
                Console.SetCursorPosition(0, 0);
            }

            // Affichage du curseur.
            DisplayCardStacks(piles);
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
                    case ConsoleKey.RightArrow or ConsoleKey.D: // Fait passer le curseur vers la droite.
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

                    case ConsoleKey.LeftArrow or ConsoleKey.Q:// Fait passer le curseur vers la gauche.
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

        #region Affichage des cartes

        ///<summary> Permet d'afficher les cartes sur le plateau des deux joueurs. </summary>
        ///<param name="players"> Un tableau contenant tous les joueurs. </param>
        ///<param name="playerTurn"> L'index du joueur à qui c'est le tour. </param>
        ///<param name="dieRoll"> le résultat du lancé de dé, si il n'est pas spécifié c'est qu'on souhaite tout afficher. </param>
        public void DisplayCities(Player[] players, int playerTurn = 0, int dieResult = 0, int[] dieRolls = null)
        {
            string sep = "+-----+";
            string space = "|     |";

            bool show;
            for (int playerIndex = 1; playerIndex >= 0; playerIndex--)
            {
                // Si on doit afficher la ville de l'IA.
                if(playerIndex == 0)
                {
                    for(int i=0; i < players[playerIndex].city[0].artwork.Length; i++)
                    {
                        foreach (Card c in players[playerIndex].city) WriteInColor(c.artwork[i]+" ", c.color);
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                }

                for (int i = 0; i < nbLinesCity; i++)
                {
                    foreach (Card c in players[playerIndex].city)
                    {
                        writingColor = c.color;
                        Console.ForegroundColor = writingColor;
                        
                        // Condition selon laquelle on montre l'effet d'une carte ou non.
                        show = dieResult == 0 ||
                              ((dieResult == c.dieCondition[0] || dieResult == c.dieCondition[1]) && (playerTurn == playerIndex && (c.color.Equals(ConsoleColor.Green) 
                              || c.color.Equals(ConsoleColor.Cyan))|| (playerTurn != playerIndex && (c.color.Equals(ConsoleColor.Red) || c.color.Equals(ConsoleColor.Cyan)))));
                        switch (i)
                        {
                            case 0: Console.Write(sep); break;
                            case 1:
                                if (show)
                                {
                                    Console.Write("|");
                                    if (c.dieCondition[0] == c.dieCondition[1])
                                    {
                                        if (c.dieCondition[0] < 10) Console.Write(" ");
                                        WriteInColor($" {c.dieCondition[0]}  ", ConsoleColor.White);
                                    }
                                    else
                                    {
                                        if (c.dieCondition[0] < 10) Console.Write(" ");
                                        WriteInColor($"{c.dieCondition[0]}-{c.dieCondition[1]}", ConsoleColor.White);
                                        if (c.dieCondition[1] < 10) Console.Write(" ");
                                    }
                                    Console.Write("|");
                                }
                                else Console.Write(space);
                                break;
                            case 2:
                                Console.Write("| ");
                                WriteInColor(c.name.Substring(0, 3), ConsoleColor.White);
                                Console.Write(" |");
                                break;
                            case 3:
                                if (show)
                                {
                                    Console.Write("| ");
                                    if (c.color.Equals(ConsoleColor.Red)) WriteInColor($"-{c.moneyToEarn}$", ConsoleColor.Yellow);
                                    else WriteInColor($"+{c.moneyToEarn}$", ConsoleColor.Yellow);
                                    Console.Write(" |");
                                }
                                else Console.Write(space);
                                break;
                            case 4: Console.Write(sep); break;
                        }
                        Console.Write(" ");
                    }
                    if (playerIndex == playerTurn && dieResult > 0)
                    {
                        writingColor = ConsoleColor.White;
                        Console.ForegroundColor = writingColor;

                        if (dieRolls[1] != 0)
                        {
                            Console.Write("          " + asciiDiceFaces[dieRolls[0] - 1][i]);
                            Console.Write("  " + asciiDiceFaces[dieRolls[1] - 1][i]);
                        }
                        else Console.Write("          " + asciiDiceFaces[dieResult - 1][i]);

                    }
                    if (i == nbLinesCity - 1 && dieResult == 0)
                    {
                        writingColor = ConsoleColor.Gray;
                        Console.ForegroundColor = writingColor;
                        ConsoleColor tmpColor = ConsoleColor.Yellow;
                        if (playerTurn == playerIndex)
                        {
                            Console.Write("      Vos pièces : ");
                            if (players[0].pieces >= int.Parse(nbPiecesToWin)) tmpColor = ConsoleColor.Green;
                            WriteInColor($"{players[0].pieces }$ / {nbPiecesToWin} ", tmpColor);
                            if (expertMode)
                            {
                                if (playerCardTypes == nbCardTypes) tmpColor = ConsoleColor.Green;
                                else tmpColor = writingColor;
                                Console.Write("|");
                                WriteInColor($" {playerCardTypes} / {nbCardTypes} cartes",tmpColor);
                            }
                        }
                        else
                        {
                            Console.Write("      Les pièces adverses : ");
                            if (players[1].pieces >= int.Parse(nbPiecesToWin)) tmpColor = ConsoleColor.Green;
                            WriteInColor($"{players[1].pieces }$ / {nbPiecesToWin} ", tmpColor);
                            if (expertMode)
                            {
                                if (IACardTypes == nbCardTypes) tmpColor = ConsoleColor.Green;
                                else tmpColor = writingColor;
                                Console.Write("|");
                                WriteInColor($" {IACardTypes} / {nbCardTypes} cartes", tmpColor);
                            }
                        }
                    }
                    Console.WriteLine();
                }
                // Si on doit afficher la ville de l'IA.
                if (playerIndex == 1)
                {
                    for (int i = 0; i < players[playerIndex].city[0].artwork.Length; i++)
                    {
                        foreach (Card c in players[playerIndex].city) WriteInColor(c.artwork[i]+" ", c.color);
                        Console.WriteLine();
                    }
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
        private void DisplayCardStacks(Pile[] piles)
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
                                if (piles[j].card.dieCondition[0] == piles[j].card.dieCondition[1]) WriteInColor(AlignString("[" + piles[j].card.dieCondition[0] + "]", innerLength), ConsoleColor.White);
                                else WriteInColor(AlignString("[" + piles[j].card.dieCondition[0] + "-" + piles[j].card.dieCondition[1] + "]", innerLength), ConsoleColor.White);
                                Console.Write($"|");

                            }
                            else Console.Write(space);
                            break;
                        case 2: // Nom de la carte
                            if (piles[j].nbCard > 0)
                            {
                                Console.Write("|");
                                WriteInColor(AlignString(piles[j].card.name, innerLength),ConsoleColor.White);
                                Console.Write("|");
                            }
                            else Console.Write(space);
                            break;
                        case 3: // 1ère ligne d'artwork.
                            if (piles[j].nbCard > 0)
                            {
                                Console.Write("|");
                                Console.Write(AlignString(piles[j].card.artwork[0], innerLength));
                                Console.Write("|");
                            }
                            else Console.Write(space);
                            break;
                        case 4:
                            if (piles[j].nbCard > 0)
                            {
                                Console.Write("|");
                                Console.Write(AlignString(piles[j].card.artwork[1], innerLength));
                                Console.Write("|");
                            }
                            else Console.Write(space);
                            break;
                        case 5:
                            if (piles[j].nbCard > 0)
                            {
                                Console.Write("|");
                                Console.Write(AlignString(piles[j].card.artwork[2], innerLength));
                                Console.Write("|");
                            }
                            else Console.Write(space);
                            break;
                        case 6:
                            if (piles[j].nbCard > 0)
                            {
                                Console.Write("|");
                                Console.Write(AlignString(piles[j].card.artwork[3], innerLength));
                                Console.Write("|");
                            }
                            else
                            {
                                Console.Write("|");
                                Console.Write(AlignString("Empty", innerLength), innerLength);
                                Console.Write("|");
                            }
                            break;
                        case 7: Console.Write(space); break;
                        case 8: // Description de l'effet.
                            if (piles[j].nbCard > 0)
                            {
                                Console.Write("|");
                                WriteInColor(AlignString(piles[j].card.description[0], innerLength), ConsoleColor.Yellow);
                                Console.Write("|");
                            }
                            else Console.Write(space);
                            break;
                        case 9: // Deuxième ligne de l'activation.
                            if (piles[j].nbCard > 0)
                            {
                                Console.Write("|");
                                WriteInColor(AlignString(piles[j].card.description[1], innerLength), ConsoleColor.Gray);
                                Console.Write("|");
                            }
                            else Console.Write(space);
                            break;
                        case 10: // Deuxième ligne de l'activation.
                            if (piles[j].nbCard > 0)
                            {
                                Console.Write("|");
                                WriteInColor(AlignString(piles[j].card.description[2], innerLength), ConsoleColor.Gray);
                                Console.Write("|");
                            }
                            else Console.Write(space);
                            break;
                        case 11: Console.Write(space); break;
                        case 12:
                            if (piles[j].nbCard > 0)
                            {
                                Console.Write("|");
                                WriteInColor($" { piles[j].card.cost }$", ConsoleColor.Yellow);
                                Console.Write("            |");
                            }
                            else Console.Write(space);
                            break;
                        case 13: Console.Write(sep); break;
                    }
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
            writingColor = ConsoleColor.Gray;
            Console.ForegroundColor = writingColor;
            Console.WriteLine("\n\n\n Si vous ne souhaitez pas acheter de bâtiments appuyez sur Suppr/Delete");
        }

        #endregion Affichage des cartes

        #region Affichage de messages

        public void DisplayTitle()
        {
            int longestLength = 0;
            foreach (string line in asciiTitle) if (longestLength < line.Length) longestLength = line.Length;
            string sep = "+" + new string('-', longestLength + 2) + "+";

            string[] authorNames = { "ALAIN Arthur", "COMETTO Émile", " PINEDA Joris", "THIRIOT Virgile" };

            writingColor = ConsoleColor.White;
            Console.ForegroundColor = writingColor;

            Console.WriteLine(sep);
            foreach(string line in asciiTitle)
            {
                Console.Write("| ");
                WriteInColor(line, ConsoleColor.Yellow);
                Console.WriteLine(" |");
            }
            Console.WriteLine(sep);
            Console.Write("|");
            foreach(string name in authorNames)
            {
                WriteInColor(AlignString(name, longestLength/4), ConsoleColor.Magenta);
                Console.Write("|");
            }
            Console.WriteLine("\n" + sep);

            Console.WriteLine("| " + AlignString(" ", longestLength) + " |");
            foreach (string line in ruleLines)
            {
                Console.Write("| ");
                WriteInColor(AlignString(line, longestLength), ConsoleColor.White);
                Console.WriteLine(" |");
            }
            Console.WriteLine(sep);
            Console.Write("| ");
            WriteInColor(AlignString("Appuyez sur Entrée pour continuer ->", longestLength), ConsoleColor.Yellow);
            Console.WriteLine(" |");
            Console.WriteLine(sep);
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
                WriteInColor(IAName, ConsoleColor.DarkRed);
                Console.WriteLine(" n'a rien gagné ni perdu. ");
            }
            else
            {
                if (IATotal > 0) msg = " a gagné ";
                else msg = " a perdu";

                Console.Write(entryMsg);
                WriteInColor(IAName, ConsoleColor.DarkRed);
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
            WriteInColor(IAName, ConsoleColor.DarkRed);

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
        /// <param name="selection"> Le menu actuellement sélectionné. </param>
        private void DisplayMenu(int selection, List<string[]> items)
        {
            string sep = "+---------------------+";

            for (int i=0; i < items[0].Length+3; i++)
            {
                for(int j=0; j<items.Count; j++)
                {
                    if (selection == j)
                    {
                        writingColor = ConsoleColor.Green;
                        Console.ForegroundColor = writingColor;
                    }

                    if (i == 0 || i == 2 || i == items[0].Length + 2) Console.Write(sep);
                    else if(i == 1)
                    {
                        Console.Write("|");
                        if (selection == j) WriteInColor(items[j][0], ConsoleColor.Yellow);
                        else Console.Write(items[j][0]);
                        Console.Write("|");
                    }
                    else
                    {
                        Console.Write("|");
                        if (selection == j) WriteInColor(items[j][i-2], ConsoleColor.White);
                        else Console.Write(items[j][i-2]);
                        Console.Write("|");
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
        private string AlignString(string toAlign, int length)
        {
            if (toAlign.Length >= length) return toAlign;

            int leftPadding = (length - toAlign.Length) / 2;
            int rightPadding = length - toAlign.Length - leftPadding;

            return new string(' ', leftPadding) + toAlign + new string(' ', rightPadding);
        }

        #endregion Méthodes utilitaires

        public void SetWinConditionsState(int _playerCardTypes, bool isPlayer)
        {
            if(isPlayer) playerCardTypes = "" + _playerCardTypes;
            else IACardTypes = "" + _playerCardTypes;
        }
    }
}
