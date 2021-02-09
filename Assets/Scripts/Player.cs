using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	private const float halfPi = Mathf.PI * 0.5f;

	[SerializeField]
	private Target target;
	[SerializeField]
	private Text theteText;
	[SerializeField]
	private Text leftRightText;
	[SerializeField]
	private Text collidedText;

	[SerializeField]
	private float radius;

	public float Radius => radius;

	private void Update()
	{
		if(target == null)
		{
			return;
		}

		var vecForward = transform.forward;
		var vecToEnemy = target.transform.position - transform.position;

		theteText.text = (Mathf.Acos(Vector3.Dot(vecForward.normalized, vecToEnemy.normalized)) * Mathf.Rad2Deg).ToString();

		var vecRight = transform.right;

		string text = (Mathf.Acos(Vector3.Dot(vecRight.normalized, vecToEnemy.normalized))) <= halfPi ? "right " : "left ";

		text += (Mathf.Acos(Vector3.Dot(vecForward.normalized, vecToEnemy.normalized))) <= halfPi ? "forward" : "back";

		leftRightText.text = text;

		collidedText.text = (Radius + target.Radius) > Vector3.Distance(transform.position, target.transform.position) ? "collision" : "no collision";
	}
}

public interface IRadius
{
	float Radius { get; }
}
