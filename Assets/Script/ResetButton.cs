using UnityEngine; 
using System.Collections; 
 
public class ResetButton : MonoBehaviour  
{ 
	public GUIStyle style;
	Rect position =  new Rect(Screen.width / 2 - 70, Screen.height / 2 + 40,170f, 50f);

	void OnGUI()
	{
		if ( enabled ) {
			if ( GUI.Button(position, "Reset", style) ) {
				Application.LoadLevel(Application.loadedLevel);	
			}
		}
	}
} 
 
