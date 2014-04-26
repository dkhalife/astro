using UnityEngine;
using System.Collections;

public class SpawnChildOnStart : MonoBehaviour
{
	public Transform spawnPoint;
	public GameObject [] possibleChilds;
	//Read only child
	public GameObject child {get{ return _child;}}
	private GameObject _child;
	// Object initialisation
	void Start()
	{
		int prefabIndex = Random.Range(0, possibleChilds.Length);
		_child = GameObject.Instantiate(possibleChilds[prefabIndex], spawnPoint.position, Quaternion.identity) as GameObject;
	}
}
