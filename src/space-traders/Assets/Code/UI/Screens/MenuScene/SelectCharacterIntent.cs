using Assets.Code.UI.Infrastructure.Impl;
using Assets.Code.UI.Infrastructure.Interfaces;
using Cysharp.Threading.Tasks;
using System.Threading;


namespace Assets.Code.UI.Screens.MenuScene
{
    public sealed class SelectCharacterIntent : IAsyncNavigationIntent
    {
        UniTask<object> IAsyncNavigationIntent.Load(CancellationToken token)
        {            
            здесь загрузить персонажей
        }

        INavigationIntent IAsyncNavigationIntent.Create(object data)
        {
            return new OpenScreenIntent(typeof(SelectCharacterScreen), data);
        }
    }
}
