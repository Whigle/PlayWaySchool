using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathProvider
{
	public static List<Node> Provide(Grid grid, Node startNode, Node endNode)
	{
		if(startNode == endNode)
		{
			return new List<Node>() { startNode };
		}

		if(grid.NodesInGrid == null || grid.NodesInGrid.Count == 0)
		{
			return new List<Node>(0);
		}

		Dictionary<int, NodeData> relevantNodes = new Dictionary<int, NodeData>();
		List<NodeData> openNodes = new List<NodeData>();
		List<NodeData> closedNodes = new List<NodeData>();

		NodeData nodeData = new NodeData(startNode) { gCost = 0, hCost = Node.OptimisticDistance(startNode, endNode) };
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
					nodeData = new NodeData(neighbour) { gCost = newGCost, hCost = Node.OptimisticDistance(neighbour, endNode) };
					relevantNodes.Add(neighbour.X * 100 + neighbour.Z, nodeData);
					openNodes.Add(nodeData);

					if(neighbour == endNode)
					{
						return RecreatePath(openNodes, closedNodes, endNode, startNode);
					}
				}
			}

			openNodes.RemoveAt(0);
			openNodes = openNodes.OrderBy(node => node.hCost).ToList();
			closedNodes.Add(currentNode);
		}

		return new List<Node>(0);
	}

	public static List<Node> Provide(Grid grid, Node startNode, System.Func<Node, bool> endNodePredicate)
	{
		if(endNodePredicate(startNode))
		{
			return new List<Node>() { startNode };
		}

		if(grid.NodesInGrid == null || grid.NodesInGrid.Count == 0)
		{
			return new List<Node>(0);
		}

		Dictionary<int, NodeData> relevantNodes = new Dictionary<int, NodeData>();
		List<NodeData> openNodes = new List<NodeData>();
		List<NodeData> closedNodes = new List<NodeData>();

		Node possibleEndNode = grid.NodesInGrid.Values.Where(endNodePredicate).OrderBy(node => Vector3.Distance(node.transform.position, startNode.transform.position)).FirstOrDefault();

		NodeData nodeData = new NodeData(startNode) { gCost = 0, hCost = Node.OptimisticDistance(startNode, possibleEndNode) };
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
					nodeData = new NodeData(neighbour) { gCost = newGCost, hCost = Node.OptimisticDistance(neighbour, possibleEndNode) };
					relevantNodes.Add(neighbour.X * 100 + neighbour.Z, nodeData);
					openNodes.Add(nodeData);

					if(endNodePredicate(neighbour))
					{
						foreach(var relevantNode in relevantNodes)
						{
							relevantNode.Value.Node.text.text = "_" + relevantNode.Value.FCost.ToString() + "_";
						}

						return RecreatePath(openNodes, closedNodes, neighbour, startNode);
					}
				}
			}

			openNodes.RemoveAt(0);
			openNodes = openNodes.OrderBy(node => node.hCost).ToList();
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
			IEnumerable<NodeData> neighboursData = relevantNodes.Where(nodeData => nodeData.Node.Neighbours.Contains(currentNode.Node) && !path.Contains(nodeData.Node));
			NodeData nextNode = neighboursData.OrderBy(nodeData => nodeData.gCost).FirstOrDefault();

			path.Add(nextNode.Node);

			if(nextNode.Node == startNode)
			{
				return path;
			}

			currentNode = nextNode;
		}
	}
}

public class NodeData
{
	public int gCost;
	public int hCost;
	public int obstacleInpact;

	public Node Node { get; private set; }
	public int FCost => gCost + hCost + obstacleInpact;
	public int X => Node.X;
	public int Z => Node.Z;

	public NodeData(Node node)
	{
		this.Node = node;

		foreach(var neighbour in node.Neighbours)
		{
			if(neighbour.State == NodeState.Unwalkable)
			{
				obstacleInpact -= 14;
				Node.text.text = obstacleInpact.ToString();
			}
		}
	}
}