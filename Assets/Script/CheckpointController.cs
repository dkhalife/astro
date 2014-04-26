using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpawnChildOnStart))]
public class CheckpointController : MonoBehaviour
{
	public bool alive = true;
	public float hSepCheckpoint = 60f;
	public float vSepCheckpoint = 10f;
	private SpawnChildOnStart spawner;
	private Animator anim;
	// Object initialisation
	void Start()
	{
		spawner = GetComponent<SpawnChildOnStart>();
		anim = GetComponent<Animator>();
	}

	public void die()
	{
		GameObject.Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") && alive)
		{
			Vector3 newPos = transform.position + new Vector3 (hSepCheckpoint, Random.Range (-vSepCheckpoint, vSepCheckpoint), 0f);
			GameObject.Instantiate (gameObject, newPos, Quaternion.identity);
			//Fake collision with power up
			spawner.child.SendMessage("OnTriggerEnter2D", other);
			//GameObject.Destroy(gameObject);
			alive = false;
			anim.SetBool("Alive", alive);
		}
	}
}
