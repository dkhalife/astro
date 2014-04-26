using UnityEngine; 
using System.Collections; 
 
public class ShowScore : MonoBehaviour  
{ 
	public GUIStyle style;
	Rect position =  new Rect(Screen.width - 250f, 10f, 270f, 50f);
	
	void OnGUI(){
		GameObject scoreComp = (GameObject)GameObject.FindGameObjectWithTag ("Score");
		Score playerScore = (Score)scoreComp.GetComponent ("Score");
		string score = playerScore.score.ToString ().PadLeft (4, '0');
		GUI.Label (position, "Score: " + score, style);
	}
} 
 
