#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using ErrorCode = Odyssey.Properties.Errors;

#endregion Using directives

namespace Odyssey.Utils.Collections
{
    public class NodeEventArgs : EventArgs
    {
        private readonly int index;
        private readonly int level;
        private readonly INode node;

        public NodeEventArgs(INode node)
        {
            Contract.Requires(node != null);
            index = node.Index;
            level = node.Level;
            this.node = node;
        }

        /// <summary>
        ///   Gets or sets the label for this node
        /// </summary>
        /// <value>
        ///   A <see cref = "string" /> that contains the label of the node.
        /// </value>

        public int Index
        {
            get { return index; }
        }

        public int Level
        {
            get { return level; }
        }

        public INode Node
        {
            get { return node; }
        }
    }

    [DebuggerDisplay("{GetType().Name} = {ToString()}")]
    public abstract class Node : INode, IEnumerable<INode>
    {
        #region Delegates

        public delegate void NodeEventHandler(object sender, NodeEventArgs e);

        #endregion Delegates

        #region Private fields

        private INode firstChild;
        private int index;
        private INode lastChild;
        private int level;
        private INode nextSibling;
        private INode parent;
        private INode previousSibling;

        #endregion Private fields

        #region Events

        private static readonly object EventParentChanged;
        private static readonly object EventChildAdded;
        private static readonly object EventChildRemoved;

        public event NodeEventHandler ParentChanged;

        public event NodeEventHandler ChildAdded;

        public event NodeEventHandler ChildRemoved;

        protected virtual void OnParentChanged(object sender, NodeEventArgs e)
        {
            if (ParentChanged != null)
                ParentChanged(this, e);
        }

        protected virtual void OnChildAdded(object sender, NodeEventArgs e)
        {
            if (ChildAdded != null)
                ChildAdded(this, e);
        }

        protected virtual void OnChildRemoved(object sender, NodeEventArgs e)
        {
            if (ChildRemoved != null)
                ChildRemoved(this, e);
        }

        #endregion Events

        #region Constructor

        protected Node()
        {
        }

        #endregion Constructor

        #region Properties

        public string Label { get; set; }

        public bool IsLeaf { get; set; }

        public IEnumerable<INode> ChildrenIterator
        {
            get
            {
                INode tempNode = firstChild;
                while (tempNode != null)
                {
                    yield return tempNode;
                    tempNode = tempNode.NextSibling;
                }
            }
        }

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
            get { return ChildrenIterator.Count(); }
        }

        #endregion Properties

        #region Protected INode Properties

        protected INode ParentNode
        {
            get { return parent; }
        }

        protected INode FirstChildNode
        {
            get { return firstChild; }
        }

        protected INode LastChildNode
        {
            get { return lastChild; }
        }

        protected INode NextSiblingNode
        {
            get { return nextSibling; }
        }

        protected INode PreviousSiblingNode
        {
            get { return previousSibling; }
        }

        #endregion Protected INode Properties

        #region Protected INode Methods

        protected virtual void OnAppendChild(INode newChild)
        {
            Contract.Requires<InvalidOperationException>(IsLeaf != true, ErrorCode.ERR_Node_IsLeaf);
            Contract.Requires<ArgumentNullException>(newChild != null, ErrorCode.ERR_Node_IsNull);
            Contract.Requires<InvalidOperationException>(!IsNodeAncestorOf(newChild, this), ErrorCode.ERR_Node_IsAncestor);
            Contract.Requires<InvalidOperationException>(!IsNodeChildOf(newChild, this), ErrorCode.ERR_Node_IsAncestor);

            if (!HasChildNodes)
            {
                firstChild = lastChild = newChild;
                newChild.Parent = this;
                OnChildAdded(this, new NodeEventArgs(newChild));
            }
            else
            {
                OnInsertAfter(newChild, lastChild);
            }
        }

        protected virtual void OnRemoveChild(INode oldChild)
        {
            Contract.Requires<ArgumentNullException>(oldChild != null, ErrorCode.ERR_Node_IsNull);
            Contract.Requires<ArgumentException>(!IsNodeChildOf(oldChild, this), ErrorCode.ERR_Node_NotChild);

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
                INode previousNode = oldChild.PreviousSibling;
                INode nextNode = oldChild.NextSibling;

                previousNode.NextSibling = nextNode;
                nextNode.PreviousSibling = previousNode;
                UpdateIndicesForward(nextNode, previousNode.Index + 1);
            }

