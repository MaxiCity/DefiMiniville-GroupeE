using System.Collections.Generic;

namespace Miniville
{
    public class IARandom : IA
    {
        public IARandom(Player _player) : base(_player){}

        public override int IANbDice()
        {
            int nbDice = random.Next(1, 3);
            return nbDice;
        }

        protected override Pile Choose(List<Pile> _possiblePiles)
        {
            if (random.Next(0,4)==0)
            {
                return null;
            }
            //choisir une pile aléatoire parmi la liste des choix possibles
            Pile choice = _possiblePiles[random.Next(_possiblePiles.Count)];
            return choice;
        }
    }
}