using Assets.Code.ClientPart.Networking;
using Assets.Code.Infrastructure.States.GameStates;
using Assets.Code.Networking.Data;
using Assets.Code.UI.Infrastructure.Interfaces;
using Assets.Code.UI.Screens.MainMenu;
using System.Collections.Generic;


namespace Assets.Code.UI.Screens.MenuScene
{
    public sealed class SelectCharacterPresenter : IPresenter<SelectCharacterView>
    {
        private readonly IUIManager _uiManager;
        private readonly GameEnterManager _enterManager;

        public SelectCharacterPresenter(IUIManager uiManager, GameEnterManager enterManager)
        {
            _uiManager = uiManager;
            _enterManager = enterManager;
        }

        void IPresenter<SelectCharacterView>.Show(SelectCharacterView view, object args)
        {
            if (args is not IEnumerable<CharacterData> characters)
            {
                throw new("SelectCharacterPresenter need characters args");
            }

            view.SetupCharacters(characters);
            view.CreateCharacterButton.onClick.AddListener(ClickOnCreateCharacter);
            view.OnCharacterSelected += OnCharacterSelected;
            view.CloseButton.onClick.AddListener(ClickOnBack);
        }

        void IPresenter<SelectCharacterView>.Hide(SelectCharacterView view)
        {
            view.Hide();

            view.CreateCharacterButton.onClick.RemoveListener(ClickOnCreateCharacter);
            view.OnCharacterSelected -= OnCharacterSelected;
            view.CloseButton.onClick.RemoveListener(ClickOnBack);
        }

        private void ClickOnCreateCharacter()
        {
            _uiManager.GoToScreen<CreateCharacterScreen>();
        }

        private void OnCharacterSelected(int characterId)
        {
            _enterManager.EnterGame(characterId);
        }

        private void ClickOnBack()
        {
            _uiManager.BackToPreviousScreen();
        }
    }
}
