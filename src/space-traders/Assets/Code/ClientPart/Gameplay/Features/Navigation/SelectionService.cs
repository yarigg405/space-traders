using System;
using Entitas;


namespace Assets.Code.ClientPart.Gameplay.Features.Navigation
{
    public sealed class SelectionService
    {
        private GameEntity _selected;

        public GameEntity Selected => _selected;
        public event Action<GameEntity> SelectionChanged;

        public void Select(GameEntity entity)
        {
            if (entity == _selected) return;

            Unhook();
            _selected = entity;

            if (_selected != null)
                _selected.OnDestroyEntity += OnSelectedDestroyed;

            SelectionChanged?.Invoke(_selected);
        }

        public void Clear()
        {
            if (_selected == null) return;

            Unhook();
            _selected = null;
            SelectionChanged?.Invoke(null);
        }

        private void OnSelectedDestroyed(IEntity entity)
        {
            Clear();
        }

        private void Unhook()
        {
            if (_selected != null)
                _selected.OnDestroyEntity -= OnSelectedDestroyed;
        }
    }
}
