using Assets.Code.UI.Infrastructure.Interfaces;
using System.Collections.Generic;


namespace Assets.Code.UI.Infrastructure.Impl
{
    internal sealed class UiNavigationStack
    {
        private readonly Stack<INavigationRequest> _stack = new();

        public void Push(INavigationRequest request)
        {
            _stack.Push(request);
        }

        public INavigationRequest Pop()
        {
            if (_stack.Count <= 1)
                return null;

            _stack.Pop();
            return _stack.Peek();
        }

        public void Clear()
        {
            _stack.Clear();
        }

        public INavigationRequest Peek() =>
            _stack.Count > 0 ? _stack.Peek() : null;
    }
}
