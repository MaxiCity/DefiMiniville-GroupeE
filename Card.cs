using System;

namespace  Miniville
{
    public class Card
    {
        //Nom de la carte
        public string name{ get; private set; }
        
        //Argent rapporté par la carte lors de son utilisation
        public int moneyToEarn { get; private set; }
        
        //Résultat à obtenir au dé
        public int dieCondition { get; private set; }
        
        //Prix de la carte
        public int cost { get; private set; }
        
        //Couleur de la carte
        public ConsoleColor color { get; private set; }
        
        //Description de la carte(répartie dans un tableau afin de la segmenter)
        public string[] description { get; private set; } = new string[3];

        public Card(string _name, int _moneyToEarn, int _dieCondition, int _cost, ConsoleColor _color)
        {
            name = _name;
            moneyToEarn = _moneyToEarn;
            dieCondition = _dieCondition;
            cost = _cost;
            color = _color;


            switch (_color)
            {
                case ConsoleColor.Cyan:
                    description[0] = $"Gagnez {_moneyToEarn}$";
                    description[1] = "S'active tout";
                    description[2] = "le temps";
                    break;
                case ConsoleColor.Green:
                    description[0] = $"Gagnez {_moneyToEarn}$";
                    description[1] = "S'active à";
                    description[2] = "votre tour";
                    break;
                case ConsoleColor.Red:
                    description[0] = $"Volez {_moneyToEarn}$";
                    description[1] = "S'active au";
                    description[2] = "tour adverse";
                    break;
            }

        }

    }
}