using System;
using System.Collections.Generic;

namespace ChatCT.Core
{
    public class EmoteManager
    {
        public MessageResult ParseEmotes(string message)
        {
            var items = new List<MessageItem>();

            int index;
            do
            {
                index = message.IndexOf("Kappa", StringComparison.Ordinal);

                if (index != -1)
                {
                    var part = message.Substring(0, index);
                    items.Add(new MessageItem(part, ItemType.Text));
                    items.Add(new MessageItem(@"kappa.png", ItemType.Emote));

                    message = message.Substring(index + "Kappa".Length);
                }
            } while (index != -1);

            items.Add(new MessageItem(message, ItemType.Text));

            return new MessageResult(items);
        }
    }
}
