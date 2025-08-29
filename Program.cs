using Fderivs.Infrastructure;
using System.Diagnostics;
using System.Globalization;
class Program
{
    private static int _processando = 0;
    static void Main()
    {  
        Console.WriteLine("Digite s;k;r;σ e pressione [Enter]. Ex: 30.00;30.00;0.050000;0.200000");
        Console.WriteLine("Enquanto processa, novas entradas serão recusadas.");
        Console.WriteLine();

        var progresso = new Progress<string>(msg => Console.WriteLine(msg));
        IOutputBlackScholesModel gerador = new OutputBlackScholesModel(progresso);

        while (true)
        {
            Console.Write("> ");
            string? linha = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(linha))
                continue;

            if (Interlocked.CompareExchange(ref _processando, 1, 0) == 1)
            {
                Console.WriteLine("O sistema está processando a última requisição. Aguarde o término.");
                continue;
            }

            if (!TentarLerParametros(linha, out double s, out double k, out double r, out double v))
            {
                Console.WriteLine("Entrada inválida. Use o formato: s;k;r;σ (com ponto como separador decimal).");
                Interlocked.Exchange(ref _processando, 0);
                continue;
            }
            
            Task.Run(() =>
            {
                try
                {
                    var sw = Stopwatch.StartNew();
                    string caminho = gerador.OutputBlackScholesModel(s, k, r, v);
                    sw.Stop();

                    Console.WriteLine();
                    Console.WriteLine($"Concluído. Tempo total: {sw.ElapsedMilliseconds} ms");
                    Console.WriteLine($"Arquivo: {caminho}");
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro: {ex.Message}");
                }
                finally
                {
                    Interlocked.Exchange(ref _processando, 0);
                }
            });
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