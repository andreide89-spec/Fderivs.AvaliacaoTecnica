using Fderivs.Application.Services;
using Fderivs.Domain.BlackScholes.ValueObjects;

namespace Fderivs.Infrastructure
{
    public class OutputBlackScholesModelAdapter : IOutputBlackScholesModel
    {
        private readonly GeradorRelatorioBlackScholes _geradorRelatorio;
        private readonly IProgress<string> _progresso;
        public OutputBlackScholesModelAdapter(GeradorRelatorioBlackScholes geradorRelatorio, IProgress<string> progresso)
        {
            _geradorRelatorio = geradorRelatorio;
            _progresso = progresso;
        }

        public string OutputBlackScholesModel(double s, double k, double r, double v)
        {
            var parametros = new BlackScholesParametros(s, k, r, v);
            return _geradorRelatorio.GerarRelatorio(parametros, _progresso);
        }
    }
}
