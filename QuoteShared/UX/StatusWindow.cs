using Konsole;
using System;
using System.Collections.Generic;
using System.Text;

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

        public void Refresh(IStatus status)
        {
            _console.PrintAtColor(ConsoleColor.Yellow, 9, 0, status.Backlog.ToString(), null);
        }
    }
}
