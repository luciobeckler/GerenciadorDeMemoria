namespace GerenciadorDeMemoria
{
    public class Pagina
    {
        public int Numero { get; set; }
        public int Chegada { get; set; }
        public bool Referenciada { get; set; } = false;
        public bool Modificada { get; set; } = false;
        private string _tipoAcesso;
        private int _numClasse;

        public string TipoAcesso
        {
            get => _tipoAcesso;
            set
            {
                if (value != "W" && value != "R")
                    throw new ArgumentException("Tipo de dado inválido, permitido apenas W ou R");

                _tipoAcesso = value;
            }
        }

        public int NumClasse
        {
            get => _numClasse;
            set
            {
                if (!Referenciada && !Modificada)
                    _numClasse = 0;
                else if (!Referenciada && Modificada)
                    _numClasse = 1;
                else if (Referenciada && !Modificada)
                    _numClasse = 2;
                else
                    _numClasse = 3;
            }
        }

        public Pagina(int numero, int chegada, string tipoAcesso)
        {
            Numero = numero;
            Chegada = chegada;
            TipoAcesso = tipoAcesso;
        }
    }

}
