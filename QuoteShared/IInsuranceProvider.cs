namespace QuoteShared
{
    public interface IInsuranceProvider
    {
        string Name { get; }

        /// <summary>
        /// used for each different test panel
        /// </summary>
        int Speed { get; }

        /// <summary>
        /// tcp port that the webapi quote service will be bound to.
        /// </summary>
        int Port { get; }

        /// <summary>
        /// Excellent, Average, Spotty, Bad
        /// </summary>
        string Availability { get; }
    }
}
