using tabuleiro;
namespace xadrez
{
    class Bispo : Peca
    {
        public Bispo(Tabuleiro tab, Cor cor) : base(tab, cor)
        {

        }
        public override string ToString()
        {
            return "B";
        }
        private bool podeMover(Posicao pos)
        {
            Peca p = tabuleiro.peca(pos);
            return p == null || p.cor != this.cor;
        }
        public override bool[,] movimentosPossiveis()
        {
            bool[,] mat = new bool[tabuleiro.linhas, tabuleiro.colunas];

            Posicao pos = new Posicao(0, 0);

            //nordeste
            pos.definirValores(posicao.linha - 1, posicao.coluna + 1);
            while (tabuleiro.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.linha, pos.coluna] = true;
                if (tabuleiro.peca(pos) != null && tabuleiro.peca(pos).cor != this.cor)
                {
                    break;
                }
                pos.definirValores(pos.linha - 1, pos.coluna + 1);
            }
            //sudeste
            pos.definirValores(posicao.linha + 1, posicao.coluna + 1);
            while (tabuleiro.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.linha, pos.coluna] = true;
                if (tabuleiro.peca(pos) != null && tabuleiro.peca(pos).cor != this.cor)
                {
                    break;
                }
                pos.definirValores(pos.linha + 1, pos.coluna + 1);
            }
            //sudoeste
            pos.definirValores(posicao.linha + 1, posicao.coluna - 1);
            while (tabuleiro.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.linha, pos.coluna] = true;
                if (tabuleiro.peca(pos) != null && tabuleiro.peca(pos).cor != this.cor)
                {
                    break;
                }
                pos.definirValores(pos.linha + 1, pos.coluna - 1);
            }
            //noroeste
            pos.definirValores(posicao.linha - 1, posicao.coluna - 1);
            while (tabuleiro.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.linha, pos.coluna] = true;
                if (tabuleiro.peca(pos) != null && tabuleiro.peca(pos).cor != this.cor)
                {
                    break;
                }
                pos.definirValores(pos.linha - 1, pos.coluna - 1);
            }
            return mat;
        }
    }
}
