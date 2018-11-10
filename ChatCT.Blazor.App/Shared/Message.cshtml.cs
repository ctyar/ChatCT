using ChatCT.Core;
using Microsoft.AspNetCore.Blazor.Components;

namespace ChatCT.Blazor.App.Shared
{
    public class MessageBase : BlazorComponent
    {
        [Parameter] protected MessageResult Value { get; set; }

        protected string GetStyle(Emote emote)
        {
            if (emote.Width != default || emote.Height != default)
            {
                return $"width: {emote.Width}px; height: {emote.Height}px;";
            }

            return default;
        }
    }
}