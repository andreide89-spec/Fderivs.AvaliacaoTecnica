using Fderivs.Application.Services;
using Fderivs.Infrastructure;
using Fderivs.Infrastructure.Repositorys;
using System.Globalization;
class Program
{
    private static bool _processando = false;
    static async Task Main()
    {
        IProgress<string> progresso = new Progress<string>(status => Console.WriteLine(status));
        var repositorio = new ArquivoPsvRepository();
        var geradorRelatorio = new GeradorRelatorioBlackScholes(repositorio);
        var adaptador = new OutputBlackScholesModelAdapter(geradorRelatorio, progresso);

        Console.WriteLine("Digite s;k;r;σ e pressione [Enter]. Ex: 30.00;30.00;0.050000;0.200000");
        Console.WriteLine("Enquanto processa, novas entradas serão recusadas.");
        Console.WriteLine();

        while (true)
        {
            string? entrada = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(entrada))
                continue;

            if (_processando)
            {
                Console.WriteLine("O sistema está processando a última requisição. Aguarde o término.");
                continue;
            }

            if (!TentarLerParametros(entrada, out double s, out double k, out double r, out double v))
            {
                Console.WriteLine("Entrada inválida. Use o formato: s;k;r;σ (com ponto como separador decimal).");
                continue;
            }

            await Processar(adaptador, s, k, r, v);
        }
    }

    private static async Task Processar(OutputBlackScholesModelAdapter adaptador, double s, double k, double r, double v)
    {
        try
        {
            _processando = true;
            await Task.Run(() => adaptador.OutputBlackScholesModel(s, k, r, v));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }
        finally
        {
            _processando = false;
        }
    }

    private static bool TentarLerParametros(string input, out double s, out double k, out double r, out double v)
    {
        s = k = r = v = 0.0;

        var partes = input.Split(';');
        if (partes.Length != 4) return false;

        return double.TryParse(partes[0], NumberStyles.Float, CultureInfo.InvariantCulture, out s) &&
               double.TryParse(partes[1], NumberStyles.Float, CultureInfo.InvariantCulture, out k) &&
               double.TryParse(partes[2], NumberStyles.Float, CultureInfo.InvariantCulture, out r) &&
               double.TryParse(partes[3], NumberStyles.Float, CultureInfo.InvariantCulture, out v);
    }

}