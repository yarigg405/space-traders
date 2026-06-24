using Assets.Code.ClientPart.Gameplay.Features.InputInteraction;
using System;
using VContainer.Unity;


namespace Assets.Code.ClientPart.Gameplay.Features.Navigation
{
    internal sealed class NavigationSelectionRouter : IInitializable, IDisposable
    {
        private readonly MouseClickDetector _clickDetector;
        private readonly SelectionService _selectionService;

        public NavigationSelectionRouter(MouseClickDetector clickDetector, SelectionService selectionService)
        {
            _clickDetector = clickDetector;
            _selectionService = selectionService;
        }

        void IInitializable.Initialize()
        {
            _clickDetector.OnObjectClicked += OnObjectClicked;
        }

        void IDisposable.Dispose()
        {
            _clickDetector.OnObjectClicked -= OnObjectClicked;
        }

        private void OnObjectClicked(ClickableEntity clickable)
        {
            _selectionService.Select(clickable.Entity);
        }
    }
}
