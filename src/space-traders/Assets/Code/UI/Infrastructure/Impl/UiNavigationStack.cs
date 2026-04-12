using Assets.Code.UI.Infrastructure.Interfaces;
using System.Collections.Generic;


namespace Assets.Code.UI.Infrastructure.Impl
{
    internal sealed class UiNavigationStack
    {
        private readonly IScreensProvider _provider;
        private readonly Stack<INavigationIntent> _stack = new();

        public UiNavigationStack(IScreensProvider provider)
        {
            _provider = provider;
        }

        public (IScreen opened, IScreen closed) Push(INavigationIntent intent)
        {
            IScreen closed = null;

            if (_stack.Count > 0)
            {
                var current = _stack.Peek();
                closed = current.ExecuteHide(_provider);
            }

            _stack.Push(intent);
            var opened = intent.Execute(_provider);

            return (opened, closed);
        }

        public (IScreen opened, IScreen closed) Pop()
        {
            if (_stack.Count <= 1)
                return (null, null);

            var current = _stack.Pop();
            var closed = current.ExecuteHide(_provider);

            var previous = _stack.Peek();
            var opened = previous.Execute(_provider);

            return (opened, closed);
        }

        public void Clear()
        {
            while (_stack.Count > 0)
            {
                var intent = _stack.Pop();
                intent.ExecuteHide(_provider);
            }
        }
    }
}
