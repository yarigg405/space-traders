using Cysharp.Threading.Tasks;
using Riptide;
using System;
using System.Collections.Generic;
using System.Threading;


namespace Assets.Code.Networking.RequestSystem
{
    public sealed class NetworkRequestSystem
    {
        private readonly Client _client;
        private readonly Dictionary<uint, UniTaskCompletionSource<IResponse>> _pending = new();

        private uint _requestIdCounter;

        public NetworkRequestSystem(Client client)
        {
            _client = client;
        }

        public async UniTask<TResponse> Send<TResponse>(
            IRequest<TResponse> request, CancellationToken ct, TimeSpan? timeout = null)
              where TResponse : IResponse, new()
        {
            var id = ++_requestIdCounter;
            var message = Message.Create(MessageSendMode.Reliable, request.MessageType);
            message.AddUInt(id);
            request.Serialize(message);

            var tcs = new UniTaskCompletionSource<IResponse>();
            _pending[id] = tcs;

            try
            {
                _client.Send(message);
                var task = tcs.Task.AttachExternalCancellation(ct);

                if (timeout.HasValue)
                    task = task.Timeout(timeout.Value);
                var response = await task;

                return (TResponse)response;
            }

            finally
            {
                _pending.Remove(id);
            }                
        }

        public void Resolve(uint requestId, IResponse response)
        {
            if (_pending.TryGetValue(requestId, out var tcs))
            {
                tcs.TrySetResult(response);
            }
        }

        public void Reject(uint requestId, string reason)
        {
            if (_pending.TryGetValue(requestId, out var tcs))
            {
                tcs.TrySetException(new(reason));
            }
        }

        public void CancelAll()
        {
            foreach (var kv in _pending)
            {
                kv.Value.TrySetCanceled();
            }

            _pending.Clear();
        }
    }
}
