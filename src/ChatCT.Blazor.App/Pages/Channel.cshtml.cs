using System;
using System.Collections.Generic;
using ChatCT.Core;
using Microsoft.AspNetCore.Blazor.Components;

namespace ChatCT.Blazor.App.Pages
{
    public class ChannelBase : BlazorComponent, IDisposable
    {
        private const int MaxMessages = 20;
        private MessageManager _messageManager;
        private EmoteManager _emoteManager;

        [Parameter] protected string Channel { get; set; }

        [Parameter] protected string UserName { get; set; }

        [Parameter] protected string Token { get; set; }

        [Parameter] protected Queue<MessageResult> Messages { get; set; }

        protected override void OnInit()
        {
            Messages = new Queue<MessageResult>(MaxMessages);
            _messageManager = new MessageManager(AddMessage);
            _emoteManager = new EmoteManager();

            _messageManager.Connect(UserName, Token, Channel);
        }

        private void AddMessage(string message)
        {
            var messageResult = _emoteManager.ParseEmotes(message);

            while (Messages.Count >= MaxMessages)
            {
                Messages.Dequeue();
            }

            Messages.Enqueue(messageResult);

            StateHasChanged();
        }

        public void Dispose()
        {
            _messageManager?.Disconnect();
        }
    }
}
