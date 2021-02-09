using UnityEngine;

public class Target : MonoBehaviour, IRadius
{
	[SerializeField]
	private float radius;

	public float Radius => radius;
}
