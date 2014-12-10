#region Using Directives

using Odyssey.Content;

#endregion

namespace Odyssey.Animations
{
    public interface IAnimatable : IResource
    {
        void Play();
        void Play(string animationName);
    }
}