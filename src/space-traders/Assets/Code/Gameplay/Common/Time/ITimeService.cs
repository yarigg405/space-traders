using System;

namespace Assets.Code.Gameplay.Common.Time
{
    public interface ITimeService
    {
        float DeltaTime { get; }
        DateTime UtcNow { get; }

        void StartTime();
        void StopTime();
    }
}