#region Using Directives
using System.Collections.Generic;


#endregion

namespace Odyssey.Utilities.Collections
{
    public interface INode
    {
        INode Parent { get; set; }
        INode PreviousSibling { get; set; }
        INode NextSibling { get; set; }
        INode FirstChild { get; }
        INode LastChild { get; }
        IEnumerable<INode> Children { get; }

        bool HasChildNodes { get; }
        bool HasNextSibling { get; }
        int Index { get; set; }
        int Level { get; set; }
        int ChildrenCount { get; }
        bool IsLeaf { get; }
        string Label { get; set; }
        
        void AppendChild(INode newChild);
        void InsertBefore(INode newChild, INode refNode);
        void InsertAfter(INode newChild, INode refNode);
        void RemoveChild(INode oldChild);
        void RemoveAll();
        void ReplaceChild(INode newChild, INode oldChild);
        void PrependChild(INode newChild);
        bool Contains(INode child);

    }

    public interface INode<T> : INode
    {
        T Value { get; set; }
      
    }
}