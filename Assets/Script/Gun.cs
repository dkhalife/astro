using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
	public GameObject laserPrefab = null;
	public float velocity = 1f;

	public void shoot(){
		if (laserPrefab)
		{
			Vector3 pos = transform.position;
			pos.z = 1.1f;
			GameObject laser = (GameObject) GameObject.Instantiate(laserPrefab, pos, transform.rotation);
			Rigidbody2D rb = laser.GetComponent<Rigidbody2D>();
			laser.transform.up = transform.parent.up;
			rb.velocity = rb.transform.up*velocity;

			laser.transform.parent = GameObject.FindGameObjectWithTag("LaserRoot").transform;
		}
	}
}
