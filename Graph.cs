using System;
using System.Collections.Generic;
using System.Linq;
using tabula.BoyerMyrvold.Graphs.DataStructures;
using tabula.BoyerMyrvold.PlanarityTesting.BoyerMyrvold;

namespace tabula
{
    public class Graph : IGraph<Node>
    {
        public Graph(int seed, GraphProperties properties)
        {
            Properties = properties;
            _seededRandomizer = new Random(seed);

            // max in non inclusive
            VerticesCount = _seededRandomizer.Next(Properties.MinNodes, Properties.MaxNodes + 1);
            Nodes = new List<Node>(VerticesCount);
            FillNodeList();
            ConnectNodes();
        }

        private Random _seededRandomizer;
        
        public readonly GraphProperties Properties;
        public readonly List<Node> Nodes;
        private readonly List<Edge<Node>> _edges = new List<Edge<Node>>();

        public bool IsPlanar()
        {
            return new BoyerMyrvold<Node>().IsPlanar(this);
        }

        private void FillNodeList()
        {
            var totalNeighbours = 0;

            for (var i = 0; i < Nodes.Capacity; i++)
            {
                var isLastNode = i == Nodes.Capacity - 1;
                var isFirstNode = i == 0;
                var thisNodeNumberOfNeighbours = _seededRandomizer.Next(Properties.MinNeighboursPerNode,
                    Properties.MaxNeighboursPerNode + 1);

                if (isFirstNode)
                {
                    Nodes.Add(new Node(Properties.FirstNodeNeighbours, i));
                }
                else if (isLastNode)
                {
                    Nodes.Add(new Node(CorrectLastNodeNumberOfNeighbours(totalNeighbours, thisNodeNumberOfNeighbours),
                        i));
                }
                else
                {
                    Nodes.Add(new Node(thisNodeNumberOfNeighbours, i));
                }

                totalNeighbours += Nodes[i].MaxNeighbours;
            }
        }

        private void ConnectNodes()
        {
            var totalNodeLinksToMake = Nodes.Select(node => node.MaxNeighbours).Sum();
            var currentlyConnectingNode = Nodes.First();

            while (totalNodeLinksToMake > 0)
            {
                if (currentlyConnectingNode == null) break;

                while (currentlyConnectingNode.Neighbours.Count < currentlyConnectingNode.MaxNeighbours)
                {
                    var minNumberOfNeighbours = Nodes.Min(node => node.Neighbours.Count);

                    var suitableNeighbours = Nodes
                        .Where(node => node.Neighbours.Count == minNumberOfNeighbours)
                        .Where(node => node != currentlyConnectingNode)
                        .Where(node => node.Neighbours.Count < node.MaxNeighbours)
                        .Where(node => !node.HasNeighbour(currentlyConnectingNode));

                    var firstSuitableNeighbour =
                        suitableNeighbours.ElementAtOrDefault(
                            _seededRandomizer.Next(0, suitableNeighbours.Count()));

                    if (firstSuitableNeighbour == null)
                    {
                        // repair last two links to make on the same node by simply ignoring to connect those neighbours
                        if (currentlyConnectingNode.Neighbours.Count != currentlyConnectingNode.MaxNeighbours - 2)
                        {
                            throw new Exception("Can't find a suitable neighbour to add");
                        }

                        totalNodeLinksToMake -= 2;
                        break;
                    }

                    currentlyConnectingNode.AddNeighbour(firstSuitableNeighbour);
                    firstSuitableNeighbour.AddNeighbour(currentlyConnectingNode);
                    _edges.Add(new Edge<Node>(currentlyConnectingNode, firstSuitableNeighbour));

                    totalNodeLinksToMake -= 2;
                }

                currentlyConnectingNode = Nodes.SkipWhile(node => node.Neighbours.Count == node.MaxNeighbours)
                    .FirstOrDefault();
            }
        }

        private int CorrectLastNodeNumberOfNeighbours(int currentNumberOfNeighbour, int lastNodeNumberOfNeighbour)
        {
            // total number of neighbour is correct
            if (currentNumberOfNeighbour % 2 == 0) return lastNodeNumberOfNeighbour;
            // don't go over the max number of neighbour for a specific node
            if (lastNodeNumberOfNeighbour == Properties.MaxNeighboursPerNode) return lastNodeNumberOfNeighbour - 1;

            return lastNodeNumberOfNeighbour + 1;
        }

        #region Boilerplate stuff to use BoyerMyrvold

        public int VerticesCount { get; }
        public bool IsDirected => false;

        public IEnumerable<Node> Vertices => Nodes;
        public IEnumerable<IEdge<Node>> Edges => _edges;

        public IEnumerable<Node> GetNeighbours(Node vertex)
        {
            return vertex.Neighbours;
        }

        public bool HasEdge(Node from, Node to)
        {
            return from.HasNeighbour(to);
        }

        public void AddEdge(Node from, Node to)
        {
            throw new Exception("AddEdge should never be called in the implemented graph");
        }

        public void AddVertex(Node vertex)
        {
            throw new Exception("AddVertex should never be called in the implemented graph");
        }
        #endregion
    }

    public struct GraphProperties
    {
        public GraphProperties(int minNodes, int maxNodes, int firstNodeNeighbours, int minNeighboursPerNode,
            int maxNeighboursPerNode)
        {
            MinNodes = minNodes;
            MaxNodes = maxNodes;
            FirstNodeNeighbours = firstNodeNeighbours;
            MinNeighboursPerNode = minNeighboursPerNode;
            MaxNeighboursPerNode = maxNeighboursPerNode;
        }

        public readonly int MinNodes;
        public readonly int MaxNodes;
        public readonly int FirstNodeNeighbours;
        public readonly int MinNeighboursPerNode;
        public readonly int MaxNeighboursPerNode;
        // private int _maxLeafs; will experiment with this later
    }
}