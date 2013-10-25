#region Using Directives
using System.Collections.Generic;


#endregion

namespace Odyssey.Utils.Collections
{
    public interface INode
    {
        INode Parent { get; set; }
        INode PreviousSibling { get; set; }
        INode NextSibling { get; set; }
        INode FirstChild { get; }
        INode LastChild { get; }
        IEnumerable<INode> ChildrenIterator { get; }

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

    public interface INode<T>
    {
        T Value { get; set; }
        INode<T> Parent { get; set; }
        INode<T> PreviousSibling { get; set; }
        INode<T> NextSibling { get; set; }
        INode<T> FirstChild { get; }
        INode<T> LastChild { get; }
        IEnumerable<INode<T>> ChildrenIterator { get; }

        bool HasChildNodes { get; }
        bool HasNextSibling { get; }
        int Index { get; set; }
        int Level { get; set; }

        void AppendChild(INode<T> newChild);
        void InsertBefore(INode<T> newChild, INode<T> refNode);
        void InsertAfter(INode<T> newChild, INode<T> refNode);
        void RemoveChild(INode<T> oldChild);
        void RemoveAll();
        void ReplaceChild(INode<T> newChild, INode<T> oldChild);
        void PrependChild(INode<T> newChild);

       
    }
}