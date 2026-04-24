using Assets.Code.Networking;
using Assets.Code.ServerPart.Networking;
using Cysharp.Threading.Tasks;
using Riptide;
using System;
using System.Collections.Generic;
using System.Threading;


namespace Assets.Code.ClientPart.Networking
{
    internal sealed class NetworkRequestSystem
    {
        private readonly NetworkManager _networkManager;
        private readonly ILifetimeCancellationToken _lifetimeToken;

        private uint _requestId;
        private readonly Dictionary<uint, UniTaskCompletionSource<Message>> _pending = new();

        public NetworkRequestSystem(NetworkManager networkManager, ILifetimeCancellationToken lifetimeToken)
        {
            _networkManager = networkManager;
            _lifetimeToken = lifetimeToken;
        }

        public async UniTask<Message> SendRequest(Message msg, CancellationToken externalCt, TimeSpan timeout)
        {
            var linkedCts = CancellationTokenSource
                .CreateLinkedTokenSource(_lifetimeToken.Token, externalCt);
            var ct = linkedCts.Token;
            var id = ++_requestId;

            msg.AddUInt(id);

            var tcs = new UniTaskCompletionSource<Message>();
            _pending[id] = tcs;

            try
            {
                _networkManager.Client.Send(msg);

                return await tcs.Task
                    .AttachExternalCancellation(ct)
                    .Timeout(timeout);
            }
            finally
            {
                _pending.Remove(id);
                linkedCts.Dispose();
            }
        }

        public void SetResponseOk(Message msg)
        {
            var requestId = msg.GetUInt();
            Resolve(requestId, msg);
        }

        public void SetResponseFailed(Message msg)
        {
            var requestId = msg.GetUInt();
            var error = msg.GetString();
            Reject(requestId, error);
        }

        private void Resolve(uint id, Message msg)
        {
            if (_pending.TryGetValue(id, out var tcs))
                tcs.TrySetResult(msg);
        }

        private void Reject(uint id, string error)
        {
            if (_pending.TryGetValue(id, out var tcs))
                tcs.TrySetException(new RequestFailedException(error));
        }
    }
}
