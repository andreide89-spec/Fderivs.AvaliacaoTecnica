namespace Fderivs.Domain.BlackScholes.ValueObjects
{
    public record BlackScholesParametros
    {
        public double Preco { get; }
        public double Strike { get; }
        public double TaxaDeCarrego { get; }
        public double Volatilidade { get; }

        public BlackScholesParametros(double preco, double strike, double taxaDeCarrego, double volatilidade)
        {
            if (preco <= 0) throw new ArgumentException("Preço deve ser positivo.");
            if (strike <= 0) throw new ArgumentException("Strike deve ser positivo.");
            if (volatilidade <= 0) throw new ArgumentException("Volatilidade deve ser positiva.");

            Preco = preco;
            Strike = strike;
            TaxaDeCarrego = taxaDeCarrego;
            Volatilidade = volatilidade;
        }
    }

}
