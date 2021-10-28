using System;

namespace Miniville
{
    public class Die
    {
        /// <summary>
        /// Variable indiquant le nombre de face, comme on utilise un D6 ce seras 7 random prends entre x et y-1
        /// </summary>
        public static int nbFaces = 6;
        
        /// <summary>
        /// Méthode de lancer pour réaliser le lancer de dé.
        /// </summary>
        /// <returns>la face sur laquelle le dé est tombé</returns>
        public static int Lancer()
        {
            Random rand = new Random();
            int face = rand.Next(1, nbFaces+1);

            return (face);
        }
    }
}