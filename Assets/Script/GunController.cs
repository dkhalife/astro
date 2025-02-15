﻿using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour
{
	public float shootRate = 0.1f;
	Gun[] guns;
	float nextShot = 0f;
	public float lifetime = 15;
	public AudioClip shotSound;
	SFX sfx;
	float deathtime;

	// Object initialisation
	void Start()
	{
		deathtime = Time.time + lifetime;
		guns = GetComponentsInChildren<Gun>();
		sfx = (SFX) GameObject.FindGameObjectWithTag("Sound").GetComponent("SFX");
	}

	void Update()
	{
		if (Time.time > deathtime)
			GameObject.Destroy(gameObject);
	}

	// Update the object (once per frame)
	public void shootGuns()
	{
		if (Time.time >= nextShot)
		{
			foreach(Gun g in guns)
				g.shoot();

			Camera.main.audio.PlayOneShot(shotSound);

			nextShot = Time.time + shootRate; 
		}
	}
}
