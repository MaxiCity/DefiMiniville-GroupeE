namespace Miniville
{
    public class Pile
    {
        public Card card { get; private set; }
        public int nbCard { get; private set; }

        public Pile(Card _card)
        {
             card = _card;
            nbCard = 8;
        }

        public Card Draw()
        {
            if (nbCard > 0)
            {
                nbCard--;
                return card;
            }
            else
            {
                return null;
            }
        }
    }
}