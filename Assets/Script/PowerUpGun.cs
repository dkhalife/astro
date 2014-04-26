using UnityEngine;
using System.Collections;

public class PowerUpGun : MonoBehaviour
{
	public GameObject gunPrefab;

	void OnTriggerEnter2D(Collider2D other) 
	{
		//Debug.Log(other.name);
		if (other.gameObject.CompareTag("Player"))
		{
			//Destroy previous weapon if any
			PlayerController pc = other.GetComponent<PlayerController>();
			if (pc.weapon)
				GameObject.Destroy(pc.weapon.gameObject);

			// Offset position to place guns under spaceship
			Vector3 pos = other.transform.position + new Vector3(0,0,1);
			GameObject gun = GameObject.Instantiate(gunPrefab, pos, other.transform.rotation) as GameObject;
			// Set new weapon
			pc.weapon = gun.GetComponent<GunController>();
			gun.transform.parent = other.transform;
			GameObject.Destroy(gameObject);
		}
	}
}
