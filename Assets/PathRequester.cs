using System.Collections.Generic;
using UnityEngine;

public class PathRequester : MonoBehaviour
{
	[SerializeField]
	private bool autoRequestOnMove = false;

	[SerializeField]
	private Grid grid;
	[SerializeField]
	private Transform startPositionT;
	[SerializeField]
	private Transform endPositionT;

	private Vector3 lastStartPosition;
	private Vector3 lastEndPosition;

	private void Update()
	{
		if(autoRequestOnMove)
		{
			if(startPositionT == null || endPositionT == null)
			{
				return;
			}

			if(startPositionT.position == lastStartPosition && endPositionT.position == lastEndPosition)
			{
				return;
			}

			RequestPath();
		}
	}

	[ContextMenu("Request Path")]
	private void RequestPath()
	{
		Node startNode = grid.GetNodeClosestToPosition(startPositionT.position);
		Node endNode = grid.GetNodeClosestToPosition(endPositionT.position);

		ClearColorsForNodes(grid.NodesInGrid);

		List<Node> path = PathProvider.Provide(grid, startNode, endNode);

		foreach(var node in path)
		{
			SetColorForNode(node, Color.green);
		}

		lastStartPosition = startPositionT.position;
		lastEndPosition = endPositionT.position;
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


