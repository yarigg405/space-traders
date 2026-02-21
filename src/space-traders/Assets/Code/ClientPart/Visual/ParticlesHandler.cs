using Assets.Code.ClientPart.Gameplay.Features.Player.Infrastructure;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;
using VContainer.Unity;


namespace Assets.Code.ClientPart.Visual
{
    internal sealed class ParticlesHandler : IAsyncStartable, IDisposable
    {
        private readonly IPlayerProvider _playerProvider;
        private readonly HashSet<HandledParticle> _handledParticles = new();


        public ParticlesHandler(IPlayerProvider playerProvider)
        {
            _playerProvider = playerProvider;
        }



        async UniTask IAsyncStartable.StartAsync(CancellationToken cancellation)
        {
            await UniTask.WaitUntil(() => _playerProvider.PlayerEntity != null);
            _playerProvider.PlayerEntity.ViewModel.QuadrantIndex.OnChange += OnPlayerQuadrantChanged;
        }

        void IDisposable.Dispose()
        {
            _playerProvider.PlayerEntity.ViewModel.QuadrantIndex.OnChange -= OnPlayerQuadrantChanged;
        }

        private void OnPlayerQuadrantChanged(int2 playerQuadrant)
        {
            Debug.Log("Quadrant changed");

            var entity = _playerProvider.PlayerEntity;
            var delta = entity.LocalPosition - entity.PreviousFrameLocalPosition;

            foreach (var handledParticle in _handledParticles)
            {
                handledParticle.TeleportOffset(delta);
            }
        }

        internal void AddParticle(HandledParticle handledParticle)
        {
            _handledParticles.Add(handledParticle);
        }

        internal void RemoveParticle(HandledParticle handledParticle)
        {
            _handledParticles.Remove(handledParticle);
        }
    }
}
