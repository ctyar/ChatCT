namespace ChatCT.Core
{
    public class MessageItem
    {
        public string Content { get; }

        public ItemType Type { get; }

        public MessageItem(string content, ItemType type)
        {
            Content = content;
            Type = type;
        }
    }
}