using UnityEngine; 
using System.Collections; 
 
public class HighScore : MonoBehaviour  
{ 
	public GUIStyle style;

	public int score;
	
	void OnGUI(){
		if (enabled) {
			GameObject scoreComp = (GameObject)GameObject.FindGameObjectWithTag ("Score");
			Score playerScore = (Score)scoreComp.GetComponent ("Score");
			string current = playerScore.score.ToString ().PadLeft (4, '0');
			GUI.Label (new Rect(Screen.width / 2 - 65f, Screen.height / 2 - 30, 270f, 50f), "Score: " + current, style);

			int highscore = PlayerPrefs.GetInt ("High Score", 0);
			string best = highscore.ToString ().PadLeft (4, '0');
			GUI.Label (new Rect(Screen.width / 2 - 60f, Screen.height / 2, 270f, 50f), "Best: " + best, style);
		}
	}
} 
 
