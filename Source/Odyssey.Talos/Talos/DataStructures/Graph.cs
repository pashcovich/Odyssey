using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Talos.DataStructures
{
    /// <summary>
    /// The Graph class represents a graph, which is composed of a collection of nodes and edges.  This Graph class
    /// maintains its collection of nodes using the NodeList class, which is a collection of TNode objects.
    /// It delegates the edge maintenance to the TNode class.  The TNode class maintains the edge information using
    /// the adjacency list technique.
    /// </summary>
    public class Graph
    {
        #region Private Member Variables
        // private member variables
        private NodeList nodes;
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor.  Creates a new Graph class instance.
        /// </summary>
        public Graph()
        {
            this.nodes = new NodeList();
        }

        /// <summary>
        /// Creates a new graph class instance based on a list of nodes.
        /// </summary>
        /// <param name="nodes">The list of nodes to populate the newly created Graph class with.</param>
        public Graph(NodeList nodes)
        {
            this.nodes = nodes;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Clears out all of the nodes in the graph.
        /// </summary>
        public virtual void Clear()
        {
            nodes.Clear();
        }

        #region Adding TNode Methods
        /// <summary>
        /// Adds a new node to the graph.
        /// </summary>
        /// <param name="n">A node instance to add to the graph</param>
        /// <remarks>If there already exists a node in the graph with the same <b>key</b> value as <b>n</b> then an
        /// <b>ArgumentException</b> exception will be thrown.</remarks>
        public virtual void AddNode(IGraphNode n)
        {
            // Make sure this node is unique
            if (!nodes.ContainsKey(n.Name))
                nodes.Add(n);
            else
                throw new ArgumentException("There already exists a node in the graph with key " + n.Name);
        }
        #endregion

        #region Adding Edge Methods
        /// <summary>
        /// Adds a directed edge from one node to another.
        /// </summary>
        /// <param name="uKey">The <b>Key</b> of the node from which the directed edge eminates.</param>
        /// <param name="vKey">The <b>Key</b> of the node from which the directed edge leads to.</param>
        /// <remarks>If nodes with <b>uKey</b> and <b>vKey</b> do not exist in the graph, an <b>ArgumentException</b>
        /// exception is thrown.</remarks>
        public virtual void AddDirectedEdge(string uKey, string vKey)
        {
            AddDirectedEdge(uKey, vKey, 0);
        }

        /// <summary>
        /// Adds a directed, weighted edge from one node to another.
        /// </summary>
        /// <param name="uKey">The <b>Key</b> of the node from which the directed edge eminates.</param>
        /// <param name="vKey">The <b>Key</b> of the node from which the directed edge leads to.</param>
        /// <param name="cost">The weight of the edge.</param>
        /// <remarks>If nodes with <b>uKey</b> and <b>vKey</b> do not exist in the graph, an <b>ArgumentException</b>
        /// exception is thrown.</remarks>
        public virtual void AddDirectedEdge(string uKey, string vKey, int cost)
        {
            // get references to uKey and vKey
            if (nodes.ContainsKey(uKey) && nodes.ContainsKey(vKey))
                AddDirectedEdge(nodes[uKey], nodes[vKey], cost);
            else
                throw new ArgumentException("One or both of the nodes supplied were not members of the graph.");
        }

        /// <summary>
        /// Adds a directed edge from one node to another.
        /// </summary>
        /// <param name="u">The node from which the directed edge eminates.</param>
        /// <param name="v">The node from which the directed edge leads to.</param>
        /// <remarks>If the passed-in nodes do not exist in the graph, an <b>ArgumentException</b>
        /// exception is thrown.</remarks>
        public virtual void AddDirectedEdge(IGraphNode u, IGraphNode v)
        {
            AddDirectedEdge(u, v, 0);
        }

        /// <summary>
        /// Adds a directed, weighted edge from one node to another.
        /// </summary>
        /// <param name="u">The node from which the directed edge eminates.</param>
        /// <param name="v">The node from which the directed edge leads to.</param>
        /// <param name="cost">The weight of the edge.</param>
        /// <remarks>If the passed-in nodes do not exist in the graph, an <b>ArgumentException</b>
        /// exception is thrown.</remarks>
        public virtual void AddDirectedEdge(IGraphNode u, IGraphNode v, int cost)
        {
            // Make sure u and v are Nodes in this graph
            if (nodes.ContainsKey(u.Name) && nodes.ContainsKey(v.Name))
                // add an edge from u -> v
                u.AddDirected(new Connection(u, v, cost));
            else
                // one or both of the nodes were not found in the graph
                throw new ArgumentException("One or both of the nodes supplied were not members of the graph.");
        }

        /// <summary>
        /// Adds an undirected edge from one node to another.
        /// </summary>
        /// <remarks>If nodes with <b>uKey</b> and <b>vKey</b> do not exist in the graph, an <b>ArgumentException</b>
        /// exception is thrown.</remarks>
        public virtual void AddUndirectedEdge(string uKey, string vKey)
        {
            AddUndirectedEdge(uKey, vKey, 0);
        }

        /// <summary>
        /// Adds an undirected, weighted edge from one node to another.
        /// </summary>
        /// <param name="cost">The weight of the edge.</param>
        /// <remarks>If nodes with <b>uKey</b> and <b>vKey</b> do not exist in the graph, an <b>ArgumentException</b>
        /// exception is thrown.</remarks>
        public virtual void AddUndirectedEdge(string uKey, string vKey, int cost)
        {
            // get references to uKey and vKey
            if (nodes.ContainsKey(uKey) && nodes.ContainsKey(vKey))
                AddUndirectedEdge(nodes[uKey], nodes[vKey], cost);
            else
                throw new ArgumentException("One or both of the nodes supplied were not members of the graph.");
        }

        /// <summary>
        /// Adds an undirected edge from one node to another.
        /// </summary>
        /// <remarks>If the passed-in nodes do not exist in the graph, an <b>ArgumentException</b>
        /// exception is thrown.</remarks>
        public virtual void AddUndirectedEdge(IGraphNode u, IGraphNode v)
        {
            AddUndirectedEdge(u, v, 0);
        }

        /// <summary>
        /// Adds an undirected, weighted edge from one node to another.
        /// </summary>
        /// <param name="cost">The weight of the edge.</param>
        /// <remarks>If the passed-in nodes do not exist in the graph, an <b>ArgumentException</b>
        /// exception is thrown.</remarks>
        public virtual void AddUndirectedEdge(IGraphNode u, IGraphNode v, int cost)
        {
            // Make sure u and v are Nodes in this graph
            if (nodes.ContainsKey(u.Name) && nodes.ContainsKey(v.Name))
            {
                // Add an edge from u -> v and from v -> u
                u.AddDirected(new Connection(u, v, cost));
                v.AddDirected(new Connection(v, u, cost));
            }
            else
                // one or both of the nodes were not found in the graph
                throw new ArgumentException("One or both of the nodes supplied were not members of the graph.");
        }

        /// <summary>
        /// Adds an undirected, weighted edge from one node to another.
        /// </summary>
        /// <param name="cost">The weight of the edge.</param>
        /// <remarks>If the passed-in nodes do not exist in the graph, an <b>ArgumentException</b>
        /// exception is thrown.</remarks>
        public virtual void AddUndirectedEdge(IGraphNode u, IGraphNode v, double cost)
        {
            // Make sure u and v are Nodes in this graph
            if (nodes.ContainsKey(u.Name) && nodes.ContainsKey(v.Name))
            {
                // Add an edge from u -> v and from v -> u
                u.AddDirected(new Connection(u, v, (float)cost));
                v.AddDirected(new Connection(v, u, (float)cost));
            }
            else
                // one or both of the nodes were not found in the graph
                throw new ArgumentException("One or both of the nodes supplied were not members of the graph.");
        }

        #endregion

        #region Contains Methods
        /// <summary>
        /// Determines if a node exists within the graph.
        /// </summary>
        /// <param name="n">The node to check for in the graph.</param>
        /// <returns><b>True</b> if the node <b>n</b> exists in the graph, <b>False</b> otherwise.</returns>
        public virtual bool Contains(IGraphNode n)
        {
            return Contains(n.Name);
        }

        /// <summary>
        /// Determines if a node exists within the graph.
        /// </summary>
        /// <param name="key">The key of the node to check for in the graph.</param>
        /// <returns><b>True</b> if a node with key <b>key</b> exists in the graph, <b>False</b> otherwise.</returns>
        public virtual bool Contains(string key)
        {
            return nodes.ContainsKey(key);
        }
        #endregion
        #endregion

        #region Public Properties
        /// <summary>
        /// Returns the number of nodes in the graph.
        /// </summary>
        public virtual int Count
        {
            get
            {
                return nodes.Count;
            }
        }

        /// <summary>
        /// Returns a reference to the set of nodes in the graph.
        /// </summary>
        public virtual NodeList Nodes
        {
            get
            {
                return this.nodes;
            }
        }
        #endregion
    }
}
