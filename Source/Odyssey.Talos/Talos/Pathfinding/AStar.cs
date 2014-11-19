using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Talos.DataStructures;

namespace Odyssey.Talos.Pathfinding
{
    class AStar
    {
        /// <summary>
        /// This is the method responsible for finding the shortest path between a Start and Destination cities using the A*
        /// search algorithm.
        /// </summary>
        /// <typeparam name="TNode">The Node type</typeparam>
        /// <param name="start">Start city</param>
        /// <param name="destination">Destination city</param>
        /// <param name="distance">Function which tells us the exact distance between two neighbours.</param>
        /// <param name="estimate">Function which tells us the estimated distance between the last node on a proposed path and the
        /// destination node.</param>
        /// <returns></returns>
        static public Path<TNode> FindPath<TNode>(
            TNode start,
            TNode destination,
            Func<TNode, TNode, double> distance,
            Func<TNode, double> estimate) 
            where TNode : IHasNeighbours<TNode>
        {
            var closed = new HashSet<TNode>();

            var queue = new HeapPriorityQueue<Path<TNode>>(100);
            //var queue = new PriorityQueue<double, Path<TNode>>();
            queue.Enqueue(new Path<TNode>(start), 0);

            while (!queue.IsEmpty)
            {
                var path = queue.Dequeue();

                if (closed.Contains(path.LastStep))
                    continue;
                
                if (path.LastStep.Equals(destination))
                    return path;

                closed.Add(path.LastStep);

                foreach (TNode n in path.LastStep.Neighbours)
                {
                    double d = distance(path.LastStep, n);

                    var newPath = path.AddStep(n, d);

                    queue.Enqueue(newPath, newPath.TotalCost + estimate(n));
                }

                //ViewOtherPaths(queue, estimate);
            }

            return null;
        }
    }
}
