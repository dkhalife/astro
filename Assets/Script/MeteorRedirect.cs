using UnityEngine; 
using System.Collections; 
 
public class MeteorRedirect : MonoBehaviour  
{ 
	public float maxDistance = 20f;
	public float targetRadius = 10f;

	void setDirection(Vector2 direction){
		float speed = rigidbody2D.velocity.magnitude;
		rigidbody2D.velocity = direction.normalized * speed;
	}
 
	// Update the object (once per frame) 
	void Update ()  
	{ 
		if (!renderer.isVisible) {
			float targetx = Random.Range(-targetRadius, targetRadius);
			float targety = Random.Range(-targetRadius, targetRadius);
			Vector2 pos = new Vector2(transform.position.x, transform.position.y);
			setDirection(new Vector2(targetx, targety) - pos);
		}
	} 
} 