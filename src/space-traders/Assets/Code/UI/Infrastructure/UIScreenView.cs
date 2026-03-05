using UnityEngine;
using UnityEngine.UI;


namespace Assets.Code.UI.Infrastructure
{
    public abstract class UIScreenView : MonoBehaviour
    {
        [field: SerializeField] public Button CloseButton { get; private set; }
        [field: SerializeField] public Canvas Canvas { get; private set; }

        private void OnValidate()
        {
            if (!Canvas)
                Canvas = GetComponent<Canvas>();
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
