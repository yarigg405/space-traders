using Assets.Code.Infrastructure.States.GameStates;
using Assets.Code.Infrastructure.States.StateMachine;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using Yrr.UI;


namespace Assets.Code.UI.Screens
{
    public class MainScreenMenu : MainScreen
    {
        [Inject] private readonly IStateMachine _stateMachine;

        [SerializeField] private Button _playButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _testModalButton;

        private void OnEnable()
        {
            _playButton.onClick.AddListener(ClickOnPlay);
            _settingsButton.onClick.AddListener(ClickOnSettings);
            _testModalButton.onClick.AddListener(ClickOnTestModal);
        }

        private void OnDisable()
        {
            _playButton.onClick.RemoveListener(ClickOnPlay);
            _settingsButton.onClick.RemoveListener(ClickOnSettings);
            _testModalButton.onClick.RemoveListener(ClickOnTestModal);
        }

        private void ClickOnPlay()
        {
            _stateMachine.Enter<LoadBattleState, string>("GameScene");
        }

        private void ClickOnSettings()
        {
            UIManager.Show<SettingsScreen>();
        }

        private void ClickOnTestModal()
        {
            UIManager.Show<TestModalScreen>();
        }
    }
}