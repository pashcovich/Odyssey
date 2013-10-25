#region Using directives

using System;
using System.Linq;
using Odyssey.Utils;
using Odyssey.Utils.Collections;
using System.Collections.Generic;

#endregion

namespace Odyssey.Graphics.Rendering.Management
{


    public abstract class SceneNode : Node
    {
        #region Private Fields

        #endregion

        #region Properties



        public SceneNodeType NodeType { get; private set; }

        public bool Inited { get; protected set; }


        #endregion

        #region Methods

        public virtual void Initialize()
        {
            Inited = true;
        }

        public abstract void Update();

        protected override void OnChildAdded(object sender, NodeEventArgs e)
        {
            base.OnChildAdded(sender, e);
            SceneNode sNode = ((SceneNode) e.Node);
            if (!sNode.Inited)
                sNode.Initialize();
        }

        #endregion

        #region Base SceneNode Properties

        public SceneNode Parent
        {
            get { return ParentNode as SceneNode; }
        }

        public SceneNode PreviousSibling
        {
            get { return PreviousSiblingNode as SceneNode; }
        }

        public SceneNode NextSibling
        {
            get { return NextSiblingNode as SceneNode; }
        }

        public SceneNode FirstChild
        {
            get { return FirstChildNode as SceneNode; }
        }

        public SceneNode LastChild
        {
            get { return LastChildNode as SceneNode; }
        }


        #endregion

        #region Constructor

        protected SceneNode(string label, SceneNodeType nodeType)
        {
            Label = label;
            NodeType = nodeType;
        }

        protected SceneNode(SceneNodeType nodeType) : this(nodeType.ToString(), nodeType)
        {
        }

        #endregion

        #region Base Node Methods

        public void AppendChildren(IEnumerable<SceneNode> newChildren)
        {
            foreach (SceneNode node in newChildren)
                AppendChild(node);
        }

        public void AppendChild(SceneNode newChild)
        {
            OnAppendChild(newChild);
        }

        /// <summary>
        ///   Removes the node from this node's children.
        /// </summary>
        /// <param name = "oldChild">The node to remove.</param>
        public void RemoveChild(SceneNode oldChild)
        {
            OnRemoveChild(oldChild);
        }

        /// <summary>
        ///   Replaces a node with another one.
        /// </summary>
        /// <param name = "newChild">The new node.</param>
        /// <param name = "oldChild">The node to be replaced.</param>
        public void ReplaceChild(SceneNode newChild, SceneNode oldChild)
        {
            OnReplaceChild(newChild, oldChild);
        }

        /// <summary>
        ///   Inserts a new node immediately before the referenced one.
        /// </summary>
        /// <param name = "newChild">The new node.</param>
        /// <param name = "refNode">The referenced node.</param>
        public void InsertBefore(SceneNode newChild, SceneNode refNode)
        {
            OnInsertBefore(newChild, refNode);
        }

        /// <summary>
        ///   Inserts a new node immediately after the referenced one.
        /// </summary>
        /// <param name = "newChild">The new node.</param>
        /// <param name = "refNode">The referenced node.</param>
        public void InsertAfter(SceneNode newChild, SceneNode refNode)
        {
            OnInsertAfter(newChild, refNode);
        }

        /// <summary>
        ///   Inserts the specified node before every other node.
        /// </summary>
        /// <param name = "newChild">The new child node.</param>
        public void PrependChild(SceneNode newChild)
        {
            OnPrependChild(newChild);
        }

        public bool Contains(string label)
        {
            return ChildrenIterator.Cast<SceneNode>().Any(node => node.Label == label);
        }

        public SceneNode Find(string label)
        {
            SceneNode result = null;

            foreach (SceneNode node in ChildrenIterator.Where(node => node.Label == label))
            {
                result = node;
            }

            return result;
        }

        //public SceneNodeCollection<T> SelectDescendants<T>(Predicate<T> predicate)
        //    where T : SceneNode
        //{
        //    SceneNodeCollection<T> nodeCollection = new SceneNodeCollection<T>();
        //    foreach (T node in PreOrderVisit(this).OfType<T>().Where(node => node != null && predicate(node)))
        //    {
        //        nodeCollection.Add(node);
        //    }

        //    return nodeCollection;
        //}

        //public SceneNodeCollection<T> SelectDescendants<T>()
        //    where T : SceneNode
        //{
        //    SceneNodeCollection<T> nodeCollection = new SceneNodeCollection<T>();
        //    foreach (T nodeT in PreOrderVisit(this).OfType<T>())
        //    {
        //        nodeCollection.Add(nodeT);
        //    }

        //    return nodeCollection;
        //}

        #endregion

        #region Static Methods

        /// <summary>
        ///   Finds first node of type TNode that is parent of startingNode.
        /// </summary>
        /// <typeparam name = "TNode">The type of the Node, derived from SceneNode.</typeparam>
        /// <param name = "startingNode">The node from where to start searching.</param>
        /// <returns>The node of type T if found, otherwise null.</returns>
        public static TNode FindFirstTParentNode<TNode>(SceneNode startingNode)
            where TNode : SceneNode
        {
            SceneNode node = startingNode;
            while (node != null)
            {
                TNode tNode = node as TNode;
                if (tNode == null)
                    node = node.Parent;
                else
                    return tNode;
            }
            return null;
        }

        /// <summary>
        ///   Finds first node of type TNode that is a child of startingNode.
        /// </summary>
        /// <typeparam name = "TNode">The type of the Node, derived from SceneNode.</typeparam>
        /// <param name = "startingNode">The node from where to start searching.</param>
        /// <returns>The node of type T if found, otherwise null.</returns>
        public static TNode FindFirstTChildNode<TNode>(SceneNode startingNode)
            where TNode : SceneNode
        {
            foreach (INode node in PreOrderVisit(startingNode))
            {
                TNode tNode = node as TNode;
                if (tNode == null)
                    continue;
                else
                    return tNode;
            }
            return null;
        }

        #endregion
    }
}