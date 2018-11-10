namespace ChatCT.Core
{
    public class Emote : MessageItem
    {
        public int Width { get; }

        public int Height { get; }

        public Emote(string content, int width, int height) : base(content, ItemType.Emote)
        {
            Width = width;
            Height = height;
        }
    }
}