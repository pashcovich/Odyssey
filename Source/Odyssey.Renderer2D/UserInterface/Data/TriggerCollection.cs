using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Odyssey.UserInterface.Data
{
    public class TriggerCollection : IEnumerable<TriggerBase>
    {
        private readonly List<TriggerBase> triggers;

        public TriggerCollection()
        {
            triggers = new List<TriggerBase>();
        }

        public void Add(TriggerBase trigger)
        {
            Contract.Requires<ArgumentNullException>(trigger != null, "trigger");
            triggers.Add(trigger);
        }

        public void AddRange(IEnumerable<TriggerBase> triggers)
        {
            Contract.Requires<ArgumentNullException>(triggers != null, "trigger");
            this.triggers.AddRange(triggers);
        }

        public bool HasTriggers
        {
            get { return triggers.Count > 0; }
        }


        #region IEnumerable<TriggerBase>
        IEnumerator<TriggerBase> IEnumerable<TriggerBase>.GetEnumerator()
        {
            return triggers.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return triggers.GetEnumerator();
        } 
        #endregion
    }
}

