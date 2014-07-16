namespace Odyssey.Daedalus.ViewModel.Messages
{
    public enum TechniqueAction
    {
        None,
        Add,
        Remove,
        AllowPreview,
        Preview,
        Unassign
    }

    public class TechniqueMessage
    {
        public TechniqueAction Action { get; private set; }

        public TechniqueMappingViewModel Technique { get; private set; }

        public TechniqueMessage(TechniqueAction action, TechniqueMappingViewModel vmTechnique)
        {
            Action = action;
            Technique = vmTechnique;
        }
    }
}