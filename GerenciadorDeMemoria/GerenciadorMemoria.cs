using System.Reflection.Metadata.Ecma335;

namespace GerenciadorDeMemoria
{
    public class GerenciadorMemoria
    {
        public int _tamanhoMoldura { get; set; }
        private int _cicloRelogio;
        private List<Pagina> _molduras;
        private int _contador;

        public GerenciadorMemoria(int tamanhoMoldura, int cicloRelogio)
        {
            _tamanhoMoldura = tamanhoMoldura;
            _molduras = new List<Pagina>(tamanhoMoldura);
            _cicloRelogio = cicloRelogio;
            _contador = 0;
        }

        public int executaOTIMO(List<Pagina> paginasOriginais)
        {
            int timer = 0;
            int indexRemovido;

            while (paginasOriginais.Count > 0)
            {
                List<Pagina> paginasAtuais = paginasOriginais
                    .Where(p => timer >= p.Chegada)
                    .ToList();

                foreach (var paginaAtual in paginasAtuais)
                {
                    indexRemovido = AjustaMolduraOtimoRetornaIndexParaRemover(paginaAtual, paginasOriginais);
                    paginasOriginais.RemoveAt(indexRemovido);
                }

                timer++;
            }
            return _contador;
        }

        private int AjustaMolduraOtimoRetornaIndexParaRemover(Pagina paginaAtual, List<Pagina> paginasOriginais)
        {
            if (_molduras.Select(m => m.Numero).Contains(paginaAtual.Numero))
            {
                return paginasOriginais.IndexOf(paginaAtual);
            }

            if (_molduras.Count < _tamanhoMoldura)
            {
                _molduras.Add(paginaAtual);
                _contador++;
                return paginasOriginais.IndexOf(paginaAtual);
            }

            int indexMaxDistanciaPaginas = -1;
            int indexMaxDistanciaMoldura = -1;

            for (int i = 0; i < _molduras.Count; i++)
            {
                var pagina = _molduras[i];
                int proximaPosicao = paginasOriginais.FindIndex(pos => pos.Numero == pagina.Numero);

                if (proximaPosicao == -1)
                {
                    _contador++;
                    _molduras[i] = paginaAtual;
                    return 0;
                }

                if (proximaPosicao > indexMaxDistanciaPaginas)
                {
                    indexMaxDistanciaPaginas = proximaPosicao;
                    indexMaxDistanciaMoldura = i;
                }
            }

            _contador++;
            _molduras[indexMaxDistanciaMoldura] = paginaAtual;
            return 0;
        }

        public int executaNRU(List<Pagina> paginas)
        {
            return _contador;
        }

        public int executaRelogio(List<Pagina> paginas)
        {
            throw new NotImplementedException();
        }

        public int executaWSClock(List<Pagina> paginas)
        {
            throw new NotImplementedException();
        }
        


      

    }

}
