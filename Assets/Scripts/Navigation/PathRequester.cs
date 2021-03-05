﻿using System.Collections.Generic;
using UnityEngine;

public class PathRequester : MonoBehaviour
{
	[SerializeField]
	private bool autoRequestOnMove = false;

	[SerializeField]
	private Grid grid;
	[SerializeField]
	private MovableObject wanted;

	private Vector3 lastStartPosition;
	private Vector3 lastEndPosition;

	private List<GridRequestedPathMover> pathRequesters = new List<GridRequestedPathMover>();

	private static PathRequester me;
	public static PathRequester Me
	{
		get
		{
			if(me == null)
			{
				me = FindObjectOfType<PathRequester>();
			}

			return me;
		}
	}

	public void Register(GridRequestedPathMover mover)
	{
		if(pathRequesters.Contains(mover))
		{
			return;
		}

		pathRequesters.Add(mover);
	}

	public void Unregister(GridRequestedPathMover mover)
	{
		if(!pathRequesters.Contains(mover))
		{
			return;
		}

		pathRequesters.Remove(mover);
	}

	private void Update()
	{
		if(autoRequestOnMove)
		{
			if(pathRequesters == null || pathRequesters .Count == 0|| wanted == null)
			{
				return;
			}

			if(wanted.Position == lastEndPosition)
			{
				return;
			}

			lastEndPosition = wanted.Position;
			Node endNode = grid.GetNodeClosestToPosition(wanted.Position);

			ClearColorsForNodes(grid.NodesInGrid);

			foreach(var seeker in pathRequesters)
			{
				Queue<Node> pathToTarget = RequestPath(seeker.Transform, endNode);
				seeker.SetNewTargetAndPath(endNode, pathToTarget);
			}
		}
	}

	[ContextMenu("Request Path")]
	private Queue<Node> RequestPath(Transform seekerTransform, Node endNode)
	{
		Node startNode = grid.GetNodeClosestToPosition(seekerTransform.position);

		List<Node> path = PathProvider.Provide(grid, startNode, endNode);

		foreach(var node in path)
		{
			SetColorForNode(node, Color.green);
		}

		Queue<Node> pathToTarget = new Queue<Node>(path.Count);

		for(int i = path.Count - 1; i >= 0; i--)
		{
			pathToTarget.Enqueue(path[i]);
		}

		return pathToTarget;
	}

	private void SetColorForNode(Node node, Color color)
	{
		//node.Material.color = color;
	}

	private void ClearColorsForNodes(IReadOnlyDictionary<int, Node> nodesInGrid)
	{
		//foreach(var Node in nodesInGrid.Values)
		//{
		//	Node.Material.color = Color.white;
		//}
	}
}


