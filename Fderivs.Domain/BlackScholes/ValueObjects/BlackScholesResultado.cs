namespace Fderivs.Domain.BlackScholes.ValueObjects
{
    public record BlackScholesResultado(
       BlackScholesParametros Parametros,
       double TempoAnual,
       DateTime DataVencimento,
       double Call,
       double Put
   );
}
