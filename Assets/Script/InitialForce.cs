using UnityEngine;
using System.Collections;

public class InitialForce : MonoBehaviour
{
	public Vector2 force = new Vector2(0f, 0f);
	public float multiplier = 1f;
	// Object initialisation
	void Start()
	{
		rigidbody2D.AddForce(force * multiplier);
	}

	void Update()
	{
		//rigidbody2D.AddForce(force);
	}

	void OnDrawGizmosSelected () {
		// Display the explosion radius when selected
		Gizmos.color = Color.white;
		Gizmos.DrawLine(transform.position, transform.position + new Vector3(force.x, force.y) );
	}
}
