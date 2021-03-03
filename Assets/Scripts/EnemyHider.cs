using UnityEngine;

public class EnemyHider : MonoBehaviour
{
	//[SerializeField]
	//private int escapeRange = 5;
	//[SerializeField]
	//private Grid grid;
	//[SerializeField]
	//private Transform playerTransform;
	//[SerializeField]
	//private Vector3 positionPlacementOffset = new Vector3(0, 0.5f, 0);
	//[SerializeField]
	//private List<Transform> enemyTransforms = new List<Transform>();
	//[SerializeField]
	//private float evaluationTimeInSec = 1f;

	//private Coroutine evaluationCoroutine;
	//private int layerMask;

	//private void Awake()
	//{
	//	if(playerTransform == null || enemyTransforms == null || enemyTransforms.Count == 0)
	//	{
	//		UnityEngine.Debug.LogError("[EnemyHider.Awake] Player transform or enemy transforms are not set properly. Aborted");
	//		return;
	//	}

	//	layerMask = LayerMask.NameToLayer("Obstacles");
	//	evaluationCoroutine = StartCoroutine(NewCoroutine());
	//}

	//private Vector3 direction;
	//private Ray ray;
	//private float distance;

	//private IEnumerator NewCoroutine()
	//{
	//	while(true)
	//	{
	//		foreach(var enemyTransform in enemyTransforms)
	//		{
	//			distance = Vector3.Distance(enemyTransform.position, playerTransform.position);
	//			direction = (enemyTransform.position - playerTransform.position).normalized;
	//			ray = new Ray(playerTransform.position, direction);

	//			if(!Physics.Raycast(ray, out RaycastHit hit, distance))
	//			{
	//				MoveEnemyToHidePosition(enemyTransform);
	//			}
	//		}

	//		yield return new WaitForSeconds(evaluationTimeInSec);
	//	}
	//}

	//private void MoveEnemyToHidePosition(Transform enemyTransform)
	//{
	//	Node playerNode = grid.GetNodeClosestToPosition(playerTransform.position);
	//	Node enemyNode = grid.GetNodeClosestToPosition(enemyTransform.position);

	//	Node stepNode = grid.CalculatePathToSpecialNode();
	//	enemyTransform.position = stepNode.Center + positionPlacementOffset;
	//}

	//private void OnDestroy()
	//{
	//	StopCoroutine(evaluationCoroutine);
	//	evaluationCoroutine = null;
	//}

	//private void OnDrawGizmos()
	//{
	//	Gizmos.DrawLine(playerTransform.position, playerTransform.position + direction * distance);
	//}
}
