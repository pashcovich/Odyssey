using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.UserInterface.Data
{
    public class TriggerActionCollection : IEnumerable<TriggerAction>
    {
        private readonly List<TriggerAction> triggerActions;

        public TriggerActionCollection()
        {
            triggerActions = new List<TriggerAction>();
        }

        public void Add(TriggerAction triggerAction)
        {
            Contract.Requires<ArgumentNullException>(triggerAction != null, "triggerAction");
            triggerActions.Add(triggerAction);
        }

        public void AddRange(IEnumerable<TriggerAction> triggerAction)
        {
            Contract.Requires<ArgumentNullException>(triggerAction != null, "triggerAction");
            this.triggerActions.AddRange(triggerAction);
        }

        public bool HasTriggers
        {
            get { return triggerActions.Count > 0; }
        }


        #region IEnumerable<TriggerAction>
        IEnumerator<TriggerAction> IEnumerable<TriggerAction>.GetEnumerator()
        {
            return triggerActions.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return triggerActions.GetEnumerator();
        } 
        #endregion
    }
}
