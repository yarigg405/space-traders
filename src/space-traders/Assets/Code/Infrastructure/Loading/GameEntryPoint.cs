using Assets.Code.Infrastructure.States.StateMachine;
using VContainer.Unity;


namespace Assets.Code.Infrastructure.Loading
{
    internal sealed class GameEntryPoint : IStartable
    {
        private readonly IStateMachine _stateMachine;

        public GameEntryPoint(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        void IStartable.Start()
        {
            
        }
    }
}
