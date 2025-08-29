using Fderivs.Application.Interfaces;
using Fderivs.Domain.BlackScholes.Services;
using Fderivs.Domain.BlackScholes.ValueObjects;

namespace Fderivs.Application.Services
{
    public class GeradorRelatorioBlackScholes
    {
        private readonly IRepositorioSaida _repositorioSaida;
        public GeradorRelatorioBlackScholes(IRepositorioSaida repositorioSaida)
        {
            _repositorioSaida = repositorioSaida;
        }
        public string GerarRelatorio(BlackScholesParametros parametros, IProgress<string>? progresso = null)
        {  
            progresso?.Report("Iniciando a geração de dados pelo modelo BlackScholes");      

            var resultados = new List<BlackScholesResultado>();
            DateTime hoje = DateTime.Today;
            const int anosRequisitoDaAvaliacaoTecnica = 20;
            DateTime fim = hoje.AddYears(anosRequisitoDaAvaliacaoTecnica);

            foreach (var data in EnumerarDiasUteis(hoje, fim))
            {
                int dias = (int)(data - hoje).TotalDays;
                double tempoAnual = dias / 365.0;

                var resultado = BlackScholesCalculoService.Calcular(parametros, tempoAnual, data);
                resultados.Add(resultado);
            }

            progresso?.Report("Dados calculados. Salvando o relatório.");

            return _repositorioSaida.Salvar(resultados, progresso);
        }
        private static IEnumerable<DateTime> EnumerarDiasUteis(DateTime inicioExclusivo, DateTime fimInclusivo)
        {
            for (var d = inicioExclusivo.AddDays(1); d <= fimInclusivo; d = d.AddDays(1))
            {
                if (d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday)
                    yield return d;
            }
        }
    }
}
