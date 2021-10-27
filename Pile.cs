namespace Miniville
{
    public class Pile
    {
        //Carte de la pile
        public Card card { get; private set; }
        
        //Nombre de carte de la pile, 6 par défaut;
        public int nbCard { get; private set; } = 6;

        public Pile(Card _card)
        {
            card = _card;
        }

        public Card Draw()
        {
            //S'il y a au moins une carte dans la pile...
            if (nbCard > 0)
            {
                //Décrémente le nombre de carte dans la pile
                nbCard--;
                
                //Retourne la carte contenue dans la pile
                return card;
            }
            else
            {
                //Retourne null s'il n'y a plus de carte dans la pile
                return null;
            }
        }
    }
}