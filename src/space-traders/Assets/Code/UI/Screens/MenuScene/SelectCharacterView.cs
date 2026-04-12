using Assets.Code._Tempo;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yrr.Utils;


namespace Assets.Code.UI.Screens.MainMenu
{
    internal sealed class SelectCharacterView : UIScreenView
    {
        [field: SerializeField] public Button CreateCharacterButton { get; private set; }
        public event Action<string> OnCharacterSelected;

        [SerializeField] private SelectCharacterCardView _characterCardPrefab;
        [SerializeField] private Transform _cardsRoot;

        private List<SelectCharacterCardView> _cards = new();

        internal void SetupCharacters(IEnumerable<CharacterData> characters)
        {
            ClearCards();
            foreach (var character in characters)
            {
                var card = Instantiate(_characterCardPrefab, _cardsRoot);
                card.SetupName(character.Name);
                card.SelectCharacterButton.onClick.AddListener(() => SelectCharacter(character));
                _cards.Add(card);
            }
        }

        private void ClearCards()
        {
            foreach (var card in _cards)
            {
                card.SelectCharacterButton.onClick.RemoveAllListeners();
            }

            _cards.Clear();
            _cardsRoot.ClearChildren();
        }

        private void SelectCharacter(CharacterData character)
        {
            OnCharacterSelected?.Invoke(character.Guid);
        }
    }
}
