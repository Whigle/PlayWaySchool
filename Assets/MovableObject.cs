using UnityEngine;

public class MovableObject : MonoBehaviour
{
	public Vector3 Position => transform.position;

	private void Start()
	{
		Node occupiedNode = Grid.Me.GetNodeClosestToPosition(Position);

		occupiedNode.State = NodeState.Occupied;
	}
}
