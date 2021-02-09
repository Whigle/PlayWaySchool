using UnityEngine;

public class Target : MonoBehaviour, IRadius
{
	[SerializeField]
	private Player player;
	[SerializeField]
	private float radius;
	[SerializeField]
	private float speed;
	[SerializeField]
	private float viewDistance;
	[SerializeField]
	[Range(0.0001f, 360f)]
	private float fov = 60f;

	public float Radius => radius;

	private Vector3 targetPosition;
	private float halfFovInRads;

	public void Start()
	{
		halfFovInRads = fov * 0.5f * Mathf.Deg2Rad;
		SetNextTarget();
	}

	public void Update()
	{
		float distance = Vector3.Distance(targetPosition, transform.position);
		
		if(SeePlayer())
		{
			SetNextTarget(player.transform.position);
		}

		if(PlayerInRange())
		{
			return;
		}

		if(distance < 0.1f)
		{
			SetNextTarget();

			return;
		}

		Vector3 moveVector = (targetPosition - transform.position).normalized * speed * Time.deltaTime;

		if(Vector3.Magnitude(moveVector) > distance)
		{
			transform.position = targetPosition;

			return;
		}

		transform.position += moveVector;
	}

	private bool PlayerInRange()
	{
		return Radius + player.Radius > Vector3.Distance(transform.position, player.transform.position);
	}

	private bool SeePlayer()
	{
		//is in fov
		var vecForward = transform.forward;
		var vecToEnemy = player.transform.position - transform.position;

		if(Mathf.Acos(Vector3.Dot(vecForward.normalized, vecToEnemy.normalized)) > halfFovInRads)
		{
			return false;
		}

		//is in distance
		if(viewDistance < Vector3.Distance(player.transform.position, transform.position))
		{
			return false;
		}

		return true;
	}

	private void SetNextTarget(Vector3 position)
	{
		targetPosition = position;
		LookAtTarget();
	}

	private void SetNextTarget()
	{
		float x = UnityEngine.Random.Range(-10f, 10f);
		float z = UnityEngine.Random.Range(-10f, 10f);
		SetNextTarget(new Vector3(x, 0f, z));
	}

	private void LookAtTarget()
	{
		transform.rotation = Quaternion.LookRotation(targetPosition - transform.position, Vector3.up);
	}
}
