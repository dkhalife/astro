using UnityEngine;
using System.Collections;

public class AstroGenerator : MonoBehaviour {
	public GameObject[] meteorPrefabs;
	public int maxMeteors = 10;
	public float minRadius = 5;
	public float maxRadius = 10;
	public float minVelocity = 1;
	public float maxVelocity = 3;
	public float minTorque = 1;
	public float maxTorque = 7;

	// Use this for initialization
	void Start () {
		Random.seed = (int) Time.time;
	}

	// Update is called once per frame
	void Update () {
		if (GameObject.FindGameObjectsWithTag("Meteor").Length < maxMeteors) {
			createRandomMeteor();
		}
	}

	void createRandomMeteor(){
		float angle = Random.value * Mathf.PI * 2;
		float radius = Random.Range (minRadius, maxRadius);

		int prefabIndex = Random.Range(0, meteorPrefabs.Length);
		Vector3 pos = new Vector3 (radius * Mathf.Cos (angle), radius * Mathf.Sin (angle), 0);
		pos += GameObject.FindGameObjectWithTag ("Player").transform.position;
		GameObject meteor = (GameObject) Instantiate(meteorPrefabs[prefabIndex], pos, Quaternion.identity);
		meteor.transform.parent = transform;
	
		float torque = Random.Range(minTorque, maxTorque);
		meteor.rigidbody2D.AddTorque(torque);
		
		float velocity = Random.Range(minVelocity, maxVelocity);
		Vector2 direction = new Vector2(Random.Range (-1,2), Random.Range (-1,2));
		meteor.rigidbody2D.velocity = direction * velocity;
	}
}
