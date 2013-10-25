using System;
using System.Collections.Generic;
using System.Linq;
using ErrorCode = Odyssey.Properties.Errors;

namespace Odyssey.Utils.Collections
{
    public abstract class Node<T> : INode, INode<T>
    {
        #region Private fields

        private T value;
        private int index;
        private int level;
        private INode<T> parent;
        private INode<T> nextSibling;
        private INode<T> previousSibling;
        private INode<T> firstChild;
        private INode<T> lastChild;

        #endregion Private fields

        #region Properties

        public string Label { get; set; }

        public bool IsLeaf { get; private set; }

        public bool HasChildNodes
        {
            get { return lastChild != null; }
        }

        public bool HasNextSibling
        {
            get { return nextSibling != null; }
        }

        public int ChildrenCount
        {
            get
            {
                return ChildrenNodeIterator.Count();
            }
        }

        public T Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public bool IsEmpty()
        {
            if (!Equals(value, default(T)))
                return true;
            else
                return false;
        }

        #endregion Properties

        protected Node(T value)
        {
            this.value = value;
        }

        #region Protected INode<T> Properties

        protected IEnumerable<INode<T>> ChildrenNodeIterator
        {
            get
            {
                INode<T> tempNode = firstChild;
                while (tempNode != null)
                {
                    yield return tempNode;
                    tempNode = tempNode.NextSibling;
                }
            }
        }

        protected INode<T> ParentNode
        {
            get { return parent; }
        }

        protected INode<T> FirstChildNode
        {
            get { return firstChild; }
        }

        protected INode<T> LastChildNode
        {
            get { return lastChild; }
        }

        protected INode<T> NextSiblingNode
        {
            get { return nextSibling; }
        }

        protected INode<T> PreviousSiblingNode
        {
            get { return previousSibling; }
        }

        #endregion Protected INode<T> Properties

        #region Protected INode<T> Methods

        protected virtual void OnAppendChild(INode<T> newChild)
        {
            if (newChild == null)
                throw new ArgumentNullException("newChild", ErrorCode.ERR_Node_IsNull);

            if (!HasChildNodes)
            {
                if (Node.IsNodeAncestorOf(newChild as INode, this))
                    throw new InvalidOperationException(ErrorCode.ERR_Node_IsAncestor);

                newChild.Parent = this;
                firstChild = lastChild = newChild;
            }
            else
            {
                OnInsertAfter(newChild, lastChild);
            }
        }

        protected virtual void OnRemoveChild(INode<T> oldChild)
        {
            if (oldChild == null)
                throw new ArgumentNullException("oldChild", ErrorCode.ERR_Node_IsNull);

            if (!Node.IsNodeChildOf(oldChild as INode, this))
                throw new ArgumentException(ErrorCode.ERR_Node_NotChild, "oldChild");

            if (firstChild == oldChild)
            {
                if (oldChild == lastChild)
                    firstChild = lastChild = null;
                else
                {
                    firstChild = oldChild.NextSibling;
                    firstChild.PreviousSibling = null;
                    Node.UpdateIndicesForward(firstChild as INode, 0);
                }
            }
            else if (lastChild == oldChild)
            {
                lastChild = oldChild.PreviousSibling;
                lastChild.NextSibling = null;
                Node.UpdateIndicesBackward(lastChild as INode, lastChild.Index);
            }
            else
            {
                INode<T> previousNode = null;
                INode<T> nextNode = null;
                foreach (Node children in ChildrenNodeIterator)
                {
                    if (children == oldChild)
                    {
                        previousNode = oldChild.PreviousSibling;
                        nextNode = oldChild.NextSibling;
                        break;
                    }
                }

                previousNode.NextSibling = nextNode;
                nextNode.PreviousSibling = previousNode;
                Node.UpdateIndicesForward(nextNode as INode, previousNode.Index + 1);
            }
        }

