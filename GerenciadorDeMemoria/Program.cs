using GerenciadorDeMemoria;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

string pasta = "C:\\Users\\lucio\\OneDrive\\Documentos\\Estudo\\IFMG\\SistemasOperacionais\\GerenciadorDeMemoria\\GerenciadorDeMemoria\\ArquivosTeste";

if (Directory.Exists(pasta))
{
    string[] files = Directory.GetFiles(pasta);

    foreach (string file in files)
    {

        List<Pagina> paginas = new List<Pagina>();

        // Lê o conteúdo do arquivo
        string content = File.ReadAllText(file);
        List<string> PaginasString = content.Split(Environment.NewLine).ToList();

        int totalPaginas = int.Parse(PaginasString[0]);
        PaginasString.RemoveAt(0);

        int espacoMemoria = int.Parse(PaginasString[0]);
        PaginasString.RemoveAt(0);

        int cicloRelogio = int.Parse(PaginasString[0]);
        PaginasString.RemoveAt(0);

        Console.WriteLine($"Processando arquivo: {file}");

        // Converte as linhas restantes em objetos Pagina
        foreach (var item in PaginasString)
        {
            if (string.IsNullOrWhiteSpace(item)) continue; // Ignora linhas vazias

            string[] partes = Regex.Replace(item, @"\s+", " ").Split(" "); // Normaliza os tipos de espaços, necessário pois podem haver espaços inquebráveis que devem ser substituidos por espaços normais.

            int numPagina = int.Parse(partes[0]);
            int chegada = int.Parse(partes[1]);
            string tipoAcesso = partes[2];

            paginas.Add(new Pagina(numPagina, chegada, tipoAcesso));
        }

        // Executa os algoritmos de escalonamento
        GerenciadorMemoria gerenciadorMemoria = new GerenciadorMemoria(espacoMemoria, cicloRelogio);

        List<Pagina> backupPaginas = CopiarListaPaginas(paginas);

        int resultadoOtimo = gerenciadorMemoria.executaOTIMO(CopiarListaPaginas(backupPaginas));
        Console.WriteLine("Resultado ótimo: " + resultadoOtimo);

        int resultadoNRU = gerenciadorMemoria.executaNRU(CopiarListaPaginas(backupPaginas));
        Console.WriteLine("Resultado NRU: " + resultadoNRU);

        int resultadoRelogio = gerenciadorMemoria.executaRelogio(CopiarListaPaginas(backupPaginas));
        Console.WriteLine("Resultado Relógio: " + resultadoRelogio);

        int resultadoWSClock = gerenciadorMemoria.executaWSClock(CopiarListaPaginas(backupPaginas));
        Console.WriteLine("Resultado WSClock: " + resultadoWSClock);

        // Gera o nome do arquivo de saída
        string nomeArquivoSemExtensao = Path.GetFileNameWithoutExtension(file);
        string numero = nomeArquivoSemExtensao.Split("-")[1];
        string caminhoArquivoResultado = $"{pasta}TESTE-{numero}-RESULTADO.txt";

        // Cria o conteúdo do arquivo de saída
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"{resultadoOtimo}");
        sb.AppendLine($"{resultadoNRU}");
        sb.AppendLine($"{resultadoRelogio}");
        sb.AppendLine($"{resultadoWSClock}");

        // Grava o arquivo de resultados
        File.WriteAllText(caminhoArquivoResultado, sb.ToString());

        Console.WriteLine($"Resultados salvos em: {caminhoArquivoResultado}");


    }

    List<Pagina> CopiarListaPaginas(List<Pagina> original)
    {
        return original.Select(pagina => new Pagina(pagina.Numero, pagina.Chegada, pagina.TipoAcesso)).ToList();
    }
}