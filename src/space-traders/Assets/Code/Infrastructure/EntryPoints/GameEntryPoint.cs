using Assets.Code.Infrastructure.States.GameStates;
using Assets.Code.Infrastructure.States.StateMachine;
using UnityEngine;
using VContainer.Unity;


namespace Assets.Code.Infrastructure.EntryPoints
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
           // Application.targetFrameRate = 10;
            _stateMachine.Enter<BootstrapState>();
        }
    }
}
