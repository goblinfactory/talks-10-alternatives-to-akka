using Konsole;
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
            _console.PrintAtColor(ConsoleColor.DarkGray, 0, 1, "Spread : ", null);
            _console.CursorTop = 3;
        }

        public void Update(BacklogCountChanged change)
        {
            _console.PrintAtColor(ConsoleColor.Cyan, 9, 0, change.NewValue.ToString() + "  ", null);
        }
        public void Update(SpreadChanged change)
        {
            _console.PrintAtColor(ConsoleColor.DarkGray, 9, 1, change.NewValue.ToString() + "  ", null);
        }

        public void WriteLine(string message)
        {
            _console.WriteLine(ConsoleColor.DarkGray, message);
        }

    }
}
