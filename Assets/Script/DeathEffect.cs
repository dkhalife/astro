using UnityEngine;
using System.Collections;

public class DeathEffect : MonoBehaviour
{
	public Sprite [] parts;
	public GameObject PartPrefab;
	public float velocity = 6f;
	// Object initialisation
	void Start()
	{
		GameObject papa = new GameObject("DeathParts");
		foreach(Sprite pSprite in parts)
		{
			GameObject part = GameObject.Instantiate(PartPrefab, transform.position, Quaternion.identity) as GameObject;
			part.GetComponent<SpriteRenderer>().sprite = pSprite;
			float angle = Random.Range(0, 2f*Mathf.PI);
			part.rigidbody2D.velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle))*velocity*Random.Range(0.5f, 1f);
			part.rigidbody2D.angularVelocity = 5f*velocity;
			part.rigidbody2D.AddTorque(5f*velocity);
			part.transform.parent = papa.transform;
		}
	}

	// Update the object (once per frame)
	void Update()
	{
	}
}
