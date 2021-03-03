using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Node : MonoBehaviour
{
	public event Action<Node> Destroyed;

	[SerializeField]
	private List<Node> neighbours;
	[SerializeField]
	private Material material;

	[ReadOnly]
	public int X;
	[ReadOnly]
	public int Z;

	public IReadOnlyCollection<Node> Neighbours => neighbours;
	public Material Material => (material != null) ? material : material = GetComponentInChildren<MeshRenderer>().material;

	private void OnDestroy()
	{
		Destroyed?.Invoke(this);
	}

	public void AddNeighbour(Node neighbour)
	{
		if(neighbours.Contains(neighbour))
		{
			return;
		}

		neighbours.Add(neighbour);
		neighbour.Destroyed += Neighbour_Destroyed;
	}

	private void Neighbour_Destroyed(Node neighbour)
	{
		RemoveNeighbour(neighbour);
	}

	public void RemoveNeighbour(Node neighbour)
	{
		if(!neighbours.Contains(neighbour))
		{
			return;
		}

		neighbours.Remove(neighbour);
		neighbour.Destroyed -= Neighbour_Destroyed;
	}

	public void AddNeighbours(IEnumerable<Node> neighbours)
	{
		foreach(var neighbour in neighbours)
		{
			AddNeighbour(neighbour);
		}
	}

	public void RemoveNeighbours(IEnumerable<Node> neighbours)
	{
		for(int i = neighbours.Count() - 1; i >= 0; i--)
		{
			RemoveNeighbour(neighbours.ElementAt(i));
		}
	}

	public void SetNeighbours(IEnumerable<Node> neighbours)
	{
		if(neighbours != null || neighbours.Count() > 0)
		{
			ClearNeighbours();
		}

		AddNeighbours(neighbours);
	}

	public void ClearNeighbours()
	{
		RemoveNeighbours(neighbours);
	}

	//public int gCost = int.MaxValue / 10;
	//public int hCost = int.MaxValue / 10;
	//public int fCost => gCost + hCost;

	//public void SetNeighbours(IEnumerable<Node> neighbours)
	//{
	//	this.neighbours = neighbours.ToArray();
	//}
}
