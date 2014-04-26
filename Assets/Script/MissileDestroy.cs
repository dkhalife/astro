using UnityEngine; 
using System.Collections; 
 
public class MissileDestroy : MonoBehaviour  
{ 
	void OnTriggerEnter2D(Collider2D collider){
		rigidbody2D.velocity = Vector2.zero;
		Destroy (gameObject);

		MeteorController md = collider.gameObject.GetComponent<MeteorController> ();
		if (md != null) {
			md.Hurt(rigidbody2D.velocity);
		}
	}
} 
 
