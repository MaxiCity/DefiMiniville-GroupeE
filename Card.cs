using System;

namespace  Miniville
{
    public class Card
    {
        public string name{ get; private set; }
        public int moneyToEarn { get; private set; }
        public int dieCondition { get; private set; }
        public int cost { get; private set; }
        public ConsoleColor color { get; private set; }
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