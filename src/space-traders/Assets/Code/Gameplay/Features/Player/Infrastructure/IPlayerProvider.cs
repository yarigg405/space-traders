using System;


namespace Assets.Code.Gameplay.Features.Player.Infrastructure
{
    public interface IPlayerProvider
    {
        GameEntity PlayerEntity { get; }

        event Action OnPlayerInitizlized;
    }
}