using UnityEngine;

public abstract class GridMover
{
	protected Transform transform;
	protected Node node;
	protected float delay = 0f;
	protected float reversedSpeed = 1f;

	public Transform Transform => transform;
	public Node Node => node;
	
	public GridMover(Transform transform, float speed)
	{
		this.transform = transform;
		node = Grid.Me.GetFreeNodeClosestToPosition(transform.position);

		if(speed != 0)
		{
			reversedSpeed = 1f / speed;
		}

		Move(node);
	}

	public abstract void Update();
	
	public virtual void Destroy()
	{

	}

	public void Move(Node neighbourNode)
	{
		if(node != null)
		{
			node.State = NodeState.Walkable;
		}
		delay = Vector3.Distance(node.transform.position, neighbourNode.transform.position) * reversedSpeed;
		node = neighbourNode;
		transform.position = node.transform.position + Vector3.up * 0.5f;
		node.State = NodeState.Occupied;
	}
}
