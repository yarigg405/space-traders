using Assets.Code.Networking;
using Assets.Code.Networking.Data;
using Assets.Code.UI.Screens.StationScreens;
using Cysharp.Threading.Tasks;
using Riptide;
using System;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;


namespace Assets.Code.ClientPart.Networking
{
    public sealed class ClientMessenger
    {
        private readonly NetworkRequestSystem _requestSystem;
        private readonly NetworkManager _networkManager;

        internal ClientMessenger(NetworkRequestSystem requestSystem, NetworkManager networkManager)
        {
            _requestSystem = requestSystem;
            _networkManager = networkManager;
        }

        public async UniTask<EnterGameResult> RequestForEnterTheGame(int selectedCharacterId, CancellationToken ct)
        {
            var msg = Message.Create(MessageSendMode.Reliable, ClientToServerMessageType.RequestEnterTheGame)
                .AddInt(selectedCharacterId);

            var response = await _requestSystem.SendRequest(
                msg, ct, TimeSpan.FromSeconds(5));

            var result = new EnterGameResult
            {
                IsStation = response.GetBool(),
            };

            if (!result.IsStation)
            {
                result.SpaceScene = new SpaceSceneData
                {
                    SceneName = response.GetString(),
                    ConfigJson = response.GetString(),
                };
            }

            return result;
        }

        public async UniTask<LoadStationData> RequestForLoadStation(int selectedCharacterId, CancellationToken ct)
        {
            var msg = Message.Create(MessageSendMode.Reliable, ClientToServerMessageType.RequestForStationSceneData)
                .AddInt(selectedCharacterId);

            var response = await _requestSystem.SendRequest
                (msg, ct, TimeSpan.FromSeconds(5));

            var result = new LoadStationData
            {
                StationId = response.GetInt(),
                StationName = response.GetString(),
                StarSystemName = response.GetString(),
                StationType = response.GetInt(),
                ShipModelId = response.GetString(),
                ShipFitJson = response.GetString(),
            };

            return result;
        }

        public async UniTask<StationTradeData> RequestForStationTradeData(int stationId, CancellationToken ct)
        {
            var msg = Message.Create(MessageSendMode.Reliable, ClientToServerMessageType.RequestForStationTradeData)
                .AddInt(stationId);

            var response = await _requestSystem.SendRequest
                (msg, ct, TimeSpan.FromSeconds(5));

            var result = new StationTradeData
            {
                CurrentStationId = response.GetInt(),
            };

            var stationCount = response.GetInt();
            result.Stations = new List<StationOrdersData>(stationCount);

            for (int i = 0; i < stationCount; i++)
            {
                var station = new StationOrdersData
                {
                    StationId = response.GetInt(),
                    StationName = response.GetString(),
                    PositionX = response.GetDouble(),
                    PositionY = response.GetDouble(),
                    StarSystemId = response.GetInt(),
                    StarSystemName = response.GetString(),
                    BuyOrders = ReadOrders(response),
                    SellOrders = ReadOrders(response),
                };

                result.Stations.Add(station);
            }

            return result;
        }

        private static List<TradeOrderData> ReadOrders(Message response)
        {
            var count = response.GetInt();
            var orders = new List<TradeOrderData>(count);

            for (int i = 0; i < count; i++)
            {
                orders.Add(new TradeOrderData
                {
                    ItemId = response.GetString(),
                    Price = response.GetLong(),
                    Quantity = response.GetInt(),
                    ExpiresAt = response.GetLong(),
                });
            }

            return orders;
        }

        public async UniTask<IEnumerable<CharacterData>> RequestForCharacters(string login, string password, CancellationToken ct)
        {
            var msg = Message.Create(MessageSendMode.Reliable, ClientToServerMessageType.RequestGetCharacters)
                .AddString(login)
                .AddString(password);

            var response = await _requestSystem.SendRequest(
                msg, ct, TimeSpan.FromSeconds(5));

            var count = response.GetInt();
            var result = new List<CharacterData>(count);
            for (int i = 0; i < count; i++)
            {
                var character = new CharacterData
                {
                    Id = response.GetInt(),
                    Name = response.GetString(),
                };
                result.Add(character);
            }
            return result;
        }

        public async UniTask<bool> RequestForCreateNewCharacter(string login, CharacterData character, CancellationToken ct)
        {
            var msg = Message.Create(MessageSendMode.Reliable, ClientToServerMessageType.RequestCreateCharacter)
                .AddString(login)
                .AddString(character.Name);

            var response = await _requestSystem.SendRequest(
                msg, ct, TimeSpan.FromSeconds(5));

            if (response != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async UniTask<string> RequestForDock(int stationId, int dockingBayIndex,  CancellationToken ct)
        {
            var msg = Message.Create(MessageSendMode.Reliable, ClientToServerMessageType.RequestForDock)
                .AddInt(stationId)
                .AddInt(dockingBayIndex)
                ;

            var response = await _requestSystem.SendRequest
                (msg,ct, TimeSpan.FromSeconds(5));

            var result = response.GetString();
            return result;
        }

        public async UniTask<SpaceSceneData> RequestForUndock(CancellationToken ct)
        {
            var msg = Message.Create(MessageSendMode.Reliable, ClientToServerMessageType.RequestForUndock);

            var response = await _requestSystem.SendRequest
                (msg, ct, TimeSpan.FromSeconds(5));

            var result = new SpaceSceneData
            {
                SceneName = response.GetString(),
                ConfigJson = response.GetString(),
            };
            return result;
        }

        public async UniTask<long> RequestForMoney(CancellationToken ct)
        {
            var msg = Message.Create(MessageSendMode.Reliable, ClientToServerMessageType.RequestForMoney);

            var response = await _requestSystem.SendRequest
                (msg, ct, TimeSpan.FromSeconds(5));

            return response.GetLong();
        }

        public void RequestForLoadingSceneEntities()
        {
            var message = Message.Create(MessageSendMode.Reliable, ClientToServerMessageType.RequestForSceneEntities);
            _networkManager.Client.Send(message);
        }



        public void SendTargetRotationToServer(float targetRotation)
        {
            var message = Message.Create(MessageSendMode.Unreliable, ClientToServerMessageType.SendTargetRotation)
                .AddFloat(targetRotation);
            _networkManager.Client.Send(message);
        }

        public void SendSpeedModifierToServer(float speedModifier)
        {
            var message = Message.Create(MessageSendMode.Unreliable, ClientToServerMessageType.SendSpeedModifier)
                .AddFloat(speedModifier);
            _networkManager.Client.Send(message);
        }

        public void SendKeepDistance(uint targetId, Vector2 minMaxDistance)
        {
            var message = Message.Create(MessageSendMode.Reliable, ClientToServerMessageType.SendKeepDistance)
                .AddUInt(targetId)
                .AddVector2(minMaxDistance);
            _networkManager.Client.Send(message);
        }

        public void SendSetOrbit(uint targetId, float orbitRadius)
        {
            var message = Message.Create(MessageSendMode.Reliable, ClientToServerMessageType.SendSetOrbit)
                 .AddUInt(targetId)
                 .AddFloat(orbitRadius);
            _networkManager.Client.Send(message);
        }

        public void SendSetWarpTo(double2 coordinates)
        {
            var message = Message.Create(MessageSendMode.Reliable, ClientToServerMessageType.SendSetWarpTo)
                .AddDouble(coordinates.x)
                .AddDouble(coordinates.y);
            _networkManager.Client.Send(message);
        }
    }
}
