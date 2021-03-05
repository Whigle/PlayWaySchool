using UnityEngine;

public class MovableObject : MonoBehaviour
{
	[SerializeField]
	private bool controlledByPlayer;
	[SerializeField]
	private float speed = 1f;

	private Node occupiedNode;
	private GridMover gridMover;
	private Grid grid;

	public Vector3 Position => transform.position;

	private void Awake()
	{
		grid = Grid.Me;

		if(grid.IsCreated)
		{
			Grid_GridCreated(grid);
		}

		grid.GridCreated += Grid_GridCreated;
		grid.Destroyed += Grid_Destroyed;
	}

	private void Grid_Destroyed(Grid grid)
	{
		grid.GridCreated -= Grid_GridCreated;
		grid.Destroyed -= Grid_Destroyed;
	}

	private void Update()
	{
		if(gridMover != null)
		{
			gridMover.Update();
		}
	}

	private void OnDestroy()
	{
		if(grid != null)
		{
			grid.GridCreated -= Grid_GridCreated;
			grid.Destroyed -= Grid_Destroyed;
		}

		gridMover.Destroy();
	}

	private void Grid_GridCreated(Grid grid)
	{
		if(controlledByPlayer)
		{
			gridMover = new GridWSADMover(this.transform, speed);
		}
		else
		{
			gridMover = new GridRequestedPathMover(this.transform, speed);
		}
	}
}
