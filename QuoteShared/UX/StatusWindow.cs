using Konsole;
using QuoteClient.Akka.UserInterface;
using System;

namespace QuoteShared.UX
{
    public class StatusWindow
    {
        private readonly IConsole _console;

        public StatusWindow(IConsole console)
        {
            _console = console;
            _console.PrintAt(0, 0, "Backlog: ");
        }

        public void Update(BacklogCountChanged change)
        {
            _console.PrintAtColor(ConsoleColor.Yellow, 9, 0, change.NewValue.ToString(), null);
        }
    }
}
