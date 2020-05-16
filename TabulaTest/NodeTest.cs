using System;
using NUnit.Framework;
using tabula;

namespace TabulaTest
{
    public class NodeTest
    {
        [SetUp]
        public void Setup()
        {
            
        }
        
        [Test]
        public void CreateNodeWithData()
        {
            var nodeData = "a string data";
            var node = new Node(2, nodeData);

            Assert.AreSame(node.Data, nodeData);
        }

        [Test]
        public void CreateNodeWithNoData()
        {
            var node = new Node(2);
            
            Assert.Null(node.Data);
        }

        [Test]
        public void AddNeighbourHappyPath()
        {
            var node = new Node(2, "node 1");
            var neighbour = new Node(2, "node 2");
            
            node.AddNeighbour(neighbour);
            
            Assert.IsTrue(node.HasNeighbour(neighbour));
        }

        [Test]
        public void AddNeighbourThrowIfMaxNeighbourReached()
        {
            var node = new Node(1, "node 1");
            var neighbour = new Node(1,"node 2");
            var failingNeighbour = new Node(1, "node 3");
            
            node.AddNeighbour(neighbour);

            Assert.Throws<Exception>(() => node.AddNeighbour(failingNeighbour));
        }
    }
}