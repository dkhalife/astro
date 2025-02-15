﻿using UnityEngine;
using System.Collections;

public class MeteorGrab : MonoBehaviour
{
	public int maxGrabs = 1;
	public AudioClip grabSound;

	// Object initialisation
	void Start()
	{
	}

	// Update the object (once per frame)
	void Update()
	{
	}

	int getNumberGrabs()
	{
		GameObject[] meteors = GameObject.FindGameObjectsWithTag("Meteor");
		int grabs = 0;
		foreach(GameObject meteor in meteors)
		{
			SpringJoint2D sj = meteor.GetComponent<SpringJoint2D>();
			if (sj && sj.connectedBody == rigidbody2D )
				grabs++;
		}
		return grabs;
	}

	void OnCollisionEnter2D(Collision2D other) {
        if (getNumberGrabs() < maxGrabs && other.gameObject.CompareTag("Meteor"))
		{
			GameObject meteor = other.gameObject;
			SpringJoint2D sj = meteor.AddComponent<SpringJoint2D>();
			sj.connectedBody = rigidbody2D;
			sj.distance = 1.5f;

			// Reduce mass for easier dragging
			meteor.rigidbody2D.mass /= 10f;
			meteor.rigidbody2D.drag = 0.5f; // Add friction so the meteor doesn't spin all around

			audio.PlayOneShot(grabSound);
		}
    } 
}
