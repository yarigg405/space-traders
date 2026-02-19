using Assets.Code.ClientPart.Gameplay.Features.Player.Infrastructure;
using Assets.Code.ClientPart.Visual.Player;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using VContainer.Unity;


namespace Assets.Code.ClientPart.Visual
{
    internal sealed class ParticlesHandler : IStartable, IDisposable
    {
        private readonly PlayerViewModel _playerObserver;
        private readonly IPlayerProvider _playerProvider;

        private readonly HashSet<HandledParticle> _handledParticles = new();


        public ParticlesHandler(PlayerViewModel playerObserver, IPlayerProvider playerProvider)
        {
            _playerObserver = playerObserver;
            _playerProvider = playerProvider;
        }

        void IStartable.Start()
        {
            _playerObserver.PlayerQuadrant.OnChange += OnPlayerQuadrantChanged;
        }

        void IDisposable.Dispose()
        {
            _playerObserver.PlayerQuadrant.OnChange -= OnPlayerQuadrantChanged;
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
