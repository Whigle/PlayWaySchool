using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSADMover : MonoBehaviour
{

	private Transform selfTransform;

	private void Awake()
	{
		selfTransform = GetComponent<Transform>();
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.W))
		{
			transform.position += Vector3.forward;
		}
		else if(Input.GetKeyDown(KeyCode.S))
		{
			transform.position -= Vector3.forward;
		}
		else if(Input.GetKeyDown(KeyCode.A))
		{
			transform.position -= Vector3.right;
		}
		else if(Input.GetKeyDown(KeyCode.D))
		{
			transform.position += Vector3.right;
		}
	}
}
