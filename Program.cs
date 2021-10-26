using System;

namespace Miniville
{
    class Game { }

    class Program
    {
        static void Main(string[] args)
        {
            HMICUI mich = new HMICUI(new Game());
            mich.Choose(1);
        }
    }
}
