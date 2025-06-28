namespace Yrr.UI.Infrastructure
{
    internal interface IScreenSupplier<TKey>
    {
        IUIScreen GetScreen<T>();
    }
}
