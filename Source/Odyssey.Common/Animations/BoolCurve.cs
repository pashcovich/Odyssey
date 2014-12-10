namespace Odyssey.Animations
{
    public class BoolCurve : AnimationCurve<BoolKeyFrame>
    {
        public BoolCurve()
        {
            Function = Discrete;
        }
    }
}