namespace Miniville
{
    /** Classe représentant une pile de carte dans laquelle on peut piocher un seul type de carte. */
    public class Pile
    {
        /// <summary> Le type de carte contenu dans la pile. </summary>
        public Card card { get; private set; }
        
        /// <summary> Le nombre de cartes restantes dans la pile. </summary>
        public int nbCard { get; private set; } = 6;

        /// <summary> Le constructeur de la pile. </summary>
        /// <param name="_card"> Le type de carte quisera contenu dans la pile. </param>
        public Pile(Card _card)
        {
            card = _card;
        }

        /// <summary> Permet de récupérer un exemplaire de la carte de la pile. </summary>
        /// <returns> Une carte de la pile. </returns>
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