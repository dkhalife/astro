using UnityEngine;
using System.Collections;

public class HardFollow2d : MonoBehaviour
{
	public Transform target;
	public bool copyRotation = false;

	// Update the object (once per frame)
	void LateUpdate()
	{
		if (target == null){
			//Debug.LogWarning("target null");
			return;
		}

		transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
		if (copyRotation)
			transform.rotation = target.rotation;
	}
}
