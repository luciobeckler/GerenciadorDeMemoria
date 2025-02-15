namespace GerenciadorDeMemoria
{
    public class Pagina
    {
        public int Numero { get; set; }
        public int Chegada { get; set; }
        public bool R { get; set; }
        public bool M { get; }
        public int Classe => (!M && !R ) ? 0 :
                             (M && !R) ? 1 :
                             (!M && R) ? 2 : 3;

        public void AtualizaBitR(int timer, int cicloRelogio)
        {
            bool isPaginaAntiga = timer >= Chegada + cicloRelogio;
            R = !isPaginaAntiga;
        }

        public Pagina(int numero, int chegada, string tipoAcesso)
        {
            Numero = numero;
            Chegada = chegada;

            if (tipoAcesso != "W" && tipoAcesso != "R")
                throw new ArgumentException("Tipo de dado inválido, permitido apenas W ou R");

            M = tipoAcesso == "W";
            R = true;
        }

        public Pagina(Pagina pagina)
        {
            Numero = pagina.Numero;
            Chegada = pagina.Chegada;
            R = pagina.R;
            M = pagina.M;
        }
    }
}
