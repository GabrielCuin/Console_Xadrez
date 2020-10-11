using System;
using tabuleiro;

namespace Xadrez_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Tabuleiro tab = new Tabuleiro(8, 8);
            Posicao p = new Posicao(3, 4);

            Console.WriteLine(tab);
            Console.ReadLine();
        }
    }
}
