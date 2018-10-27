using System;
using System.Collections.Concurrent;
using System.Threading;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Events;

namespace ChatCT.Core
{
    public class MessageManager
    {
        private readonly BlockingCollection<string> _blockingCollection;
        private readonly CancellationToken _cancellationToken;
        private TwitchClient _twitchClient;

        public MessageManager(BlockingCollection<string> blockingCollection, CancellationToken cancellationToken)
        {
            _blockingCollection = blockingCollection;
            _cancellationToken = cancellationToken;
        }

        public void Connect(string username, string accessToken, string channel)
        {
            var credentials = new ConnectionCredentials(username, accessToken);

            _twitchClient = new TwitchClient();
            _twitchClient.Initialize(credentials, channel);

            _twitchClient.OnConnected += OnConnected;
            _twitchClient.OnMessageReceived += OnMessageReceived;
            _twitchClient.OnDisconnected += OnDisconnected;

            _twitchClient.Connect();
        }

        public void Disconnect()
        {
            _twitchClient.Disconnect();
        }

        private void OnConnected(object sender, OnConnectedArgs e)
        {
            AddMessage($"Connected to {e.AutoJoinChannel}");
        }

        private void OnDisconnected(object sender, OnDisconnectedEventArgs e)
        {
            AddMessage("Disconnected from channel.");
        }

        private void OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            AddMessage(e.ChatMessage.Message);
        }

        private void AddMessage(string message)
        {
            try
            {
                _blockingCollection.TryAdd(message, 2, _cancellationToken);
            }
            catch (OperationCanceledException)
            {
                _blockingCollection.CompleteAdding();
            }
        }
    }
}