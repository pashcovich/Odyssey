namespace Odyssey.UserInterface.Behaviors
{
    public abstract class Behavior<TElement> : Behavior
        where TElement : UIElement
    {
        protected Behavior() : base(typeof (TElement))
        {
        }

        protected new TElement AssociatedElement
        {
            get { return (TElement) base.AssociatedElement; }
        }
    }
}