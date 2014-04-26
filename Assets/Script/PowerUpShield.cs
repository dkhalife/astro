using UnityEngine; 
using System.Collections; 
 
public class PowerUpShield : MonoBehaviour  
{ 
	void OnTriggerEnter2D(Collider2D other) 
	{
		//Debug.Log(other.name);
		if (other.gameObject.CompareTag("Player"))
		{
			//Destroy previous weapon if any
			PlayerController pc = other.GetComponent<PlayerController>();
			pc.lifeObj.life += 1;
			GameObject.Destroy(gameObject);
		}
	}
} 
 
