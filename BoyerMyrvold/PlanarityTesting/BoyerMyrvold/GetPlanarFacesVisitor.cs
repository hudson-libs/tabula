﻿﻿using System.Collections.Generic;
using tabula.BoyerMyrvold.PlanarityTesting.PlanarFaceTraversal;

namespace tabula.BoyerMyrvold.PlanarityTesting.BoyerMyrvold
{
	internal class GetPlanarFacesVisitor<T> : BasePlanarFaceTraversalVisitor<T>
	{
		public List<List<T>> Faces { get; private set; }

		private List<T> currentFace;

		public override void BeginTraversal()
		{
			Faces = new List<List<T>>();
			currentFace = new List<T>();
		}

		public override void NextVertex(T vertex)
		{
			currentFace.Add(vertex);
		}

		public override void EndFace()
		{
			Faces.Add(currentFace);
			currentFace = new List<T>();
		}
	}
}