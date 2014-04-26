using UnityEngine;
using System.Collections;

//Attach to background plane and make the background follow the player 

public class FakeScroll : MonoBehaviour
{
	public float speedMult = 1f;
	// Object initialisation
	void Start()
	{
	}

	// Update the object (once per frame)
	void LateUpdate()
	{
		Vector2 offset = new Vector2(transform.position.x, transform.position.y);
		renderer.material.mainTextureOffset = -offset * speedMult;
	}
}
