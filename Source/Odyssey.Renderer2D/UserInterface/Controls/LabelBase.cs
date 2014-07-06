using System;

namespace Odyssey.UserInterface.Controls
{
    public abstract class LabelBase : Control
    {
        private const string ControlTag = "Default";
        private string text;

        protected LabelBase()
            : this(ControlTag)
        {
        }

        protected LabelBase(string textDefinitionClass)
            : base("Empty", textDefinitionClass)
        {
            CanRaiseEvents = false;
        }

        public string Text
        {
            get { return text; }
            set
            {
                if (text != value)
                    text = value;
            }
        }
    }
}