namespace Odyssey.Talos.Messages
{
    public class ResizeOutputMessage : Message
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public ResizeOutputMessage(int width, int height) : base(true)
        {
            Height = height;
            Width = width;
        }
    }
}
