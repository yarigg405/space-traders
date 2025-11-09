using System;


namespace Assets.Code.Common.Time
{
    public interface ITimeService
    {
        float DeltaTime { get; }
        DateTime UtcNow { get; }

        void StartTime();
        void StopTime();
    }
}