using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grid : MonoBehaviour
{
	[SerializeField]
	private BoxCollider gridVolume;
	[SerializeField]
	private int width;
	[SerializeField]
	private int height;
	[SerializeField]
	private Node[][] nodes;

	[SerializeField]
	private Transform startPositionT;
	[SerializeField]
	private Transform endPositionT;

	public float nodeXSize;
	public float nodeZSize;
	public float nodeXOffset;
	public float nodeZOffset;
	public Vector3 startingPosition;

	private int oldWidth;
	private int oldHeight;

	private void Awake()
	{
		CreateGrid();
		CalculatePath();
	}

	[ContextMenu("Create Grid")]
	private void CreateGrid()
	{
		nodes = new Node[width][];

		for(int i = 0; i < width; i++)
		{
			nodes[i] = new Node[height];
			for(int j = 0; j < height; j++)
			{
				nodes[i][j] = new Node() { X = i, Z = j, Center = startingPosition + (Vector3.right * nodeXSize * i) + (Vector3.forward * nodeZSize * j) };
			}
		}

		for(int i = 0; i < width; i++)
		{
			for(int j = 0; j < height; j++)
			{
				List<Node> neighbours = new List<Node>();

				for(int k = Mathf.Max(0, i - 1); k <= Mathf.Min(i + 1, width - 1); k++)
				{
					for(int l = Mathf.Max(0, j - 1); l <= Mathf.Min(j + 1, height - 1); l++)
					{
						if(k != i || j != l)
						{
							neighbours.Add(nodes[k][l]);
						}
					}
				}

				nodes[i][j].SetNeighbours(neighbours);
			}
		}

		//for(int i = 0; i < width; i++)
		//{
		//	List<Node> neighbours = new List<Node>();

		//	for(int j = 0; j < height; j++)
		//	{
		//		if(i > 0)
		//		{
		//			neighbours.Add(nodes[i - 1][j]);

		//			if(j > 0)
		//			{
		//				neighbours.Add(nodes[i - 1][j - 1]);
		//			}
		//			if(j < height - 1)
		//			{
		//				neighbours.Add(nodes[i - 1][j + 1]);
		//			}
		//		}

		//		if(i < width - 1)
		//		{
		//			neighbours.Add(nodes[i + 1][j]);

		//			if(j > 0)
		//			{
		//				neighbours.Add(nodes[i + 1][j - 1]);
		//			}
		//			if(j < height - 1)
		//			{
		//				neighbours.Add(nodes[i + 1][j + 1]);
		//			}
		//		}

		//		if(j > 0)
		//		{
		//			neighbours.Add(nodes[i][j - 1]);
		//		}

		//		if(j < height - 1)
		//		{
		//			neighbours.Add(nodes[i][j + 1]);
		//		}

		//		nodes[i][j].SetNeighbours(neighbours);
		//	}
		//}
	}

	private Vector3 lastStartPosition = Vector3.up;
	private Vector3 lastEndPosition = Vector3.up;

	private Node startNode;
	private Node endNode;

	private void Update()
	{
		if(startPositionT == null || endPositionT == null)
		{
			return;
		}

		if(startPositionT.position == lastStartPosition && endPositionT.position == lastEndPosition)
		{
			return;
		}

		ResetNodes();
		CalculatePath();

		lastStartPosition = startPositionT.position;
		lastEndPosition = endPositionT.position;

	}

	private void ResetNodes()
	{
		foreach(var nodesRow in nodes)
		{
			foreach(var node in nodesRow)
			{
				node.gCost = int.MaxValue / 10;
				node.hCost = int.MaxValue / 10;
			}
		}
	}

	private void CalculatePath()
	{
		if(nodes == null || nodes.Length == 0)
		{
			return;
		}

		List<Node> openNodes = new List<Node>();
		List<Node> closedNodes = new List<Node>();

		startNode = GetNodeClosestToPosition(startPositionT.position);
		endNode = GetNodeClosestToPosition(endPositionT.position);

		openNodes.Add(startNode);

		startNode.gCost = 0;
		startNode.hCost = GetHCost(startNode);

		while(openNodes.Count > 0)
		{
			Node currentNode = openNodes.First();

			IEnumerable<Node> neighbours = currentNode.Neighbours;

			foreach(var neighbour in neighbours)
			{
				int newGCost = 0;

				if(neighbour.X != currentNode.X && neighbour.Z != currentNode.Z)
				{
					newGCost = currentNode.gCost + 14;
				}
				else
				{
					newGCost = currentNode.gCost + 10;
				}

				if(neighbour.gCost > newGCost)
				{
					neighbour.gCost = newGCost;
				}

				if(!openNodes.Contains(neighbour) && !closedNodes.Contains(neighbour))
				{
					openNodes.Add(neighbour);
					neighbour.hCost = GetHCost(neighbour);

					if(neighbour == endNode)
					{
						Debug.Log(neighbour.fCost);
						SavePath();

						return;
					}
				}
			}

			openNodes.RemoveAt(0);
			openNodes.OrderBy(node => node.fCost);
			closedNodes.Add(currentNode);
		}
	}

	public List<Node> path = new List<Node>();

	private void SavePath()
	{
		path.Clear();

		Node currentNode = endNode;
		int i = width * height;
		while(i > 0)
		{
			Node nextNode = currentNode.Neighbours.OrderBy(node => node.gCost).FirstOrDefault();

			path.Add(nextNode);

			if(nextNode == startNode)
			{
				return;
			}

			currentNode = nextNode;
			i--;
		}
	}

	private int GetHCost(Node node)
	{
		int xOffset = Mathf.Abs(endNode.X - node.X);
		int zOffset = Mathf.Abs(endNode.Z - node.Z);

		return (Mathf.Max(xOffset, zOffset) * 14) + Mathf.Abs(xOffset - zOffset) * 10;
	}

	private Node GetNodeClosestToPosition(Vector3 position)
	{
		List<Node> helperList = new List<Node>();

		foreach(var nodesRow in nodes)
		{
			helperList.AddRange(nodesRow);
		}

		return helperList.Aggregate((closest, node) => (closest == null || Vector3.Distance(position, closest.Center) > Vector3.Distance(node.Center, position)) ? node : closest);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;

		if(oldWidth != width || oldHeight != height)
		{
			Recalculate();
		}


		if(nodes == null)
		{
			for(int i = 0; i < width * height; i++)
			{
				int x = i % width;
				int z = i / width;
				Gizmos.DrawWireCube(startingPosition + (Vector3.right * nodeXSize * x) + (Vector3.forward * nodeZSize * z), new Vector3(nodeXSize, 0f, nodeZSize));
			}
		}
		else
		{
			foreach(var nodeRow in nodes)
			{
				foreach(var node in nodeRow)
				{
					if(node == startNode)
					{
						Gizmos.color = Color.yellow;
					}
					else if(node == endNode)
					{
						Gizmos.color = Color.magenta;
					}
					else
					{
						Gizmos.color = new Color(0.5f, 0.5f + node.X * 0.5f / width, 0.5f + node.Z * 0.5f / height);
					}
					Gizmos.DrawWireCube(node.Center, new Vector3(nodeXSize, 0f, nodeZSize));
				}
			}
		}

		if(gridVolume == null)
		{
			return;
		}

		if(path != null && path.Count > 0)
		{
			foreach(var node in path)
			{
				Gizmos.color = Color.black;
				Gizmos.DrawSphere(node.Center, 1f);
			}
		}

		Gizmos.DrawWireCube(gridVolume.transform.position, gridVolume.size);
	}

	private void Recalculate()
	{
		if(gridVolume == null)
		{
			return;
		}

		nodeXSize = gridVolume.size.x / width;
		nodeZSize = gridVolume.size.z / height;
		nodeXOffset = nodeXSize * 0.5f;
		nodeZOffset = nodeZSize * 0.5f;

		startingPosition = gridVolume.transform.position - gridVolume.size * 0.5f + Vector3.right * nodeXOffset + Vector3.forward * nodeZOffset;
	}
}
