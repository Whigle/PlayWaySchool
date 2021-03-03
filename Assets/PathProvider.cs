using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathProvider
{
	public static List<Node> Provide(Grid grid, Node startNode, Node endNode)
	{
		if(grid.NodesInGrid == null || grid.NodesInGrid.Count == 0)
		{
			return new List<Node>(0);
		}

		Dictionary<int, NodeData> relevantNodes = new Dictionary<int, NodeData>();
		List<NodeData> openNodes = new List<NodeData>();
		List<NodeData> closedNodes = new List<NodeData>();

		NodeData nodeData = new NodeData(startNode) { gCost = -1, hCost = GetHCost(startNode, endNode) };
		relevantNodes.Add(startNode.X * 100 + startNode.Z, nodeData);
		openNodes.Add(nodeData);

		while(openNodes.Count > 0)
		{
			NodeData currentNode = openNodes.First();

			IEnumerable<Node> neighbours = currentNode.Node.Neighbours;

			foreach(var neighbour in neighbours)
			{
				if(neighbour.State == NodeState.Unwalkable)
				{
					continue;
				}

				int newGCost = 0;

				if(neighbour.X != currentNode.X && neighbour.Z != currentNode.Z)
				{
					newGCost = currentNode.gCost + 14;
				}
				else
				{
					newGCost = currentNode.gCost + 10;
				}

				if(relevantNodes.TryGetValue((neighbour.X * 100 + neighbour.Z), out NodeData neighbourNodeData))
				{
					if(neighbourNodeData.gCost > newGCost)
					{
						neighbourNodeData.gCost = newGCost;
					}
				}
				else
				{
					nodeData = new NodeData(neighbour) { gCost = newGCost, hCost = GetHCost(startNode, endNode) };
					relevantNodes.Add(neighbour.X * 100 + neighbour.Z, nodeData);
					openNodes.Add(new NodeData(neighbour) { gCost = newGCost, hCost = GetHCost(neighbour, endNode) });

					if(neighbour == endNode)
					{
						return RecreatePath(openNodes, closedNodes, endNode, startNode);
					}
				}
			}

			openNodes.RemoveAt(0);
			openNodes.OrderBy(node => node.FCost);
			closedNodes.Add(currentNode);
		}

		return new List<Node>(0);
	}

	private static List<Node> RecreatePath(List<NodeData> openNodes, List<NodeData> closedNodes, Node endNode, Node startNode)
	{
		List<NodeData> relevantNodes = openNodes;
		relevantNodes.AddRange(closedNodes);

		NodeData currentNode = relevantNodes.First(nodeData => nodeData.Node == endNode);
		List<Node> path = new List<Node>();
		path.Add(endNode);

		while(true)
		{
			IEnumerable<NodeData> neighboursData = relevantNodes.Where(nodeData => nodeData.Node.Neighbours.Contains(currentNode.Node));
			NodeData nextNode = neighboursData.OrderBy(nodeData => nodeData.gCost).FirstOrDefault();

			path.Add(nextNode.Node);

			if(nextNode.Node == startNode)
			{
				return path;
			}

			currentNode = nextNode;
		}
	}

	private static int GetHCost(Node currentNode, Node endNode)
	{
		int xOffset = Mathf.Abs(endNode.X - currentNode.X);
		int zOffset = Mathf.Abs(endNode.Z - currentNode.Z);

		return (Mathf.Max(xOffset, zOffset) * 14) + Mathf.Abs(xOffset - zOffset) * 10;
	}
}

public class NodeData
{
	public int gCost;
	public int hCost;

	public Node Node { get; private set; }
	public int FCost => gCost + hCost;
	public int X => Node.X;
	public int Z => Node.Z;

	public NodeData(Node node)
	{
		this.Node = node;
	}
}