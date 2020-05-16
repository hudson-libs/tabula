using System;
using System.Collections.Generic;

namespace tabula
{
    public class Node
    {
        public Node(int maxNeighbours, object data = null)
        {
            MaxNeighbours = maxNeighbours;
            Neighbours = new List<Node>(MaxNeighbours);

            if (data != null)
            {
                Data = data;
            }
        }

        public int MaxNeighbours { get; }

        public object Data { get; set; }
        public List<Node> Neighbours { get; }

        public void AddNeighbour(Node neighbour)
        {
            if (Neighbours.Count == MaxNeighbours) throw new Exception("You can't add neighbours to this node: " + this);
            
            Neighbours.Add(neighbour);
        }

        public bool HasNeighbour(Node neighbour)
        {
            return Neighbours.Contains(neighbour);
        }
    }
}