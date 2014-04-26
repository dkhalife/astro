using UnityEngine; 
using System.Collections; 
 
public class DestroyInvisible : MonoBehaviour  
{ 
	// Object initialisation 
	void Start ()  
	{ 
	} 
 
	// Update the object (once per frame) 
	void Update ()  
	{ 
	} 

	void OnBecameInvisible() {
		GameObject.Destroy (gameObject);
	}
} 
 
