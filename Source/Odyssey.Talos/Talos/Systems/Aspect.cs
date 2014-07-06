using Odyssey.Talos.Components;
using System;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Odyssey.Talos.Systems
{
    public class Aspect
    {
        protected long ContainsTypesMap { get; set; }
        protected long ExcludeTypesMap { get; set; }
        protected long OneTypesMap { get; set; }

        protected Aspect()
        {
            this.OneTypesMap = 0;
            this.ExcludeTypesMap = 0;
            this.ContainsTypesMap = 0;
        }

        public static Aspect All(params Type[] types)
        {
            return new Aspect().GetAll(types);
        }

        public static Aspect Exclude(params Type[] types)
        {
            return new Aspect().GetExclude(types);
        }

        public static Aspect One(params Type[] types)
        {
            return new Aspect().GetOne(types);
        }

        public Aspect GetAll(params Type[] types)
        {
            Contract.Requires<ArgumentNullException>(types!=null);
            //Contract.Requires<InvalidOperationException>(ReflectionHelper.AreTypesDerived(types, typeof(IComponent)), "Supplied type is not a valid component.");

            foreach (ComponentType componentType in types.Select(ComponentTypeManager.GetType))
            {
                this.ContainsTypesMap |= componentType.KeyPart;
            }

            return this;
        }

        public Aspect GetExclude(params Type[] types)
        {
            Contract.Requires<ArgumentNullException>(types != null);

            foreach (ComponentType componentType in types.Select(ComponentTypeManager.GetType))
            {
                this.ExcludeTypesMap |= componentType.KeyPart;
            }

            return this;
        }

        public Aspect GetOne(params Type[] types)
        {
            Contract.Requires<ArgumentNullException>(types != null);

            foreach (ComponentType componentType in types.Select(ComponentTypeManager.GetType))
            {
                this.OneTypesMap |= componentType.KeyPart;
            }

            return this;
        }

        public virtual bool Interests(long entityKey)
        {
            if (!(this.ContainsTypesMap > 0 || this.ExcludeTypesMap > 0 || this.OneTypesMap > 0))
            {
                return false;
            }

            ////Little help
            ////10010 & 10000 = 10000
            ////10010 | 10000 = 10010
            ////10010 | 01000 = 11010

            ////1001 & 0000 = 0000 OK
            ////1001 & 0100 = 0000 NOK           
            ////0011 & 1001 = 0001 Ok

            return ((this.OneTypesMap & entityKey) != 0 || this.OneTypesMap == 0) &&
                   ((this.ContainsTypesMap & entityKey) == this.ContainsTypesMap || this.ContainsTypesMap == 0) &&
                   ((this.ExcludeTypesMap & entityKey) != this.ExcludeTypesMap || this.ExcludeTypesMap == 0);
        }
    }
}
