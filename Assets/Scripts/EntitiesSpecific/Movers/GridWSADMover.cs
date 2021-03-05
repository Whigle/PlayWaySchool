using UnityEngine;

public class GridWSADMover : GridMover
{
	public GridWSADMover(Transform transform, float speed) : base(transform, speed) { }

	public override void Update()
	{
		delay -= Time.deltaTime;

		if(delay > 0)
		{
			return;
		}

		if(Input.anyKey)
		{
			Vector2Int direction = new Vector2Int();

			if(Input.GetKey(KeyCode.W))
			{
				direction += Vector2Int.up;
			}
			if(Input.GetKey(KeyCode.S))
			{
				direction += Vector2Int.down;
			}
			if(Input.GetKey(KeyCode.A))
			{
				direction += Vector2Int.left;
			}
			if(Input.GetKey(KeyCode.D))
			{
				direction += Vector2Int.right;
			}

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
}
