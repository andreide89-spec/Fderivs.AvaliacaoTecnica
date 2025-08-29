using Fderivs.Domain.BlackScholes.ValueObjects;

namespace Fderivs.Application.Interfaces
{
    public interface  IRepositorioSaida
    {
        string Salvar(IEnumerable<BlackScholesResultado> resultados, IProgress<string>? progresso = null);
    }
}
