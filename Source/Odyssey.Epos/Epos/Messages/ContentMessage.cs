namespace Odyssey.Epos.Messages
{
    public class ContentMessage<TContent> : EntityMessage
    {
        public string AssetName { get; private set; }
        public TContent Content { get; private set; }

        public ContentMessage(Entity source, string assetName, TContent content, bool isSynchronous = false)
            : base(source, isSynchronous)
        {
            AssetName = assetName;
            Content = content;
        }
    }
}
