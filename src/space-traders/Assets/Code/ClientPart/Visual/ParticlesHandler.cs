using Assets.Code.ClientPart.Visual.Player;
using System;
using Unity.Mathematics;
using UnityEngine;
using VContainer.Unity;


namespace Assets.Code.ClientPart.Visual
{
    internal sealed class ParticlesHandler : IStartable, IDisposable
    {
        private readonly PlayerQuadrantChangeObserver _playerObserver;

        public ParticlesHandler(PlayerQuadrantChangeObserver playerObserver)
        {
            _playerObserver = playerObserver;
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
            Debug.Log("Quadrant Changed");
        }
    }
}