            OnChildRemoved(this, new NodeEventArgs(oldChild));
        }

        protected virtual void OnReplaceChild(INode newChild, INode oldChild)
        {
            Contract.Requires(HasChildNodes);
            Contract.Requires<ArgumentNullException>(newChild != null, ErrorCode.ERR_Node_IsNull);
            Contract.Requires<ArgumentNullException>(oldChild != null, ErrorCode.ERR_Node_IsNull);
            Contract.Requires<InvalidOperationException>(!IsNodeAncestorOf(newChild, this), ErrorCode.ERR_Node_IsAncestor);
            Contract.Requires<InvalidOperationException>(!IsNodeChildOf(newChild, this), ErrorCode.ERR_Node_IsAncestor);

            if (firstChild == oldChild)
            {
                if (oldChild == lastChild)
                {
                    firstChild = lastChild = newChild;
                }
                else
                {
                    newChild.PreviousSibling = null;
                    newChild.NextSibling = firstChild.NextSibling;
                    firstChild.NextSibling.PreviousSibling = newChild;
                    firstChild = newChild;
                    firstChild.Index = 0;
                }
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
                INode previousNode = oldChild.PreviousSibling;
                INode nextNode = oldChild.NextSibling;

                previousNode.NextSibling = newChild;
                nextNode.PreviousSibling = newChild;
                newChild.PreviousSibling = previousNode;
                newChild.NextSibling = nextNode;
                newChild.Index = previousNode.Index + 1;
            }

            newChild.Parent = oldChild.Parent;

            OnChildRemoved(this, new NodeEventArgs(oldChild));
            OnChildAdded(this, new NodeEventArgs(newChild));
        }

        protected virtual void OnInsertBefore(INode newChild, INode refNode)
        {
            Contract.Requires<InvalidOperationException>(IsLeaf != true, ErrorCode.ERR_Node_IsLeaf);
            Contract.Requires<ArgumentNullException>(newChild != null, ErrorCode.ERR_Node_IsNull);
            Contract.Requires<ArgumentNullException>(refNode != null, ErrorCode.ERR_Node_IsNull);
            Contract.Requires<InvalidOperationException>(!IsNodeAncestorOf(newChild, this), ErrorCode.ERR_Node_IsAncestor);
            Contract.Requires<InvalidOperationException>(!IsNodeChildOf(newChild, this), ErrorCode.ERR_Node_IsAncestor);

            if (refNode == firstChild)
            {
                newChild.PreviousSibling = null;
                firstChild = refNode;
            }
            else
            {
                INode previousNode = refNode.PreviousSibling;
                newChild.PreviousSibling = previousNode;
                previousNode.NextSibling = newChild;
            }

            newChild.NextSibling = refNode;
            refNode.PreviousSibling = newChild;

            if (refNode == firstChild)
                firstChild = newChild;

            UpdateIndicesForward(newChild, refNode.Index);

            newChild.Parent = this;

            OnChildAdded(this, new NodeEventArgs(newChild));
        }

        protected virtual void OnInsertAfter(INode newChild, INode refNode)
        {
            Contract.Requires<InvalidOperationException>(IsLeaf != true, ErrorCode.ERR_Node_IsLeaf);
            Contract.Requires<ArgumentNullException>(newChild != null, ErrorCode.ERR_Node_IsNull);
            Contract.Requires<ArgumentNullException>(refNode != null, ErrorCode.ERR_Node_IsNull);
            Contract.Requires<InvalidOperationException>(!IsNodeAncestorOf(newChild, this), ErrorCode.ERR_Node_IsAncestor);
            Contract.Requires<InvalidOperationException>(!IsNodeChildOf(newChild, this), ErrorCode.ERR_Node_IsAncestor);

            if (refNode == lastChild)
            {
                newChild.NextSibling = null;
                lastChild = newChild;
            }
            else
            {
                INode nextNode = refNode.NextSibling;
                nextNode.PreviousSibling = newChild;
                newChild.NextSibling = nextNode;
            }

            refNode.NextSibling = newChild;
            newChild.PreviousSibling = refNode;

            UpdateIndicesForward(newChild, refNode.Index + 1);

            newChild.Parent = this;
            OnChildAdded(this, new NodeEventArgs(newChild));
        }

        protected virtual void OnPrependChild(INode newChild)
        {
            Contract.Requires<InvalidOperationException>(IsLeaf != true, ErrorCode.ERR_Node_IsLeaf);
            Contract.Requires<ArgumentNullException>(newChild != null, ErrorCode.ERR_Node_IsNull);

            if (!HasChildNodes)
            {
                firstChild = lastChild = newChild;
                newChild.Parent = this;
                OnChildAdded(this, new NodeEventArgs(newChild));
            }
            else
                OnInsertBefore(newChild, firstChild);
        }

        #endregion Protected INode Methods

        #region Methods

        /// <summary>
        ///   Removes all children from this node.
        /// </summary>
        public void RemoveAll()
        {
            firstChild = lastChild = null;
        }

        public bool Contains(INode child)
        {
            return ChildrenIterator.Any(node => node == child);
        }

        public override string ToString()
        {
            return string.Format("Level {0} - Index {1}", level, index);
        }

        #endregion Methods

        #region Static Methods

        /// <summary>
        ///   Checks whether the node is a child of the specified parent node.
        /// </summary>
        /// <param name = "childNode">The child node.</param>
        /// <param name = "parentNode">The parent node.</param>
        /// <returns><c>True</c> if childNode is a child of parentNode. <c>False</c> otherwise.</returns>
        public static bool IsNodeChildOf(INode childNode, INode parentNode)
        {
            Contract.Requires<NullReferenceException>(parentNode != null);

            return parentNode.ChildrenIterator.Any(node => node == childNode);
        }

        /// <summary>
        ///   Checks whether the node is a parent of the specified node or whether the two nodes are
        ///   the same.
        /// </summary>
        /// <param name = "node">The child node.</param>
        /// <param name = "refNode">The parent node.</param>
        /// <returns><c>True</c> if node is a parent of refNode. <c>False</c> otherwise.</returns>
        public static bool IsNodeAncestorOf(INode node, INode refNode)
        {
            Contract.Requires<NullReferenceException>(node != null);
            Contract.Requires<NullReferenceException>(refNode != null);

            if (node == refNode)
                return true;

            INode currentNode = refNode;
            while (currentNode.Parent != null)
            {
                if (currentNode.Parent == node)
                    return true;
                currentNode = currentNode.Parent;
            }
            return false;
        }

        internal static void UpdateIndicesForward(INode headNode, int startIndex)
        {
            Contract.Requires<NullReferenceException>(headNode != null);
            INode node = headNode;
            int i = startIndex;
            while (node != null)
            {
                node.Index = i;
                node = node.NextSibling;
                i++;
            }
        }

        internal static void UpdateIndicesBackward(INode headNode, int startIndex)
        {
            Contract.Requires<NullReferenceException>(headNode != null);
            INode node = headNode;
            int i = startIndex;
            while (node != null)
            {
                node.Index = i;
                node = node.PreviousSibling;
                i--;
            }
        }

        #endregion Static Methods

        #region Visit algorithms

        private static int recursion;

        public static IEnumerable<INode> PreOrderVisit(INode headNode)
        {
            yield return headNode;

            if (headNode.HasChildNodes)
                foreach (INode node in PreOrderVisit(headNode.FirstChild))
                    yield return node;
            if (headNode.HasNextSibling)
                foreach (INode node in PreOrderVisit(headNode.NextSibling))
                    yield return node;
        }

        public static IEnumerable<INode> PostOrderVisit(INode headNode)
        {
            if (headNode.HasChildNodes)
            {
                foreach (INode childNode in PreOrderVisit(headNode.FirstChild))
                    yield return childNode;
            }
            else
            {
                INode node = headNode.NextSibling;
                while (node != null)
                {
                    yield return node;
                    node = node.NextSibling;
                }
            }
            yield return headNode;
        }

        #endregion Visit algorithms

        #region IEnumerable<INode> Members

        IEnumerator<INode> IEnumerable<INode>.GetEnumerator()
        {
            return ((INode)this).ChildrenIterator.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<INode>)this).GetEnumerator();
        }

        #endregion IEnumerable<INode> Members

        #region INode Members

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        INode INode.Parent
        {
            get { return parent; }
            set
            {
                if (parent != value)
                {
                    parent = value;
                    foreach (INode node in PreOrderVisit(this))
                        node.Level = node.Parent.Level + 1;
                    OnParentChanged(this, new NodeEventArgs(parent));
                }
            }
        }

        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        INode INode.PreviousSibling
        {
            get { return previousSibling; }
            set { previousSibling = value; }
        }

        INode INode.NextSibling
        {
            get { return nextSibling; }
            set { nextSibling = value; }
        }

        INode INode.FirstChild
        {
            get { return firstChild; }
        }

        INode INode.LastChild
        {
            get { return lastChild; }
        }

        IEnumerable<INode> INode.ChildrenIterator
        {
            get { return ChildrenIterator; }
        }

        void INode.AppendChild(INode newNode)
        {
            OnAppendChild(newNode);
        }

        void INode.InsertBefore(INode newChild, INode refNode)
        {
            OnInsertBefore(newChild, refNode);
        }

        void INode.InsertAfter(INode newChild, INode refNode)
        {
            OnInsertAfter(newChild, refNode);
        }

        void INode.RemoveChild(INode oldChild)
        {
            OnRemoveChild(oldChild);
        }

        void INode.ReplaceChild(INode newChild, INode oldChild)
        {
            OnReplaceChild(newChild, oldChild);
        }

        void INode.PrependChild(INode newChild)
        {
            OnPrependChild(newChild);
        }

        #endregion INode Members
    }
}