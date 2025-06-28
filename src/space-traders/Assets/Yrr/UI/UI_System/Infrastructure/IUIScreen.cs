namespace Yrr.UI.Infrastructure
{
    public interface IUIScreen
    {
        void Hide();
        void SetupUiManager(IUIManager manager);
        bool IsModal { get; }
    }
}
