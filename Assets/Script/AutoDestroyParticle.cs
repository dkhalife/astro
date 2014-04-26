using UnityEngine;
using System.Collections;

public class AutoDestroyParticle : MonoBehaviour
{
	ParticleSystem ps;

	// Object initialisation
	void Start()
	{
		ps = GetComponent<ParticleSystem>();
	}

	// Update the object (once per frame)
	void Update()
	{
		if(ps && !ps.IsAlive())
		{
			GameObject.Destroy(gameObject);
		}
	}
}
