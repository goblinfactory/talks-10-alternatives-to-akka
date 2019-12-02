using QuoteShared;

namespace QuoteClient.Akka.Commands
{
    public class RunProgram 
    {
        private readonly InsuranceProvider[] _providers;

        public RunProgram(InsuranceProvider[] providers)
        {
            _providers = providers;
        }
    }
}
