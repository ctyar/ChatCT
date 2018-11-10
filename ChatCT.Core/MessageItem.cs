namespace ChatCT.Core
{
    public abstract class MessageItem
    {
        public string Content { get; }

        public ItemType Type { get; }

        protected MessageItem(string content, ItemType type)
        {
            Content = content;
            Type = type;
        }
    }
}