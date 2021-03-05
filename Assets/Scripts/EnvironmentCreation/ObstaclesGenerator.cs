using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstaclesGenerator : MonoBehaviour
{
	[SerializeField]
	private Grid grid;
	[SerializeField]
	private GameObject obstaclePrefab;
	[SerializeField]
	private int minObstaclesCount;
	[SerializeField]
	private int maxObstaclesCount;

	private List<GameObject> spawnedObstacles = new List<GameObject>();

	private void Awake()
	{
		grid.GridCreated += Grid_GridCreated;
		grid.Destroyed += Grid_Destroyed;
	}

	private void Grid_Destroyed(Grid grid)
	{
		grid.GridCreated -= Grid_GridCreated;
		grid.Destroyed -= Grid_Destroyed;
	}

	private void OnDestroy()
	{
		grid.GridCreated -= Grid_GridCreated;
		grid.Destroyed -= Grid_Destroyed;
	}

	private void Grid_GridCreated(Grid grid)
	{
		minObstaclesCount = Mathf.Clamp(minObstaclesCount, 0, (int)(grid.NodesInColumn * grid.NodesInRow * 0.5f));
		maxObstaclesCount = Mathf.Clamp(maxObstaclesCount, 0, (int)(grid.NodesInColumn * grid.NodesInRow * 0.5f));
		int obstaclesCount = UnityEngine.Random.Range(minObstaclesCount, maxObstaclesCount + 1);

		ClearObstacles();
		SpawnObstacles(grid, obstaclesCount);
	}

	private void SpawnObstacles(Grid grid, int obstaclesCount)
	{
		while(obstaclesCount > 0)
		{
			var randomNode = grid.NodesInGrid.ElementAt(Random.Range(0, grid.NodesInGrid.Count())).Value;

			if(randomNode.State != NodeState.Walkable)
			{
				continue;
			}

			spawnedObstacles.Add(Instantiate(obstaclePrefab, randomNode.transform));
			randomNode.State = NodeState.Unwalkable;
			obstaclesCount--;
		}
	}

	private void ClearObstacles()
	{
		foreach(var spawnedObstacle in spawnedObstacles)
		{
			Destroy(spawnedObstacle);
		}

		spawnedObstacles.Clear();
	}
}
