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
        public string[] description { get; private set; }

        public Card(string _name, int _moneyToEarn, int _dieCondition, int _cost, ConsoleColor _color,
            string[] _description)
        {
            name = _name;
            moneyToEarn = _moneyToEarn;
            dieCondition = _dieCondition;
            cost = _cost;
            color = _color;
            description = _description;
        }

    }
}