using System;
using System.Collections.Generic;

namespace ChatCT.Core
{
    public class EmoteManager
    {
        const string EmoteName = "Kappa";
        const string EmotePath = "https://static-cdn.jtvnw.net/emoticons/v1/25/3.0";

        public MessageResult ParseEmotes(string message)
        {
            var items = new List<MessageItem>();

            int index;
            do
            {
                index = message.IndexOf(EmoteName, StringComparison.Ordinal);

                if (index != -1)
                {
                    var part = message.Substring(0, index);
                    items.Add(new Text(part));
                    items.Add(new Emote(EmotePath, 19, 21));

                    message = message.Substring(index + EmoteName.Length);
                }
            } while (index != -1);

            items.Add(new Text(message));

            return new MessageResult(items);
        }
    }
}
