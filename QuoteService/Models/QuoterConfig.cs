namespace QuoteService.Models
{
    public class QuoterConfig : IQuoterConfig
    {
        public QuoterConfig(string name, int speed, int port, string availability)
        {
            Name = name;
            Speed = speed;
            Port = port;
            Availability = availability;
        }

        public string Name { get; }
        public int Speed { get; }
        public int Port { get; }
        public string Availability { get; }
    }
}