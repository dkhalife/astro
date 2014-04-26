using UnityEngine; 
using System.Collections; 
 
public class MeteorGenerator : MonoBehaviour  
{ 
	public GameObject [] prefabMeteors;
	public float meteorSpeed = 5;

	public GameObject player;
	public float maxDistance = 10;
	public int minMeteors = 5;
	public int newMeteorsPerLevel = 2;

	void OnDrawGizmosSelected () {
		// Display the explosion radius when selected
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(player.transform.position, maxDistance);
	}
 
	// Update the object (once per frame) 
	void Update ()  
	{ 
		if (player == null)
			return;

		GameObject[] meteors = GameObject.FindGameObjectsWithTag("Meteor");
		int totalMeteors = meteors.Length;
		Vector3 playerPos = player.transform.position;
		
		// Destroy far meteors
		foreach(GameObject meteor in meteors){
			if(!meteor.renderer.isVisible &&
			   (playerPos - meteor.transform.position).magnitude >= maxDistance){
				GameObject.Destroy(meteor.gameObject);
				--totalMeteors;
			}
		}

		//float angle = Mathf.Atan2 (transform.up.y, transform.up.x);
		float minX;
		if (player.rigidbody2D.velocity.x >= 0) {
			minX = player.transform.position.x + maxDistance / 2;
			
		} else {
			minX = player.transform.position.x - maxDistance;
		}

		float playerY = player.transform.position.y;
		float minY = playerY - maxDistance;

		GameObject scoreComp = (GameObject)GameObject.FindGameObjectWithTag ("Score");
		int score = ((Score)scoreComp.GetComponent ("Score")).score;
		int maxSize = Mathf.Max(1, Mathf.Min (prefabMeteors.Length, score/2));
		int nbMeteors = minMeteors + newMeteorsPerLevel * score;

		// Create if not enough meteors
		for(int i=totalMeteors; i<nbMeteors; ++i){
			int prefabIndex = Random.Range (0, maxSize);
			//float theta = Random.Range (angle - Mathf.PI / 4, angle + Mathf.PI / 4);
			//float r = Random.Range (0, maxDistance);
			//Vector3 position = new Vector3(r * Mathf.Cos(theta), r * Mathf.Sin(theta), 0);
			float x = Random.Range (minX, minX + maxDistance / 2);
			float y = Random.Range (minY, minY + 2 * maxDistance);

			Vector3 position = new Vector3(x, y, 0);

			GameObject meteor = (GameObject) GameObject.Instantiate(prefabMeteors[prefabIndex], position, Quaternion.identity);
			Vector3 direction = new Vector3(Random.Range (-1.0f, 1.0f), y > playerY ? -1.0f : 1.0f, 0).normalized;
			meteor.rigidbody2D.velocity = direction * Random.Range (1, meteorSpeed);

			meteor.transform.parent = transform;
		}
	} 
} 
 
