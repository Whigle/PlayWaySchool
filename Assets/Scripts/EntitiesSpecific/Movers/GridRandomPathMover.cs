using System.Collections.Generic;
using UnityEngine;

public class GridRandomPathMover : GridMover
{
	private Node targetNode;
	private Queue<Node> pathToTarget = new Queue<Node>();

	public GridRandomPathMover(Transform transform, float speed) : base(transform, speed) { }

	public override void Update()
	{
		delay -= Time.deltaTime;

		if(delay > 0)
		{
			return;
		}

		if(targetNode == null)
		{
			Grid grid = Grid.Me;
			targetNode = grid.GetNodeWithCoordinates(new Vector2Int(Random.Range(0, grid.NodesInRow), Random.Range(0, grid.NodesInColumn)));
			var path = PathProvider.Provide(grid, node, targetNode);

			for(int i = path.Count - 1; i >= 0; i--)
			{
				pathToTarget.Enqueue(path[i]);
			}
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
}
