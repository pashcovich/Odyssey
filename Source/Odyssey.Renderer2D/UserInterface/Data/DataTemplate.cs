using System;
using System.Collections.Generic;
using System.Linq;

namespace Odyssey.UserInterface.Data
{
    public class DataTemplate
    {
        private Dictionary<string, Binding> bindings;

        public DataTemplate()
        {
            bindings = new Dictionary<string, Binding>();
        }

        public IEnumerable<KeyValuePair<string, Binding>> Bindings
        {
            get
            {
                return bindings;
            }
            set
            {
                if (!ReferenceEquals(bindings, value))
                {
                    var dictionary = value as Dictionary<string, Binding>;
                    bindings = dictionary ?? value.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                }
            }
        }

        public Type DataType { get; set; }

        public string Key { get; set; }

        public UIElement VisualTree { get; set; }
    }
}