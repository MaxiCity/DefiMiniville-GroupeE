using System;

namespace  Miniville
{
    public class Card
    {
        ///<summary> Nom de la carte. </summary>
        public string name{ get; private set; }

        ///<summary> Argent rapporté par la carte lors de son utilisation. <summary>
        public int moneyToEarn { get; private set; }

        ///<summary> Résultat à obtenir au dé pour activer l'effet. <summary>
        public int[] dieCondition { get; private set; }

        ///<summary> Prix de la carte. </summary>
        public int cost { get; private set; }

        ///<summary> Couleur de la carte. </summary>
        public ConsoleColor color { get; private set; }

        ///<summary> Description de la carte répartie dans un tableau afin de la segmenter pour les afficher lignes par lignes. </summary>
        public string[] description { get; private set; } = new string[3];

        /// <summary> Constructeur de la carte, permet de savoir quel type de carte elle représente. </summary>
        /// <param name="_name"> Le nom de la carte. </param>
        /// <param name="_moneyToEarn"> L'argent que rapporte l'effet de la carte. </param>
        /// <param name="_dieCondition"> La valeur du dé qui active l'effet de la carte. </param>
        /// <param name="_cost"> Le coût de la carte. </param>
        /// <param name="_color"> La couleur de la carte. </param>
        public Card(string _name, int _moneyToEarn, int[] _dieCondition, int _cost, ConsoleColor _color)
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