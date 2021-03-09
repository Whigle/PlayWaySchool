using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Node : MonoBehaviour
{
	public event Action<Node> Destroyed;

	[SerializeField]
	private List<Node> neighbours;
	[SerializeField]
	private Material material;
	[SerializeField]
	public TMP_Text text;

	[ReadOnly]
	public int X;
	[ReadOnly]
	public int Z;

	public IReadOnlyCollection<Node> Neighbours => neighbours;
	public Material Material => (material != null) ? material : material = GetComponentInChildren<MeshRenderer>().material;

	public NodeState State = NodeState.Walkable;

	private void OnDestroy()
	{
		Destroyed?.Invoke(this);
	}

	public static int OptimisticDistance(Node node1, Node node2)
	{
		int xOffset = Mathf.Abs(node1.X - node2.X);
		int zOffset = Mathf.Abs(node1.Z - node2.Z);

		return (Mathf.Min(xOffset, zOffset) * 14) + Mathf.Abs(xOffset - zOffset) * 10;
	}

	/// <summary>
	/// Get Node for given direction, x in x world direction (left, right), y in z world direction (forward, back).
	/// </summary>
	/// <param name="direction">x and y should be at -1, 0 or 1.</param>
	/// <returns>Neighbour Node in given direction.</returns>
	public Node GetNeighbour(Vector2Int direction)
	{
		return neighbours.FirstOrDefault(node => node.X == X + direction.x && node.Z == Z + direction.y);
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
}

public enum NodeState
{
	Walkable, Unwalkable, Occupied
}
