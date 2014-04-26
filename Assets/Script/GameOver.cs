using UnityEngine; 
using System.Collections; 
 
public class GameOver : MonoBehaviour  
{ 
	public GameObject deathPrefab;

	public void GameOverStartEffects(){
		Vector3 pos = transform.position;
		pos.z = -5f;
		GameObject.Instantiate (deathPrefab, pos, Quaternion.identity);
	}

	public void GameOverAfterEffects(){
		MonoBehaviour resetButton = (MonoBehaviour) Camera.main.GetComponent("ResetButton");
		resetButton.enabled = true;
		
		MonoBehaviour highscore = (MonoBehaviour) Camera.main.GetComponent("HighScore");
		highscore.enabled = true;
		
		GameObject scoreComp = (GameObject)GameObject.FindGameObjectWithTag ("Score");
		Score playerScore = (Score)scoreComp.GetComponent ("Score");
		int currentScore = playerScore.score;
		int highScore = PlayerPrefs.GetInt ("High Score", 0);
		if (currentScore > highScore) {
			PlayerPrefs.SetInt("High Score", currentScore);
		}
	}
} 
 
