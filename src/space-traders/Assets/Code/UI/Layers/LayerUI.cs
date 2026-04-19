using UnityEngine;


namespace Assets.Code.UI.Layers
{
    public abstract class LayerUI : MonoBehaviour
    {
        public virtual void ShowView(UIScreenView view)
        {
            view.transform.SetParent(transform);
            view.transform.SetAsLastSibling();
            view.transform.localPosition = Vector3.zero;
            view.Show();
        }

        public virtual void HideView(UIScreenView view)
        {
            view.Hide();
            view.transform.SetAsFirstSibling();
        }
    }
}
