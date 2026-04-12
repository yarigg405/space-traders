using Cysharp.Threading.Tasks;
using System.Threading;


namespace Assets.Code.UI.Infrastructure.Interfaces
{
    public interface IAsyncNavigationIntent : INavigationRequest
    {
        UniTask<object> Load(CancellationToken token);
        INavigationIntent Create(object data);
    }
}
