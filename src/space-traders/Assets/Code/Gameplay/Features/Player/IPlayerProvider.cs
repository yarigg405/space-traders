using System;

namespace Assets.Code.Gameplay.Features.Player
{
    public interface IPlayerProvider
    {
        GameEntity PlayerEntity { get; }

        event Action OnPlayerInitizlized;
    }
}