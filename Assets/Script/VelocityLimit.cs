using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class VelocityLimit : MonoBehaviour
{
	public float maxSpeed = 15f;

	void LateUpdate()
	{
		if (rigidbody2D.velocity.magnitude > maxSpeed)
			rigidbody2D.velocity = rigidbody2D.velocity.normalized * maxSpeed;
	}
}
