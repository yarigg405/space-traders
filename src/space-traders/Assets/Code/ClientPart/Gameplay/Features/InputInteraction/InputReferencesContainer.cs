using System;
using VContainer.Unity;


namespace Assets.Code.ClientPart.Gameplay.Features.InputInteraction
{
    public sealed class InputReferencesContainer : IInitializable, IDisposable
    {
        public InputSystem_Actions Actions => _actions;

        private readonly InputSystem_Actions _actions = new();


        void IInitializable.Initialize()
        {
            _actions.Enable();
        }

        void IDisposable.Dispose()
        {
            _actions.Disable();
        }
    }
}
