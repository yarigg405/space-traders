using UnityEngine;
using UnityEngine.UI;


namespace Assets.Code.UI.Screens.ItemInfo
{
    public sealed class ItemInfoTabView : MonoBehaviour
    {
        [field: SerializeField] public Button TabButton { get; private set; }
        [SerializeField] private GameObject _selectedIndicator;

        [Space]
        [SerializeField] private GameObject _page;
        [field: SerializeField] public string TabId { get; private set; }


        public void SetActive(bool active)
        {
            _page.SetActive(active);
            _selectedIndicator.SetActive(active);
        }

        public void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }
    }
}
