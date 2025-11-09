using Assets.Code.Infrastructure.Loading;
using Assets.Code.Infrastructure.States.GameStates;
using Assets.Code.Infrastructure.States.StateMachine;
using UnityEngine;
using UnityEngine.UI;
using VContainer;


namespace Assets.Code.ClientPart.UI
{
    public sealed class GameTestScreen : MonoBehaviour
    {
        [SerializeField] private Button _scene1Button;
        [SerializeField] private Button _scene2Button;

        [Inject] private readonly IScenesLoader _scenesLoader;
        [Inject] private readonly IStateMachine _stateMachine;

        private void OnEnable()
        {
            _scene1Button.onClick.AddListener(ClickOnScene1);
            _scene2Button.onClick.AddListener(ClickOnScene2);
        }

        private void OnDisable()
        {
            _scene1Button.onClick.RemoveListener(ClickOnScene1);
            _scene2Button.onClick.RemoveListener(ClickOnScene2);
        }

        private void ClickOnScene1()
        {
            LoadScene(SceneNames.GameScene1);
        }

        private void ClickOnScene2()
        {
            LoadScene(SceneNames.GameScene2);
        }

        private void LoadScene(string sceneName)
        {
            if (sceneName.Equals(_scenesLoader.CurrentScene)) return;

            _stateMachine.Enter<ChangeGameSceneState, string>(sceneName);
        }
    }
}
