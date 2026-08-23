namespace Assets.Code.Common.Time
{
    public sealed class TickCounter
    {
        public uint CurrentTick { get; private set; }

        public void Tick() => CurrentTick++;
        public void SetupTick(uint tick) => CurrentTick = tick;
    }
}
