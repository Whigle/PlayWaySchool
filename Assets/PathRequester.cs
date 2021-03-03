using System.Collections.Generic;
using UnityEngine;

public class PathRequester : MonoBehaviour
{
	[SerializeField]
	private bool autoRequestOnMove = false;

	[SerializeField]
	private Grid grid;
	[SerializeField]
	private MovableObject seeker;
	[SerializeField]
	private MovableObject wanted;

	private Vector3 lastStartPosition;
	private Vector3 lastEndPosition;

	private void Update()
	{
		if(autoRequestOnMove)
		{
			if(seeker == null || wanted == null)
			{
				return;
			}

			if(seeker.Position == lastStartPosition && wanted.Position == lastEndPosition)
			{
				return;
			}

			RequestPath();
		}
	}

	[ContextMenu("Request Path")]
	private void RequestPath()
	{
		Node startNode = grid.GetNodeClosestToPosition(seeker.Position);
		Node endNode = grid.GetNodeClosestToPosition(wanted.Position);

		ClearColorsForNodes(grid.NodesInGrid);

		List<Node> path = PathProvider.Provide(grid, startNode, endNode);

		foreach(var node in path)
		{
			SetColorForNode(node, Color.green);
		}

		lastStartPosition = seeker.Position;
		lastEndPosition = wanted.Position;
	}

	private void SetColorForNode(Node node, Color color)
	{
		node.Material.color = color;
	}

	private void ClearColorsForNodes(IReadOnlyDictionary<int, Node> nodesInGrid)
	{
		foreach(var Node in nodesInGrid.Values)
		{
			Node.Material.color = Color.white;
		}
	}
}


