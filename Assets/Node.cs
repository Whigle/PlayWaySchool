using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Node
{
	private Node[] neighbours;

	public Vector3 Center;
	public int X;
	public int Z;

	public IReadOnlyCollection<Node> Neighbours => neighbours;

	public int gCost = int.MaxValue / 10;
	public int hCost = int.MaxValue / 10;
	public int fCost => gCost + hCost;

	public void SetNeighbours(IEnumerable<Node> neighbours)
	{
		this.neighbours = neighbours.ToArray();
	}
}
