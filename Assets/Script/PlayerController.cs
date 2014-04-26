using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
	public float thrusterForce = 100f;
	public float desktopMouseForceFactor = 0.2f;
	public float desktopKeyboardForceFactor = 4f;
	public float rotationSpeed = 100f;
	public GunController weapon = null;
	public LifeOverlay lifeObj;
	public Score scoreCmp;
	// Horizontal Separation
	SFX sfx;
	Vector3 prevMousePos;
	// Object initialisation
	void Start()
	{
		rigidbody2D.velocity = transform.up;
		//Screen.lockCursor = true;
		sfx = (SFX)GameObject.FindGameObjectWithTag ("Sound").GetComponent ("SFX");
	}

	// Update the object (once per frame)
	void Update()
	{
		// Process touches
		for (int i=0; i < Input.touchCount; i++)
		{
			Touch touch = Input.GetTouch(i);
			rigidbody2D.AddForce(touch.deltaPosition*thrusterForce*Time.deltaTime);
		}

#if UNITY_STANDALONE || UNITY_EDITOR
		if (Input.GetMouseButton(0))
		{
			Vector2 deltapos = Input.mousePosition - prevMousePos;
			rigidbody2D.AddForce( deltapos*thrusterForce*Time.deltaTime*desktopMouseForceFactor);
		}
		else
		{
			float h = Input.GetAxis("Horizontal");
			float v = Input.GetAxis("Vertical");
			float mult = thrusterForce*Time.deltaTime*desktopKeyboardForceFactor;
			rigidbody2D.AddForce(new Vector2(0f, v*mult) );
			rigidbody2D.AddForce(new Vector2(h*mult, 0f) );
		}
#endif

		//Turn towards direction
		//if (Input.touchCount > 0)
			transform.up = rigidbody2D.velocity.normalized;

		if (weapon != null)// && ( Input.touchCount > 1 || Input.GetButton("Fire1") ) )
			weapon.shootGuns();

		prevMousePos = Input.mousePosition;
	}

	void OnDrawGizmosSelected () {
		// Display the explosion radius when selected
		Gizmos.color = Color.white;
		Gizmos.DrawLine(transform.position, transform.position + transform.up );
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.CompareTag ("Meteor")) {
			lifeObj.life -= 1;
			other.gameObject.GetComponent<MeteorController>().Explode(transform.up);

			if (lifeObj.life < 0){
				// Death
				sfx.playerExplode();

				//GameOver();
				Camera.main.GetComponent<GameOver>().GameOverStartEffects();
				Camera.main.GetComponent<GameOver>().Invoke("GameOverAfterEffects", 2);
				Destroy (gameObject);
			}
			else {
				sfx.healthDown();
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		GameObject go = other.gameObject;
		if (go.CompareTag("Checkpoint") && go.GetComponent<CheckpointController>().alive) {
			++scoreCmp.score;
			sfx.checkpoint();
		}
	}
}
