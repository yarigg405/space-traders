using Entitas;


namespace Assets.Code.Common.Time.Systems
{
    internal sealed class TickIncrementSystem : IExecuteSystem
    {
        private readonly TickCounter _tick;

        public TickIncrementSystem(TickCounter tick)
        {
            _tick = tick;
        }

        void IExecuteSystem.Execute()
        {
            _tick.Tick();
        }
    }
}
