using Assets.Code.ClientPart.Gameplay.Features.Player.Infrastructure;
using Unity.Mathematics;
using VContainer.Unity;
using Yrr.Utils;


namespace Assets.Code.ClientPart.Visual.Player
{
    public sealed class PlayerQuadrantChangeObserver : ILateTickable
    {
        private readonly IPlayerProvider _playerProvider;
        public ReactiveValue<int2> PlayerQuadrant = new();


        public PlayerQuadrantChangeObserver(IPlayerProvider playerProvider)
        {
            _playerProvider = playerProvider;
        }


        void ILateTickable.LateTick()
        {
            if(_playerProvider.PlayerEntity==null) return;

            PlayerQuadrant.Value = _playerProvider.PlayerEntity.QuadrantIndex;
        }
    }
}