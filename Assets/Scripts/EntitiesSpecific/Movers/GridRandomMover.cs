using UnityEngine;

public class GridRandomMover : GridMover
{
	public GridRandomMover(Transform transform, float speed) : base(transform, speed) { }

	public override void Update()
	{
		delay -= Time.deltaTime;

		if(delay > 0)
		{
			return;
		}

		Vector2Int direction = new Vector2Int(Random.Range(-1,2), Random.Range(-1,2));

		if(direction == Vector2Int.zero)
		{
			return;
		}

		Node neighbourNode = node.GetNeighbour(direction);

		if(neighbourNode == null || neighbourNode.State != NodeState.Walkable)
		{
			return;
		}

		Move(neighbourNode);
	}
}
