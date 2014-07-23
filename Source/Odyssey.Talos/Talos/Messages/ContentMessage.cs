using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Talos.Messages
{
    public class ContentMessage<TContent> : Message
    {
        public TContent Content { get; private set; }

        public ContentMessage(TContent content, bool isSynchronous = false)
            : base(isSynchronous)
        {
            Content = content;
        }
    }
}
