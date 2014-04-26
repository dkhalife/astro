using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour
{
	public Vector3 rotationIncrement;
	// Object initialisation
	void Start()
	{
	}

	// Update the object (once per frame)
	void Update()
	{
		transform.rotation *= Quaternion.Euler(rotationIncrement*Time.deltaTime);
	}
}
