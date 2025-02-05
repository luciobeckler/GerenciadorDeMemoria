namespace GerenciadorDeMemoria
{
    public class GerenciadorMemoria
    {
        private List<int> _molduras = new List<int>();
        private int _memoria { get; set; }
        private int _cicloRelogio;
        int _contador = 0;


        public GerenciadorMemoria(int memoria, int ciclo)
        {
            _memoria = memoria;
            _cicloRelogio = ciclo;
        }

        public int executaOTIMO(List<Pagina> paginas)
        {
            InicializaMemoria(paginas);

            while (paginas.Count > 0)
            {
                bool isPaginaInMoldura = _molduras.Contains(paginas.First().Numero);

                if (isPaginaInMoldura)
                    paginas.RemoveAt(0);
                else
                {
                    SubstituiPaginaNaMoldura(paginas);
                    _contador++;
                }
            }

            return _contador;
        }

        private void SubstituiPaginaNaMoldura(List<Pagina> paginas)
        {
            int index = EncontraMoldura(paginas);

            _molduras[index] = paginas.First().Numero;
            paginas.RemoveAt(0);
        }

        private int EncontraMoldura(List<Pagina> paginas)
        {
            int indiceMaisDemorado = -1;
            int maiorDistancia = -1;

            for (int i = 0; i < _molduras.Count; i++)
            {
                int molduraAtual = _molduras[i];

                int proximaOcorrencia = paginas
                    .Select((pagina, index) => new { pagina, index })
                    .Where(x => x.pagina.Numero == molduraAtual)
                    .Select(x => x.index)
                    .DefaultIfEmpty(-1)
                    .First();

                if (proximaOcorrencia == -1)
                {
                    return i;
                }

                if (proximaOcorrencia > maiorDistancia)
                {
                    maiorDistancia = proximaOcorrencia;
                    indiceMaisDemorado = i;
                }
            }

            return indiceMaisDemorado;
        }

        private void InicializaMemoria(List<Pagina> paginas)
        {
            while (_molduras.Count != _memoria)
            {
                _contador++;
                _molduras.Add(paginas.First().Numero);
                paginas.RemoveAt(0);
            }
        }

        public int executaNRU(List<Pagina> paginas)
        {
            throw new NotImplementedException();
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
