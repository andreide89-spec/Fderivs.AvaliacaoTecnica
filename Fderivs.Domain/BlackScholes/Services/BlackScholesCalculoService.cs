using Fderivs.Domain.BlackScholes.ValueObjects;
using MathNet.Numerics;

namespace Fderivs.Domain.BlackScholes.Services
{
    public static class BlackScholesCalculoService
    {
        public static BlackScholesResultado Calcular(BlackScholesParametros parametros, double tempoAnual, DateTime dataVencimento)
        {
            if (tempoAnual <= 0)
                throw new ArgumentException("Tempo deve ser positivo.");

            double sqrtT = Math.Sqrt(tempoAnual);
            double sigma2 = parametros.Volatilidade * parametros.Volatilidade;

            double d1 = (Math.Log(parametros.Preco / parametros.Strike) + tempoAnual * (parametros.TaxaDeCarrego + 0.5 * sigma2))
                        / (parametros.Volatilidade * sqrtT);
            double d2 = d1 - parametros.Volatilidade * sqrtT;

            double Nd1 = ExcelFunctions.NormSDist(d1);
            double Nd2 = ExcelFunctions.NormSDist(d2);

            double desconto = Math.Exp(-parametros.TaxaDeCarrego * tempoAnual);

            double call = parametros.Preco * Nd1 - desconto * parametros.Strike * Nd2;
            double put = desconto * parametros.Strike * ExcelFunctions.NormSDist(-d2) - parametros.Preco * ExcelFunctions.NormSDist(-d1);

            return new BlackScholesResultado(parametros, tempoAnual, dataVencimento, call, put);
        }
    }
}
