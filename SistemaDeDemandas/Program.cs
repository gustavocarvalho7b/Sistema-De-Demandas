using System;
using System.IO;

class Program
{
    static void Main()
    {
        bool continuarExecucao = true;

        do
        {
            Console.WriteLine("Menu:");
            Console.WriteLine("1 - Registrar novo atendimento");
            Console.WriteLine("2 - Ver todos os atendimentos do dia");
            Console.WriteLine("3 - Fechar o programa");
            Console.Write("Escolha a opção (1, 2 ou 3): ");

            string escolha = Console.ReadLine();

            switch (escolha)
            {
                case "1":
                    RegistrarNovoAtendimento();
                    break;

                case "2":
                    VerAtendimentosDoDia();
                    break;

                case "3":
                    Console.WriteLine("O programa foi encerrado.");
                    continuarExecucao = false;
                    break;

                default:
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    break;
            }

        } while (continuarExecucao);
    }

    static void CarregarHistoricoAtendimentos()
    {
        string caminhoPasta = CarregarCaminhoPasta();

        if (!string.IsNullOrEmpty(caminhoPasta))
        {
            DateTime hoje = DateTime.Today;
            string nomeArquivo = hoje.ToString("dd.MM.yyyy") + ".txt";
            string caminhoArquivo = Path.Combine(caminhoPasta, nomeArquivo);

            if (File.Exists(caminhoArquivo))
            {
                string[] linhas = File.ReadAllLines(caminhoArquivo);

                var linhasFiltradas = linhas
                    .Where(linha => linha.Contains("Cliente") || linha.Contains("Nome") || linha.Contains("Horário"))
                    .Select(linha => linha.Trim())
                    .ToArray();

                foreach (var linha in linhasFiltradas)
                {
                    Console.Write(linha + " - ");
                }
            }
        }
    }

    static string CarregarCaminhoPasta()
    {
        string? caminhoPasta = null;

        try
        {
            DateTime dataAtual3 = DateTime.Now;
            string dataAtual4 = dataAtual3.ToString("dd.MM.yyyy");
            // Corrigido o caminho do arquivo para carregar o caminho da pasta
            if (File.Exists("C:\\Users\\Embras\\Desktop\\PinkCode\\DestinoAtendimentos\\caminho_pasta.txt"))
            {
                caminhoPasta = File.ReadAllText($"C:\\Users\\Embras\\Desktop\\PinkCode\\DestinoAtendimentos\\caminho_pasta.txt").Trim();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro ao carregar o caminho da pasta: " + ex.Message);
        }

        return caminhoPasta;
    }

    static void RegistrarNovoAtendimento()
    {
        Console.WriteLine("Registrando novo atendimento...");
        DateTime dataAtual2 = DateTime.Now;
        string horarioInicio = dataAtual2.ToString("HH:mm");
        ExecutarAtendimento(horarioInicio);
    }

    static void VerAtendimentosDoDia()
    {
        Console.WriteLine("Visualizando todos os atendimentos do dia...");
        string caminhoPasta = CarregarCaminhoPasta();
        if (!string.IsNullOrEmpty(caminhoPasta))
        {
            DateTime hoje = DateTime.Today;
            string nomeArquivo = hoje.ToString("dd.MM.yyyy") + ".txt";
            string caminhoArquivo = Path.Combine(caminhoPasta, nomeArquivo);

            if (File.Exists(caminhoArquivo))
            {
                string[] linhas = File.ReadAllLines(caminhoArquivo);

                var atendimentosDoDia = linhas
                    .Where(linha => linha.Contains("Cliente") && linha.Contains("Horário"))
                    .Select(linha => new
                    {
                        NomeCliente = ExtrairValor(linha, "Cliente"),
                        Horario = ExtrairValor(linha, "Horário")
                    })
                    .ToList();

                // Exibir atendimentos do dia
                foreach (var atendimento in atendimentosDoDia)
                {
                    Console.WriteLine($"Nome: {atendimento.NomeCliente}, Horário: {atendimento.Horario}");
                }

                // Exibir a soma de atendimentos
                Console.WriteLine($"Total de atendimentos do dia: {atendimentosDoDia.Count}");
            }
        }
    }

    static string ExtrairValor(string linha, string chave)
    {
        int indiceChave = linha.IndexOf(chave);
        if (indiceChave != -1)
        {
            int indiceInicioValor = indiceChave + chave.Length + 1; // +1 para pular o ':' e o espaço
            return linha.Substring(indiceInicioValor);
        }
        return string.Empty;
    }

    static void ExecutarAtendimento(string horarioInicio)
    {
        DateTime dataAtual1 = DateTime.Now;

        // Solicitar informações ao usuário
        Console.Write("Informe o nome do cliente: ");
        string cliente = Console.ReadLine();

        Console.Write("Informe o nome: ");
        string nome = Console.ReadLine();

        // Obter a data atual no formato "dd.MM.yyyy"
        string dataHoje = dataAtual1.ToString("dd.MM.yyyy");
        _ = dataAtual1.ToString("HH:mm");

        Console.Write("Informe a conexão: ");
        string conexao = Console.ReadLine();

        Console.Write("Informe o email: ");
        string email = Console.ReadLine();

        Console.Write("Informe a especificação: ");
        string especificacao = Console.ReadLine();

        DateTime dataAtual = DateTime.Now;
        string horarioTermino = dataAtual.ToString("HH:mm");

        // Construir o caminho do arquivo com base na data informada
        string caminhoArquivo = $"C:\\Users\\Embras\\Desktop\\PinkCode\\DestinoAtendimentos\\{dataHoje}.txt";

        // Escrever informações no bloco de notas
        EscreverNoBlocoDeNotas(caminhoArquivo, cliente, nome, horarioInicio, email, conexao, especificacao, horarioTermino);

        Console.WriteLine("Informações anotadas no bloco de notas.");
    }

    static void EscreverNoBlocoDeNotas(string caminhoArquivo, string cliente, string nome, string horarioInicio, string email, string conexao, string especificacao, string horarioTermino)
    {
        try
        {
            // Criar o diretório se não existir
            string diretorio = Path.GetDirectoryName(caminhoArquivo);
            if (!Directory.Exists(diretorio))
            {
                Directory.CreateDirectory(diretorio);
            }

            // Abrir o arquivo para escrita (cria o arquivo se não existir)
            using (StreamWriter sw = new StreamWriter(caminhoArquivo, true))
            {
                // Escrever as informações no arquivo
                sw.WriteLine($"Cliente: {cliente}");
                sw.WriteLine($"Nome: {nome}");
                sw.WriteLine($"Conexão: {conexao}");
                sw.WriteLine($"Horário: {horarioInicio} ~ {horarioTermino}");
                sw.WriteLine($"Email: {email}");
                sw.WriteLine($"Especificação: {especificacao}");
                sw.WriteLine($"\n\n///////////////////////////////////////////////////////\n");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ocorreu um erro ao escrever no bloco de notas: " + ex.Message);
        }
    }
}