        protected virtual void OnReplaceChild(INode<T> newChild, INode<T> oldChild)
        {
            if (oldChild == null)
                throw new ArgumentNullException("oldChild", ErrorCode.ERR_Node_IsNull);

            if (newChild == null)
                throw new ArgumentNullException("newChild", ErrorCode.ERR_Node_IsNull);

            if (Node.IsNodeAncestorOf(newChild as INode, this))
                throw new InvalidOperationException(ErrorCode.ERR_Node_IsAncestor);

            if (Node.IsNodeChildOf(newChild as INode, this))
                throw new ArgumentException(ErrorCode.ERR_Node_AlreadyChild, "oldChild");

            newChild.Parent = oldChild.Parent;

            if (firstChild == oldChild)
            {
                if (oldChild == lastChild)
                {
                    firstChild = lastChild = newChild;
                }

                newChild.PreviousSibling = null;
                newChild.NextSibling = firstChild.NextSibling;
                firstChild.NextSibling.PreviousSibling = newChild;
                firstChild = newChild;
                firstChild.Index = 0;
            }
            else if (lastChild == oldChild)
            {
                newChild.Index = lastChild.Index;
                newChild.NextSibling = null;
                newChild.PreviousSibling = lastChild.PreviousSibling;
                lastChild.PreviousSibling.NextSibling = newChild;
                newChild.Index = lastChild.Index;
                lastChild = newChild;
            }
            else
            {
                INode<T> previousNode = null;
                INode<T> nextNode = null;
                foreach (Node children in ChildrenNodeIterator)
                {
                    if (children == oldChild)
                    {
                        previousNode = oldChild.PreviousSibling;
                        nextNode = oldChild.NextSibling;
                        break;
                    }
                }

                previousNode.NextSibling = newChild;
                nextNode.PreviousSibling = newChild;
                newChild.PreviousSibling = previousNode;
                newChild.NextSibling = nextNode;
                newChild.Index = previousNode.Index + 1;
            }
        }

        protected virtual void OnInsertBefore(INode<T> newChild, INode<T> refNode)
        {
            if (refNode == null)
                throw new ArgumentNullException("refNode", ErrorCode.ERR_Node_IsNull);

            if (newChild == null)
                throw new ArgumentNullException("newChild", ErrorCode.ERR_Node_IsNull);

            if (Node.IsNodeAncestorOf(newChild as INode, this))
                throw new InvalidOperationException(ErrorCode.ERR_Node_IsAncestor);

            if (Node.IsNodeChildOf(newChild as INode, this))
                throw new ArgumentException(ErrorCode.ERR_Node_AlreadyChild, "refNode");

            if (refNode == firstChild)
            {
                newChild.PreviousSibling = null;
                firstChild = refNode;
            }
            else
            {
                INode<T> previousNode = refNode.PreviousSibling;
                newChild.PreviousSibling = previousNode;
                previousNode.NextSibling = newChild;
            }

            newChild.NextSibling = refNode;
            refNode.PreviousSibling = newChild;
            newChild.Parent = this;

            if (refNode == firstChild)
                firstChild = newChild;

            Node.UpdateIndicesForward(newChild as INode, refNode.Index);
        }

        protected virtual void OnInsertAfter(INode<T> newChild, INode<T> refNode)
        {
            if (refNode == null)
                throw new ArgumentNullException("refNode", ErrorCode.ERR_Node_IsNull);

            if (newChild == null)
                throw new ArgumentNullException("newChild", ErrorCode.ERR_Node_IsNull);

            if (Node.IsNodeAncestorOf(newChild as INode, this))
                throw new InvalidOperationException(ErrorCode.ERR_Node_IsAncestor);

            if (Node.IsNodeChildOf(newChild as INode, this))
                throw new ArgumentException(ErrorCode.ERR_Node_NotChild, "refNode");

            if (refNode == lastChild)
            {
                newChild.NextSibling = null;
                lastChild = newChild;
            }
            else
            {
                INode<T> nextNode = refNode.NextSibling;
                nextNode.PreviousSibling = newChild;
                newChild.NextSibling = nextNode;
            }

            refNode.NextSibling = newChild;
            newChild.PreviousSibling = refNode;
            newChild.Parent = this;

            Node.UpdateIndicesForward(newChild as INode, refNode.Index + 1);
        }

        protected virtual void OnPrependChild(INode<T> newChild)
        {
            if (newChild == null)
                throw new ArgumentNullException("newChild", ErrorCode.ERR_Node_IsNull);

            if (!HasChildNodes)
            {
                newChild.Parent = this;
                firstChild = lastChild = newChild;
            }
            else
                OnInsertBefore(newChild, firstChild);
        }

        #endregion Protected INode<T> Methods

        #region Methods

        /// <summary>
        /// Removes all children from this node.
        /// </summary>
        public void RemoveAll()
        {
            firstChild = lastChild = null;
        }

        public bool Contains(INode child)
        {
            foreach (INode node in ChildrenNodeIterator)
                if (node == child)
                    return true;

            return false;
        }

        #endregion Methods

        #region Visit algorithms

        public static IEnumerable<INode<T>> PreOrderVisit(INode<T> headNode)
        {
            yield return headNode;

            if (headNode.HasChildNodes)
            {
                foreach (INode<T> childNode in PreOrderVisit(headNode.FirstChild))
                    yield return childNode;
            }
            else
            {
                INode<T> node = headNode.NextSibling;
                while (node != null)
                {
                    yield return node;
                    node = node.NextSibling;
                }
            }
        }

