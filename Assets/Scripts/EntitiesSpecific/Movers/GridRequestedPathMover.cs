using System.Collections.Generic;
using UnityEngine;

public class GridRequestedPathMover : GridMover
{
	private Node targetNode;
	private Queue<Node> pathToTarget = new Queue<Node>();

	public GridRequestedPathMover(Transform transform, float speed) : base(transform, speed)
	{
		PathRequester.Me.Register(this);
	}

	public override void Destroy()
	{
		base.Destroy();
		if(PathRequester.Me != null)
		{
			PathRequester.Me.Unregister(this);
		}
	}

	public override void Update()
	{
		delay -= Time.deltaTime;

		if(delay > 0)
		{
			return;
		}

		if(targetNode == null)
		{
			return;
		}

		if(pathToTarget.Count > 0)
		{
			Node nextNode = pathToTarget.Dequeue();
			Move(nextNode);
			if(nextNode == targetNode)
			{
				targetNode = null;
			}
		}
	}

	public void SetNewTargetAndPath(Node endNode, Queue<Node> pathToTarget)
	{
		targetNode = endNode;
		this.pathToTarget = pathToTarget;
	}
}
