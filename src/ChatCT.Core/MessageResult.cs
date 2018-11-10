using System.Collections.Generic;
using System.Linq;

namespace ChatCT.Core
{
    public class MessageResult
    {
        public IReadOnlyList<MessageItem> Items { get; }

        public MessageResult(IEnumerable<MessageItem> items)
        {
            Items = items.ToList();
        }
    }
}