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
        private ConsoleColor writingColor = ConsoleColor.Gray;

        string[] cardActCost = { "1", "1", "2", "3", "4", "5", "5", "6" };
        ConsoleColor[] cardColors = { ConsoleColor.Cyan, ConsoleColor.Cyan, ConsoleColor.Green, ConsoleColor.Red, ConsoleColor.Green,
                                      ConsoleColor.Cyan, ConsoleColor.Red, ConsoleColor.Cyan };
        string[] cardNames = { "Champs de blé", "Ferme", "Boulangerie", "Café", "Superette", "Forêt", "Restaurant", "Stade" };
        string[] cardGains = { "1", "1", "2", "1", "3", "1", "2", "4" };
        string[] cardCosts = { "1", "2", "1", "2", "2", "2", "4", "6" };


        public HMICUI(Game ctrl)
        {
            this.ctrl = ctrl;
        }

        public int Choose(int selection)
        {
            DisplayCardStacks();
            return 1;
        }

        public void DisplayCardStacks()
        {
            string sep = "+---------------+";
            string space = "|               |";

            for (int i = 0; i < cardNames.Length; i++)
            {
                for(int j=0; j< cardNames.Length; j++)
                {
                    switch(j)
                    {
                        case 0:
                            writingColor = cardColors[i];
                            Console.ForegroundColor = writingColor;
                            Console.Write(sep);
                            break;
                        case 1:
                            Console.Write("|");
                            WriteInColor(AlignString(cardActCost[i]), ConsoleColor.White);
                            Console.Write($"|");
                            break;
                        case 2:
                            Console.Write($"| ");
                            WriteInColor($"+{ cardGains[i]}", ConsoleColor.Yellow);
                            Console.Write("        ");
                            WriteInColor($" {cardCosts[i]}$", ConsoleColor.Yellow);
                            Console.WriteLine(" |");
                            Console.WriteLine(space);
                            break;
                        case 3:
                            Console.Write("|");
                            WriteInColor(AlignString(cardNames[i]), ConsoleColor.White);
                            Console.WriteLine("|");
                            Console.WriteLine(space);
                            break;
                        default: break;
                    }
                }
                Console.WriteLine();
            }
        }

        public void DisplayCity()
        {

        }

        public void DisplayRoll(int face) { Console.Write($"+---+\n| {face} |\n +---+\n"); }

        public void WriteInColor(string toWrite, ConsoleColor color)
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
    }
}
