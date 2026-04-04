namespace Assets.Code.UI.Infrastructure
{
    public interface IScreen
    {
        void Show(object args);
        void Hide();
    }
}