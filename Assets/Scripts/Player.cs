using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	[SerializeField]
	private Transform targetTransform;
	[SerializeField]
	private Text theteText;

	private void Update()
	{
		var vecForward = transform.forward;
		var vecToEnemy = targetTransform.position - transform.position;

		theteText.text = (Mathf.Acos(Vector3.Dot(vecForward.normalized, vecToEnemy.normalized)) * Mathf.Rad2Deg).ToString();
	}
}
