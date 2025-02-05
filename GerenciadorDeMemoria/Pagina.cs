namespace GerenciadorDeMemoria
{
    public class Pagina
    {
        public int Numero { get; set; }
        public int Chegada { get; set; }
        private string _tipoAcesso;

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

        public Pagina(int numero, int chegada, string tipoAcesso)
        {
            Numero = numero;
            Chegada = chegada;
            TipoAcesso = tipoAcesso;
        }
    }

}
