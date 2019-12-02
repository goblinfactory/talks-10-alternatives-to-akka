using Akka.Actor;

namespace QuoteClient.Akka.Commands
{
    /// <summary>
    /// allows us to create all the other actors, and then pass the UX to all the other actors after it has been created
    /// so that we can avoid circular dependancy problems.
    /// </summary>
    public class UserInterfaceReady
    {
        public UserInterfaceReady(IActorRef ux) 
        {
            UX = ux;
        }

        public IActorRef UX { get; }
    }
}
