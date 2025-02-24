using System.Reflection.Metadata.Ecma335;

namespace GerenciadorDeMemoria
{
    public class GerenciadorMemoria
    {
        public int _tamanhoMoldura { get; set; }
        private int _cicloRelogio;
        private List<Pagina> _molduras;
        private int _contador;
        private int _ponteiro;

        public GerenciadorMemoria(int tamanhoMoldura, int cicloRelogio)
        {
            _tamanhoMoldura = tamanhoMoldura;
            _molduras = new List<Pagina>(tamanhoMoldura);
            _cicloRelogio = cicloRelogio;
            _contador = 0;
        }

        public int executaOTIMO(List<Pagina> paginasOriginais)
        {
            ResetaGerenciador();
            int timer = 0;

            while (paginasOriginais.Count > 0)
            {
                List<Pagina> paginasAtuais = paginasOriginais
                    .Where(p => timer >= p.Chegada)
                    .ToList();

                foreach (var paginaAtual in paginasAtuais)
                {
                    AjustaMolduraOTIMO(paginaAtual, paginasOriginais);
                    paginasOriginais.RemoveAt(0);
                }

                timer++;
            }
            return _contador;
        }

        public int executaNRU(List<Pagina> paginasOriginais)
        {
            ResetaGerenciador();
            int timer = 0;

            while (paginasOriginais.Count > 0)
            {
                List<Pagina> paginasAtuais = paginasOriginais
                    .Where(p => timer >= p.Chegada)
                    .ToList();

                foreach (var paginaAtual in paginasAtuais)
                {
                    AjustaMolduraNRU(paginaAtual, timer);
                    paginasOriginais.RemoveAt(0);
                }

                timer++;
            }
            return _contador;
        }

        public int executaRelogio(List<Pagina> paginasOriginais)
        {
            ResetaGerenciador();
            int timer = 0;
            _ponteiro = 0;

            while(paginasOriginais.Count > 0)
            {
                List<Pagina> paginasAtuais = paginasOriginais
                    .Where(p => timer > p.Chegada)
                    .ToList();

                foreach (var paginaAtual in paginasAtuais)
                {
                    AjustaMolduraRELOGIO(paginaAtual, timer);
                    paginasOriginais.RemoveAt(0);
                }

                timer++;
            }

            return _contador;
        }
        private void AjustaMolduraRELOGIO(Pagina paginaAtual, int timer)
        {
            foreach (var pagina in _molduras)
            {
                pagina.AtualizaBitR(timer, _cicloRelogio);
            }

            bool contemEspacoMoldura = _molduras.Count < _tamanhoMoldura;
            if (contemEspacoMoldura)
            {
                _molduras.Add(paginaAtual);
                _contador++;
                return;
            }

            bool isPaginaAtualNaMoldura = _molduras.Select(m => m.Numero).Contains(paginaAtual.Numero);
            if (isPaginaAtualNaMoldura)
            {
                int indexMoldura = _molduras.IndexOf(_molduras.First(p => p.Numero == paginaAtual.Numero));
                _molduras[indexMoldura] = paginaAtual;

                if (indexMoldura == _ponteiro)
                    _ponteiro = (_ponteiro + 1) % _molduras.Count;

                return;
            }

            _molduras[_ponteiro] = paginaAtual;
            _ponteiro = (_ponteiro + 1) % _molduras.Count;
            _contador++;
        }

        public int executaWSClock(List<Pagina> paginasOriginais)
        {
            ResetaGerenciador();
            int timer = 0;

            while (paginasOriginais.Count > 0)
            {
                List<Pagina> paginasAtuais = paginasOriginais
                    .Where(p => timer >= p.Chegada)
                    .ToList();

                foreach (var paginaAtual in paginasAtuais)
                {
                    AjustaMolduraWSClock(paginaAtual, timer);
                    paginasOriginais.RemoveAt(0);
                }

                timer++;
            }

            return _contador;
        }

        private void AjustaMolduraOTIMO(Pagina paginaAtual, List<Pagina> paginasOriginais)
        {
            if (_molduras.Count < _tamanhoMoldura)
            {
                _molduras.Add(paginaAtual);
                _contador++;
                return;
            }

            if (_molduras.Select(m => m.Numero).Contains(paginaAtual.Numero))
            {
                return;
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
                    return;
                }

                if (proximaPosicao > indexMaxDistanciaPaginas)
                {
                    indexMaxDistanciaPaginas = proximaPosicao;
                    indexMaxDistanciaMoldura = i;
                }
            }

            _contador++;
            _molduras[indexMaxDistanciaMoldura] = paginaAtual;
            return;
        }
        private void AjustaMolduraNRU(Pagina paginaAtual, int timer)
        {
            foreach (var pagina in _molduras)
            {
                pagina.AtualizaBitR(timer, _cicloRelogio);
            }

            bool contemEspacoMoldura = _molduras.Count < _tamanhoMoldura;
            if (contemEspacoMoldura)
            {
                _molduras.Add(paginaAtual);
                _contador++;

                return;
            }

            bool isPaginaAtualNaMoldura = _molduras.Select(m => m.Numero).Contains(paginaAtual.Numero);
            if (isPaginaAtualNaMoldura)
            {
                int indexMoldura = _molduras
                    .IndexOf(_molduras
                    .First(p => p.Numero == paginaAtual.Numero));

                _molduras[indexMoldura] = paginaAtual;

                return;
            }

            int indexParaRemover = SelecionaPaginaParaRemoverNRU();
            if (indexParaRemover != -1)
            {
                _molduras[indexParaRemover] = paginaAtual;
                _contador++;
            }
        }
        private int SelecionaPaginaParaRemoverNRU()
        {
            List<Pagina> paginasAgrupadasPorClasse = _molduras
                .OrderBy(p => p.Classe)
                .ToList();

            if (paginasAgrupadasPorClasse.Count > 0)
            {
                return _molduras.IndexOf(paginasAgrupadasPorClasse.First());
            }

            return -1;
        }
        private void AjustaMolduraWSClock(Pagina paginaAtual, int timer)
        {
            foreach (var pagina in _molduras)
            {
                pagina.AtualizaBitR(timer, _cicloRelogio);
            }

            bool contemEspacoMoldura = _molduras.Count < _tamanhoMoldura;
            if (contemEspacoMoldura)
            {
                _molduras.Add(paginaAtual);
                _contador++;
                return;
            }

            bool isPaginaAtualNaMoldura = _molduras.Any(p => p.Numero == paginaAtual.Numero);
            if (isPaginaAtualNaMoldura)
            {
                int indexMoldura = _molduras.FindIndex(p => p.Numero == paginaAtual.Numero);
                _molduras[indexMoldura] = paginaAtual;
                return;
            }

            int inicioPonteiro = _ponteiro; // Armazena o ponto de início

            do
            {
                var pagina = _molduras[_ponteiro];
                bool tempoExpirado = (timer - pagina.Chegada) >= _cicloRelogio;

                if (!pagina.R && tempoExpirado)
                {
                    _molduras[_ponteiro] = paginaAtual;
                    _contador++;
                    _ponteiro = (_ponteiro + 1) % _molduras.Count;
                    return;
                }
                else
                {
                    pagina.R = false;
                }

                _ponteiro = (_ponteiro + 1) % _molduras.Count;

            } while (_ponteiro != inicioPonteiro);  // Sai após percorrer todas as páginas
        }

        private void ResetaGerenciador()
        {
            _ponteiro = 0;
            _contador = 0;
            _molduras.Clear();
        }

    }
}
