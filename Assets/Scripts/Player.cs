using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IRadius
{
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
		var vecForward = transform.forward;
		var vecToEnemy = target.transform.position - transform.position;

		theteText.text = (Mathf.Acos(Vector3.Dot(vecForward.normalized, vecToEnemy.normalized))).ToString();

		var vecRight = transform.right;

		string text = (Mathf.Acos(Vector3.Dot(vecRight.normalized, vecToEnemy.normalized))) <= Mathf.PI / 2 ? "right " : "left ";

		text += (Mathf.Acos(Vector3.Dot(vecForward.normalized, vecToEnemy.normalized))) <= Mathf.PI / 2 ? "forward" : "back";

		leftRightText.text = text;

		collidedText.text = (Radius + target.Radius) > Vector3.Distance(transform.position, target.transform.position) ? "collision" : "no collision";
	}
}

public interface IRadius
{
	float Radius { get; }
}
