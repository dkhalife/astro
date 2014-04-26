using UnityEngine; 
using System.Collections; 
 
public class SFX : MonoBehaviour  
{ 
	public AudioClip meteorExplodeSound; // done
	public AudioClip missileExplodeSound; // done
	public AudioClip playerExplodeSound; //
	public AudioClip checkpointSound;
	public AudioClip healthUpSound;
	public AudioClip healthDownSound; //

	public void meteorExplode(){
		Camera.main.audio.PlayOneShot(meteorExplodeSound);
	}

	public void missileExplode(){
		Camera.main.audio.PlayOneShot(missileExplodeSound);
	}

	public void playerExplode(){
		Camera.main.audio.PlayOneShot(playerExplodeSound);
	}

	public void checkpoint(){
		Camera.main.audio.PlayOneShot(checkpointSound);
	}

	public void healthUp(){
		Camera.main.audio.PlayOneShot(healthUpSound);
	}

	public void healthDown(){
		Camera.main.audio.PlayOneShot(healthDownSound);
	}
} 
 
