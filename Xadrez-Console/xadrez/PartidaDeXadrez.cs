using System.Collections.Generic;
using tabuleiro;
namespace xadrez
{
    class PartidaDeXadrez
    {
        public Tabuleiro tab { get; private set; }
        public int turno { get; private set; }
        public Cor JogadorAtual { get; private set; }
        public bool terminada { get; private set; }

        private HashSet<Peca> pecas;
        private HashSet<Peca> capturadas;
        public bool xeque { get; private set; }

        public PartidaDeXadrez()
        {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            JogadorAtual = Cor.Branca;
            terminada = false;
            xeque = false;
            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();
            ColocarPecas();
        }
        public Peca ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tab.retirarPeca(origem);
            p.incrementarQtdMovimentos();
            Peca pecaCapturada = tab.retirarPeca(destino);
            tab.colocarPeca(p, destino);
            if (pecaCapturada != null)
            {
                capturadas.Add(pecaCapturada);
            }

            //RoquePequeno
            if (p is Rei && destino.coluna == origem.coluna + 2)
            {
                Posicao OrigemT = new Posicao(origem.linha, origem.coluna + 3);
                Posicao DestinoT = new Posicao(origem.linha, origem.coluna + 1);

                Peca T = tab.retirarPeca(OrigemT);
                T.incrementarQtdMovimentos();
                tab.colocarPeca(T, DestinoT);
            }
            //RoqueGrande
            if (p is Rei && destino.coluna == origem.coluna - 2)
            {
                Posicao OrigemT = new Posicao(origem.linha, origem.coluna - 4);
                Posicao DestinoT = new Posicao(origem.linha, origem.coluna - 1);

                Peca T = tab.retirarPeca(OrigemT);
                T.incrementarQtdMovimentos();
                tab.colocarPeca(T, DestinoT);
            }
            return pecaCapturada;
        }
        public void realizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = ExecutaMovimento(origem, destino);
            if (estaEmXeque(JogadorAtual))
            {
                desfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque!");
            }
            if (estaEmXeque(adiversaria(JogadorAtual)))
            {
                xeque = true;
            }
            else
            {
                xeque = false;
            }
            if (testeEmXequemate(adiversaria(JogadorAtual)))
            {
                terminada = true;
            }
            else
            {
                turno++;
                mudaJogador();
            }
        }

        private void desfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = tab.retirarPeca(destino);
            p.decrementarQtdMovimentos();
            if (pecaCapturada != null)
            {
                tab.colocarPeca(pecaCapturada, destino);
                capturadas.Remove(pecaCapturada);
            }
            tab.colocarPeca(p, origem);

            //RoquePequeno
            if (p is Rei && destino.coluna == origem.coluna + 2)
            {
                Posicao OrigemT = new Posicao(origem.linha, origem.coluna + 3);
                Posicao DestinoT = new Posicao(origem.linha, origem.coluna + 1);

                Peca T = tab.retirarPeca(DestinoT);
                T.decrementarQtdMovimentos();
                tab.colocarPeca(T, OrigemT);
            }
            //RoqueGrande
            if (p is Rei && destino.coluna == origem.coluna - 2)
            {
                Posicao OrigemT = new Posicao(origem.linha, origem.coluna - 4);
                Posicao DestinoT = new Posicao(origem.linha, origem.coluna - 1);

                Peca T = tab.retirarPeca(DestinoT);
                T.decrementarQtdMovimentos();
                tab.colocarPeca(T, OrigemT);
            }
        }

        public void validarPosicaoDeOrigem(Posicao pos)
        {
            if (tab.peca(pos) == null)
            {
                throw new TabuleiroException("Não existe peça na posição de origem escolhida!");
            }
            if (JogadorAtual != tab.peca(pos).cor)
            {
                throw new TabuleiroException("A peça de origem escolhida não é sua!");
            }
            if (!tab.peca(pos).existeMovimentosPossiveis())
            {
                throw new TabuleiroException("Não há movimentos possíveis para a peça de origem escolhida!");
            }
        }
        public void validarPosicaoDeDestino(Posicao origem, Posicao destino)
        {
            if (!tab.peca(origem).movimentoPossivel(destino))
            {
                throw new TabuleiroException("Posição de destino inválida!");
            }
        }

        private void mudaJogador()
        {
            if (JogadorAtual == Cor.Branca)
            {
                JogadorAtual = Cor.Preta;
            }
            else
            {
                JogadorAtual = Cor.Branca;
            }
        }
        public HashSet<Peca> pecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca peca in capturadas)
            {
                if (peca.cor == cor)
                {
                    aux.Add(peca);
                }
            }
            return aux;
        }
        public HashSet<Peca> pecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca peca in pecas)
            {
                if (peca.cor == cor)
                {
                    aux.Add(peca);
                }
            }
            aux.ExceptWith(pecasCapturadas(cor));
            return aux;
        }
        private Cor adiversaria(Cor cor)
        {
            if (cor == Cor.Branca)
            {
                return Cor.Preta;
            }
            else
            {
                return Cor.Branca;
            }
        }
        private Peca rei(Cor cor)
        {
            foreach (Peca peca in pecasEmJogo(cor))
            {
                if (peca is Rei)
                {
                    return peca;
                }
            }
            return null;
        }
        public bool estaEmXeque(Cor cor)
        {
            Peca r = rei(cor);
            if (r == null)
            {
                throw new TabuleiroException("Não tem Rei da cor " + cor + " no tabuleiro!");
            }
            foreach (Peca peca in pecasEmJogo(adiversaria(cor)))
            {
                bool[,] mat = peca.movimentosPossiveis();
                if (mat[r.posicao.linha, r.posicao.coluna])
                {
                    return true;
                }
            }
            return false;
        }
        public bool testeEmXequemate(Cor cor)
        {
            if (!estaEmXeque(cor))
            {
                return false;
            }
            foreach (Peca x in pecasEmJogo(cor))
            {
                bool[,] mat = x.movimentosPossiveis();
                for (int i = 0; i < tab.linhas; i++)
                {
                    for (int j = 0; j < tab.colunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = x.posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = ExecutaMovimento(origem, destino);
                            bool testeXeque = estaEmXeque(cor);
                            desfazMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
        public void colocaNovaPeca(char coluna, int linha, Peca peca)
        {
            tab.colocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
            pecas.Add(peca);
        }
        private void ColocarPecas()
        {
            colocaNovaPeca('a', 1, new Torre(tab, Cor.Branca));
            colocaNovaPeca('b', 1, new Cavalo(tab, Cor.Branca));
            colocaNovaPeca('c', 1, new Bispo(tab, Cor.Branca));
            colocaNovaPeca('d', 1, new Dama(tab, Cor.Branca));
            colocaNovaPeca('e', 1, new Rei(tab, Cor.Branca, this));
            colocaNovaPeca('f', 1, new Bispo(tab, Cor.Branca));
            colocaNovaPeca('g', 1, new Cavalo(tab, Cor.Branca));
            colocaNovaPeca('h', 1, new Torre(tab, Cor.Branca));
            colocaNovaPeca('a', 2, new Peao(tab, Cor.Branca));
            colocaNovaPeca('b', 2, new Peao(tab, Cor.Branca));
            colocaNovaPeca('c', 2, new Peao(tab, Cor.Branca));
            colocaNovaPeca('d', 2, new Peao(tab, Cor.Branca));
            colocaNovaPeca('e', 2, new Peao(tab, Cor.Branca));
            colocaNovaPeca('f', 2, new Peao(tab, Cor.Branca));
            colocaNovaPeca('g', 2, new Peao(tab, Cor.Branca));
            colocaNovaPeca('h', 2, new Peao(tab, Cor.Branca));

            colocaNovaPeca('a', 8, new Torre(tab, Cor.Preta));
            colocaNovaPeca('b', 8, new Cavalo(tab, Cor.Preta));
            colocaNovaPeca('c', 8, new Bispo(tab, Cor.Preta));
            colocaNovaPeca('d', 8, new Dama(tab, Cor.Preta));
            colocaNovaPeca('e', 8, new Rei(tab, Cor.Preta, this));
            colocaNovaPeca('f', 8, new Bispo(tab, Cor.Preta));
            colocaNovaPeca('g', 8, new Cavalo(tab, Cor.Preta));
            colocaNovaPeca('h', 8, new Torre(tab, Cor.Preta));
            colocaNovaPeca('a', 7, new Peao(tab, Cor.Preta));
            colocaNovaPeca('b', 7, new Peao(tab, Cor.Preta));
            colocaNovaPeca('c', 7, new Peao(tab, Cor.Preta));
            colocaNovaPeca('d', 7, new Peao(tab, Cor.Preta));
            colocaNovaPeca('e', 7, new Peao(tab, Cor.Preta));
            colocaNovaPeca('f', 7, new Peao(tab, Cor.Preta));
            colocaNovaPeca('g', 7, new Peao(tab, Cor.Preta));
            colocaNovaPeca('h', 7, new Peao(tab, Cor.Preta));
        }
    }
}
