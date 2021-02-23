using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Node
{
	private Node[] neighbours;
	private Vector3 center;

	public Vector3 Center
	{
		get
		{
			return center;
		}
		set
		{
			center = value;
			if(Physics.Raycast(center + Vector3.up * 10f, Vector3.down, out RaycastHit hit, 8.5f))
			{
				IsWalkable = false;
			}
			else
			{
				IsWalkable = true;
			}
		}
	}
	public int X;
	public int Z;

	public IReadOnlyCollection<Node> Neighbours => neighbours;

	public int gCost = int.MaxValue / 10;
	public int hCost = int.MaxValue / 10;
	public int fCost => gCost + hCost;

	public bool IsWalkable { get; internal set; }

	public void SetNeighbours(IEnumerable<Node> neighbours)
	{
		this.neighbours = neighbours.ToArray();
	}
}
