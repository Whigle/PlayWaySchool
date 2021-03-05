using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grid : MonoBehaviour, ISerializationCallbackReceiver
{
	public event Action<Grid> GridCreated;
	public event Action<Grid> Destroyed;

	[SerializeField]
	private BoxCollider gridVolume;
	[SerializeField]
	[Range(1, 100)]
	private int nodesInRow = 20;
	[SerializeField]
	[Range(1, 100)]
	private int nodesInColumn = 20;

	[SerializeField]
	[HideInInspector]
	private List<int> nodesInGridKeysSerialized;
	[HideInInspector]
	[SerializeField]
	private List<Node> nodesInGridValuesSerialized;

	[SerializeField]
	private Node nodePrefab;

	#region Helper Readonly Fields
	[SerializeField]
	[ReadOnly]
	private float nodeXSize;
	[SerializeField]
	[ReadOnly]
	private float nodeZSize;
	[SerializeField]
	[ReadOnly]
	private float nodeXHalfSize;
	[SerializeField]
	[ReadOnly]
	private float nodeZHalfSize;
	[SerializeField]
	[ReadOnly]
	private Vector3 gridVolumeCenter;
	[SerializeField]
	[ReadOnly]
	private Vector3 gridVolumePositionZero;
	#endregion Helper Readonly Fields

	private Dictionary<int, Node> nodesInGrid = new Dictionary<int, Node>();

	public IReadOnlyDictionary<int, Node> NodesInGrid => nodesInGrid;
	public float NodeXSize => nodeXSize;
	public float NodeZSize => nodeZSize;
	public int NodesInRow => nodesInRow;
	public int NodesInColumn => nodesInColumn;

	private static Grid me;
	public static Grid Me
	{
		get
		{
			if(me == null)
			{
				me = FindObjectOfType<Grid>();
			}

			return me;
		}
	}

	public bool IsCreated { get; private set; }

	private void Awake()
	{
		me = this;
		IsCreated = false;
		CreateGrid();
	}

	private void OnDestroy()
	{
		Destroyed?.Invoke(this);
	}

	public Node GetNodeWithCoordinates(Vector2Int coordinates)
	{
		Vector2Int ValidatedCoordinates = new Vector2Int(Mathf.Clamp(coordinates.x, 0, NodesInRow), Mathf.Clamp(coordinates.y, 0, NodesInColumn));

		return nodesInGrid[ValidatedCoordinates.x * 100 + ValidatedCoordinates.y];
	}

	public Node GetNodeClosestToPosition(Vector3 position)
	{
		if(nodesInGrid == null || nodesInGrid.Count == 0)
		{
			return null;
		}

		return nodesInGrid.Values.OrderBy(node => Vector3.Distance(position, node.transform.position)).First();
	}

	public Node GetFreeNodeClosestToPosition(Vector3 position)
	{
		if(nodesInGrid == null || nodesInGrid.Count == 0)
		{
			return null;
		}

		return nodesInGrid.Values.Where(node => node.State == NodeState.Walkable).OrderBy(node => Vector3.Distance(position, node.transform.position)).First();
	}

	[ContextMenu("Create Grid")]
	private void CreateGrid()
	{
		if(nodesInGrid != null || nodesInGrid.Count > 0)
		{
			ClearOldGrid();
		}

		nodesInGrid = new Dictionary<int, Node>(nodesInRow * nodesInColumn);
		CreateScaleAndPositionNodes();

		for(int x = 0; x < nodesInRow; x++)
		{
			for(int z = 0; z < nodesInColumn; z++)
			{
				List<Node> neighbours = new List<Node>();

				for(int k = Mathf.Max(0, x - 1); k <= Mathf.Min(x + 1, nodesInRow - 1); k++)
				{
					for(int l = Mathf.Max(0, z - 1); l <= Mathf.Min(z + 1, nodesInColumn - 1); l++)
					{
						if(k != x || z != l)
						{
							neighbours.Add(nodesInGrid[k * 100 + l]);
						}
					}
				}

				nodesInGrid[x * 100 + z].SetNeighbours(neighbours);
			}
		}

		IsCreated = true;
		GridCreated?.Invoke(this);
	}

	private void CreateScaleAndPositionNodes()
	{
		//prepare const values
		SetConstValues();

		for(int x = 0; x < nodesInRow; x++)
		{
			for(int z = 0; z < nodesInColumn; z++)
			{
				Node node = Instantiate(nodePrefab, GetPositionForCoordinates(x, z), Quaternion.identity, this.transform).GetComponent<Node>();
				node.transform.localScale = GetScaleForNode();
				node.X = x;
				node.Z = z;
				nodesInGrid.Add(x * 100 + z, node);
			}
		}
	}

	private void SetConstValues()
	{
		nodeXSize = gridVolume.size.x / (float)nodesInRow;
		nodeXHalfSize = nodeXSize * 0.5f;
		nodeZSize = gridVolume.size.z / (float)nodesInColumn;
		nodeZHalfSize = nodeZSize * 0.5f;
		gridVolumeCenter = gridVolume.center;
		gridVolumePositionZero = gridVolume.center - gridVolume.size / 2f;
	}

	private Vector3 GetScaleForNode()
	{
		return new Vector3(nodeXSize, 1f, nodeZSize);
	}

	private Vector3 GetPositionForCoordinates(int x, int z)
	{
		return gridVolumePositionZero + new Vector3(nodeXHalfSize, 0f, nodeZHalfSize) + Vector3.right * x * nodeXSize + Vector3.forward * z * nodeZSize;
	}

	private void ClearOldGrid()
	{
		foreach(var nodeInGrid in nodesInGrid)
		{
			if(nodeInGrid.Value != null)
			{
				DestroyImmediate(nodeInGrid.Value.gameObject);
			}
		}

		nodesInGrid.Clear();
	}

	public void OnBeforeSerialize()
	{
		nodesInGridKeysSerialized = nodesInGrid.Keys.ToList();
		nodesInGridValuesSerialized = nodesInGrid.Values.ToList();
	}

	public void OnAfterDeserialize()
	{
		if(nodesInGrid != null && nodesInGrid.Count > 0)
		{
			return;
		}

		for(int i = 0; i < nodesInGridKeysSerialized.Count(); i++)
		{
			nodesInGrid.Add(nodesInGridKeysSerialized[i], nodesInGridValuesSerialized[i]);
		}
	}
}
