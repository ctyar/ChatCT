namespace ChatCT.Core
{
    public class Text : MessageItem
    {
        public Text(string content) : base(content, ItemType.Text)
        {
        }
    }
}