using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using ErrorCode = Odyssey.Properties.Errors;

namespace Odyssey.Utilities.Collections
{
    public abstract class Node<T> : INode<T>
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
                return Children.Count();
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

        public IEnumerable<INode<T>> Children
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
            Contract.Requires<ArgumentNullException>(newChild != null);
            Contract.Requires<ArgumentException>(IsNodeAncestorOf(newChild, this));

            if (!HasChildNodes)
            {

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
            Contract.Requires<ArgumentNullException>(oldChild != null);
            Contract.Requires<ArgumentException>(IsNodeChildOf(oldChild, this));

            if (firstChild == oldChild)
            {
                if (oldChild == lastChild)
                    firstChild = lastChild = null;
                else
                {
                    firstChild = oldChild.NextSibling;
                    firstChild.PreviousSibling = null;
                    UpdateIndicesForward(firstChild, 0);
                }
            }
            else if (lastChild == oldChild)
            {
                lastChild = oldChild.PreviousSibling;
                lastChild.NextSibling = null;
                UpdateIndicesBackward(lastChild, lastChild.Index);
            }
            else
            {
                INode<T> previousNode = null;
                INode<T> nextNode = null;
                foreach (var children in Children)
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
                UpdateIndicesForward(nextNode, previousNode.Index + 1);
            }
        }

        protected virtual void OnReplaceChild(INode<T> newChild, INode<T> oldChild)
        {
            Contract.Requires<ArgumentNullException>(oldChild != null);
            Contract.Requires<ArgumentNullException>(newChild != null);
            Contract.Requires<ArgumentException>(IsNodeAncestorOf(newChild, this));
            Contract.Requires<ArgumentException>(IsNodeChildOf(newChild, this));

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
                foreach (var children in Children)
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
            Contract.Requires<ArgumentNullException>(refNode!=null);
            Contract.Requires<ArgumentNullException>(newChild != null);
            Contract.Requires<ArgumentException>(IsNodeAncestorOf(newChild, this));
            Contract.Requires<ArgumentException>(IsNodeChildOf(newChild, this));

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

            UpdateIndicesForward(newChild, refNode.Index);
        }

        protected virtual void OnInsertAfter(INode<T> newChild, INode<T> refNode)
        {
            Contract.Requires<ArgumentNullException>(refNode != null);
            Contract.Requires<ArgumentNullException>(newChild != null);
            Contract.Requires<ArgumentException>(IsNodeAncestorOf(newChild, this));
            Contract.Requires<ArgumentException>(IsNodeChildOf(newChild, this));

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

            UpdateIndicesForward(newChild , refNode.Index + 1);
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

        public bool Contains(INode<T> child)
        {
            foreach (INode<T> node in Children)
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

        public int Index
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

        public INode<T> Parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
                Level = parent.Level + 1;
            }
        }

        public int Level
        {
            get { return level; }
            set
            {
                level = value;
                foreach (INode<T> children in Children)
                    children.Level = Level + 1;
            }
        }

        public INode<T> PreviousSibling
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

        public INode<T> NextSibling
        {
            get
            {
                return nextSibling;
            }
            set { nextSibling = value; }
        }

        public INode<T> FirstChild
        {
            get { return firstChild; }
        }

        public INode<T> LastChild
        {
            get { return lastChild; }
        }

        public void AppendChild(INode<T> newNode)
        {
            OnAppendChild(newNode);
        }

        public void InsertBefore(INode<T> newChild, INode<T> refNode)
        {
            OnInsertBefore(newChild, refNode);
        }

        public void InsertAfter(INode<T> newChild, INode<T> refNode)
        {
            OnInsertAfter(newChild, refNode);
        }

        public void RemoveChild(INode<T> oldChild)
        {
            OnRemoveChild(oldChild);
        }

        public void ReplaceChild(INode<T> newChild, INode<T> oldChild)
        {
            OnReplaceChild(newChild, oldChild);
        }

        public void PrependChild(INode<T> newChild)
        {
            OnPrependChild(newChild);
        }

        #endregion INode<T> Members

    }
}