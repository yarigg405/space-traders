using Assets.Code.UI.Screens;
using UnityEngine;
using VContainer;
using Yrr.UI.Infrastructure;


namespace Assets.Code.Infrastructure.Loading
{
    public sealed class MenuSceneStarter : MonoBehaviour
    {
        [Inject] private readonly IUIManager _uiManager;

        private void Start()
        {
            _uiManager.Show<MainScreenMenu>();
        }
    }
}