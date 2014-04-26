using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour
{
	// Object initialisation
	void Start()
	{
	}

	// Update the object (once per frame)
	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape))
			Application.Quit();
	}
}