        public static IEnumerable<INode<T>> PostOrderVisit(INode<T> headNode)
        {
            if (headNode.HasChildNodes)
            {
                foreach (INode<T> childNode in PreOrderVisit(headNode.FirstChild))
                    yield return childNode;
            }
            else
            {
                INode<T> node = headNode.NextSibling;
                while (node != null)
                {
                    yield return node;
                    node = node.NextSibling;
                }
            }
            yield return headNode;
        }

        #endregion Visit algorithms

        #region INode<T> Members

        int INode<T>.Index
        {
            get
            {
                return index;
            }
            set
            {
                index = value;
            }
        }

        INode<T> INode<T>.Parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
                (this as INode<T>).Level = parent.Level + 1;
            }
        }

        int INode<T>.Level
        {
            get { return level; }
            set
            {
                level = value;
                foreach (INode<T> children in ChildrenNodeIterator)
                    children.Level = (this as INode<T>).Level + 1;
            }
        }

        INode<T> INode<T>.PreviousSibling
        {
            get
            {
                return previousSibling;
            }
            set
            {
                previousSibling = value;
            }
        }

        INode<T> INode<T>.NextSibling
        {
            get
            {
                return nextSibling;
            }
            set { nextSibling = value; }
        }

        INode<T> INode<T>.FirstChild
        {
            get { return firstChild; }
        }

        INode<T> INode<T>.LastChild
        {
            get { return lastChild; }
        }

        IEnumerable<INode<T>> INode<T>.ChildrenIterator
        {
            get
            {
                return ChildrenNodeIterator;
            }
        }

        void INode<T>.AppendChild(INode<T> newNode)
        {
            OnAppendChild(newNode);
        }

        void INode<T>.InsertBefore(INode<T> newChild, INode<T> refNode)
        {
            OnInsertBefore(newChild, refNode);
        }

        void INode<T>.InsertAfter(INode<T> newChild, INode<T> refNode)
        {
            OnInsertAfter(newChild, refNode);
        }

        void INode<T>.RemoveChild(INode<T> oldChild)
        {
            OnRemoveChild(oldChild);
        }

        void INode<T>.ReplaceChild(INode<T> newChild, INode<T> oldChild)
        {
            OnReplaceChild(newChild, oldChild);
        }

        void INode<T>.PrependChild(INode<T> newChild)
        {
            OnPrependChild(newChild);
        }

        #endregion INode<T> Members

        #region INode Members

        INode INode.Parent
        {
            get
            {
                return parent as INode;
            }
            set
            {
                parent = value as INode<T>;
                (this as INode<T>).Level = parent.Level + 1;
            }
        }

        INode INode.PreviousSibling
        {
            get
            {
                return previousSibling as INode;
            }
            set
            {
                previousSibling = value as INode<T>;
            }
        }

        INode INode.NextSibling
        {
            get
            {
                return nextSibling as INode;
            }
            set
            {
                previousSibling = value as INode<T>;
            }
        }

        INode INode.FirstChild
        {
            get { return firstChild as INode; }
        }

        INode INode.LastChild
        {
            get { return lastChild as INode; }
        }

        IEnumerable<INode> INode.ChildrenIterator
        {
            get { return ChildrenNodeIterator as IEnumerable<INode>; }
        }

        int INode.Index
        {
            get
            {
                return index;
            }
            set
            {
                index = value;
            }
        }

        int INode.Level
        {
            get
            {
                return level;
            }
            set
            {
                level = value;
                foreach (INode<T> children in ChildrenNodeIterator)
                    children.Level = (this as INode<T>).Level + 1;
            }
        }

        void INode.AppendChild(INode newChild)
        {
            OnAppendChild(newChild as INode<T>);
        }

        void INode.InsertBefore(INode newChild, INode refNode)
        {
            OnInsertAfter(newChild as INode<T>, refNode as INode<T>);
        }

        void INode.InsertAfter(INode newChild, INode refNode)
        {
            OnInsertAfter(newChild as INode<T>, refNode as INode<T>);
        }

        void INode.RemoveChild(INode oldChild)
        {
            OnRemoveChild(oldChild as INode<T>);
        }

        void INode.ReplaceChild(INode newChild, INode oldChild)
        {
            OnReplaceChild(newChild as INode<T>, oldChild as INode<T>);
        }

        void INode.PrependChild(INode newChild)
        {
            OnPrependChild(newChild as INode<T>);
        }

        #endregion INode Members
    }
}