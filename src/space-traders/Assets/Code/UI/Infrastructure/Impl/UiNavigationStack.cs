using System;
using System.Collections.Generic;


namespace Assets.Code.UI.Infrastructure.Impl
{
    internal sealed class UiNavigationStack
    {
        private readonly IScreensProvider _provider;
        private readonly Stack<ScreenState> _stack = new();

        public UiNavigationStack(IScreensProvider provider)
        {
            _provider = provider;
        }

        public (IScreen opened, IScreen closed) Push(Type type, object args)
        {
            IScreen closed = null;
            if (_stack.Count > 0)
            {
                var current = _stack.Peek();
                closed = _provider.GetScreen(current.ScreenType);
                closed.Hide();
            }

            var state = new ScreenState(type, args);
            _stack.Push(state);

            var opened = _provider.GetScreen(type);
            opened.Show(args);

            return (opened, closed);
        }

        public (IScreen opened, IScreen closed) Pop()
        {
            if (_stack.Count <= 1)
                return (null, null);

            var current = _stack.Pop();
            var closed = _provider.GetScreen(current.ScreenType);
            closed.Hide();

            var previous = _stack.Peek();
            var opened = _provider.GetScreen(previous.ScreenType);
            opened.Show(previous.Args);

            return (opened, closed);
        }

        public void Clear()
        {
            while (_stack.Count > 0)
            {
                var state = _stack.Pop();
                _provider.GetScreen(state.ScreenType).Hide();
            }
        }
    }
}
