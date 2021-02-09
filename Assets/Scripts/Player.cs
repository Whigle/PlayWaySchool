using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	[SerializeField]
	private Transform targetTransform;
	[SerializeField]
	private Text theteText;
	[SerializeField]
	private Text leftRightText;

	private void Update()
	{
		var vecForward = transform.forward;
		var vecToEnemy = targetTransform.position - transform.position;

		theteText.text = (Mathf.Acos(Vector3.Dot(vecForward.normalized, vecToEnemy.normalized)) * Mathf.Rad2Deg).ToString();

		var vecRight = transform.right;

		leftRightText.text = (Mathf.Acos(Vector3.Dot(vecRight.normalized, vecToEnemy.normalized)) * Mathf.Rad2Deg) <= 90 ? "right" : "left";


	}
}
