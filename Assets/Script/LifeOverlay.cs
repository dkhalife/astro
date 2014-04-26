using UnityEngine; 
using System.Collections; 
 
public class LifeOverlay : MonoBehaviour  
{ 
	public int life = 1;
	public Sprite [] overlays;
	// Object initialisation 
	void Start ()  
	{ 
		if (overlays[life] != null)
			GetComponent<SpriteRenderer>().sprite = overlays[life];
	} 
 
	// Update the object (once per frame) 
	void Update ()  
	{ 
		if (life >= 0 && life < overlays.Length)
			GetComponent<SpriteRenderer>().sprite = overlays[life];
	} 

} 