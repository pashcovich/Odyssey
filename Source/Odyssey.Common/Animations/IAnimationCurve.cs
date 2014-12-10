namespace Odyssey.Animations
{
    public interface IAnimationCurve
    {
        string TargetProperty { get; }
        string TargetName { get; }
        string Name { get; set; }
        string Key { get; }
        int KeyFrameCount { get; }
        float Duration { get; }
        IKeyFrame this[int index] { get; }
        object Evaluate(float elapsedTime);
    }
}