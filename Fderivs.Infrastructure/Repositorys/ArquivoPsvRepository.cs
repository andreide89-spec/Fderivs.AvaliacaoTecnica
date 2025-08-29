using Fderivs.Application.Interfaces;
using Fderivs.Domain.BlackScholes.ValueObjects;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Fderivs.Infrastructure.Repositorys
{
    public class ArquivoPsvRepository : IRepositorioSaida
    {
        public string Salvar(IEnumerable<BlackScholesResultado> resultados, IProgress<string>? progresso = null)
        {
            var sw = Stopwatch.StartNew();

            string usuario = Environment.UserName;
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string nomeArquivo = $"PPX_CALL_PUT_{usuario}_{timestamp}.PSV";
            string pasta = AppContext.BaseDirectory;
            string caminho = Path.Combine(pasta, nomeArquivo);

            progresso?.Report("Iniciando a geração do arquivo...");

            using var writer = new StreamWriter(caminho, append: false, encoding: new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
            writer.WriteLine("P|K|R|V|E|T|PC|PP");

            foreach (var resultado in resultados)
            {
                string linha = string.Format(
                    CultureInfo.InvariantCulture,
                    "{0:F2}|{1:F2}|{2:F6}|{3:F6}|{4}|{5:F6}|{6:F6}|{7:F6}",
                    resultado.Parametros.Preco,
                    resultado.Parametros.Strike,
                    resultado.Parametros.TaxaDeCarrego,
                    resultado.Parametros.Volatilidade,
                    resultado.DataVencimento.ToString("yyyyMMdd"),
                    resultado.TempoAnual,
                    resultado.Call,
                    resultado.Put
                );

                writer.WriteLine(linha);
                Console.WriteLine(linha);
            }

            writer.Flush();
            sw.Stop();

            progresso?.Report($"Arquivo gerado em {sw.ElapsedMilliseconds} ms.");
            progresso?.Report($"Caminho: {caminho}");

            return caminho;
        }
    }
}
