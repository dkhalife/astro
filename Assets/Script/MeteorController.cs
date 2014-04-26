using UnityEngine; 
using System.Collections; 
 
public class MeteorController : MonoBehaviour  
{ 
	public GameObject[] children;
	private int pieces = 2;
	public LifeOverlay lifeObj;
	public GameObject deathPrefab;
	public float maxIniRotation = 40f;
	SFX sfx;

	public void Start(){
		sfx = (SFX)GameObject.FindGameObjectWithTag ("Sound").GetComponent ("SFX");
		rigidbody2D.angularVelocity = maxIniRotation;// Random.Range(-maxIniRotation, maxIniRotation);
	}

	public void Hurt(Vector2 impactVelocity)
	{
		if (lifeObj == null)
			return;
		
		lifeObj.life -= 1;
		
		if (lifeObj.life < 0) {
			sfx.missileExplode ();
			Explode (impactVelocity);
		}
		else 
			sfx.meteorExplode ();
	}

	public void Explode(Vector2 impactVelocity){
		if (children.Length > 0) {

			for(int i=0; i<pieces; ++i){
				int prefabIndex = Random.Range (0, children.Length);
				float scale = transform.localScale.x /2; //trops
				Vector3 delta = new Vector3(Random.Range(0.2f, 1f), Random.Range(0.2f, 1f)).normalized*scale;
				GameObject meteor = (GameObject) GameObject.Instantiate(children[prefabIndex], transform.position + delta, transform.rotation);
				meteor.transform.parent = transform.parent;
				Rigidbody2D rb = meteor.GetComponent<Rigidbody2D>();

				rb.velocity = impactVelocity / scale; //pas assez
			}
		}

		GameObject.Instantiate(deathPrefab, transform.position, transform.rotation);
		GameObject.Destroy (gameObject);
	}
} 
 
