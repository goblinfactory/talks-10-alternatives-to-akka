using Konsole;
using Konsole.Layouts;
using QuoteShared.UX;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuoteShared.UX
{
    public class UserInterface
    {
        private IConsole _parent;
        private IConsole _console;
        private IConsole _left;
        private IConsole _right;
        private IConsole _current;
        private IConsole _panel;

        public IConsole Backlog { get; private set; }

        public StatusWindow Status { get; private set; }

        public IConsole Panel { get; private set; }

        public UserInterface(IConsole console = null)
        {
            Console.CursorVisible = false;
            _parent = console ?? new Window();
            var _left = _parent.SplitLeft();
            var _right = _parent.SplitRight();
            Backlog = _left.SplitTop("backlog");
            var _current = _left.SplitBottom("current");
            Panel = _right.SplitTop("quote panel");
            var status = _right.SplitBottom("status");
            Status = new StatusWindow(status);
            Console.CursorVisible = false;
        }
    }
}
