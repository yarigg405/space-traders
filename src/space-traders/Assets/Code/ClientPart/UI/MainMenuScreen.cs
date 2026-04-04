//using Assets.Code.Infrastructure.States.GameStates;
//using Assets.Code.Infrastructure.States.StateMachine;
//using Assets.Code.Networking;
//using Cysharp.Threading.Tasks;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;
//using VContainer;


//namespace Assets.Code.ClientPart.UI
//{
//    public sealed class MainMenuScreen : MonoBehaviour
//    {
//        [Inject] private readonly NetworkManager _networkManager;
//        [Inject] private readonly IStateMachine _stateMachine;

//        [SerializeField] private Button _hostButton;
//        [SerializeField] private Button _clientButton;

//        [SerializeField] private TMP_InputField _playerLoginIF;

//        private const string _playerPrefsKey = "PlayerLogin";

//        private void OnEnable()
//        {
//            if (GetProjectName() == "space-traders")
//            {
//                _playerLoginIF.text = GetSavedPlayerLogin();

//            }
//            else
//            {
//                _playerLoginIF.text = "yarigg2";
//            }

//            _hostButton.onClick.AddListener(OnHostClick);
//            _clientButton.onClick.AddListener(OnClientClick);
//        }

//        private void OnDisable()
//        {
//            _hostButton.onClick.RemoveListener(OnHostClick);
//            _clientButton.onClick.RemoveListener(OnClientClick);
//        }

//        private void OnHostClick()
//        {
//            OnHostClickAsync().Forget();
//        }

//        private void OnClientClick()
//        {
//            OnClientClickedAsync().Forget();
//        }

//        private async UniTask OnHostClickAsync()
//        {
//            await _networkManager.StartHost();
//            _stateMachine.Enter<LoadGameSceneState>();
//        }

//        private async UniTask OnClientClickedAsync()
//        {
//            await _networkManager.StartClient();
//            _stateMachine.Enter<LoadGameSceneState>();
//        }

//        private string GetProjectName()
//        {
//            string[] s = Application.dataPath.Split('/');
//            string projectName = s[s.Length - 2];
//            return projectName;
//        }

//        private string GetSavedPlayerLogin()
//        {
//            if (!PlayerPrefs.HasKey(_playerPrefsKey))
//            {
//                PlayerPrefs.SetString(_playerPrefsKey, "Player" + Random.Range(64, 512));
//            }

//            return PlayerPrefs.GetString(_playerPrefsKey);
//        }
//    }
//}