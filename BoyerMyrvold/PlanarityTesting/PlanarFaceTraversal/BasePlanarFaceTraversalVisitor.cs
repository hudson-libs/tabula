﻿﻿using tabula.BoyerMyrvold.Graphs.DataStructures;

namespace tabula.BoyerMyrvold.PlanarityTesting.PlanarFaceTraversal
{
	internal abstract class BasePlanarFaceTraversalVisitor<T> : IPlanarFaceTraversalVisitor<T>
	{
		public virtual void BeginTraversal()
		{

		}

		public virtual void BeginFace()
		{

		}

		public virtual void NextEdge(IEdge<T> edge)
		{

		}

		public virtual void NextVertex(T vertex)
		{

		}

		public virtual void EndFace()
		{

		}

		public virtual void EndTraversal()
		{

		}
	}
}