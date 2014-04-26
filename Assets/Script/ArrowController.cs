using UnityEngine;
using System.Collections;

public class ArrowController : MonoBehaviour {

	public Transform mainCamera;
	public Transform target;
	public Transform center;
	public float margin = 10f;
	public float invisibleRadius = 5f;

	private int fixedHeight;
	private int spriteSide;
	private int variableWidth;
	private UI2DSprite sprite;
	private float iniAlpha;

	// Use this for initialization
	void Start () 
	{
		fixedHeight = transform.root.GetComponent<UIRoot> ().manualHeight;
		sprite = GetComponent<UI2DSprite> ();
		iniAlpha = sprite.color.a;
		spriteSide = sprite.width;
	}

	void ChangeArrowPositionOnScreen(float angleDeg)
	{
		float xLim = variableWidth / 2 - spriteSide/2 - margin;
		float yLim = fixedHeight / 2 - spriteSide/2 - margin;
		// Angle du coin supérieur droit par rapport au centre de l'écran
		float supDroit = Mathf.Atan ((float)fixedHeight / variableWidth) * Mathf.Rad2Deg;
		float supGauche = 180 - supDroit;
		float infGauche = 180 + supDroit;
		float infDroit = 360 - supDroit;

		float xPos=0, yPos=0;

		if (angleDeg >= supDroit && angleDeg < supGauche)
		{
			xPos = (fixedHeight / 2)/(Mathf.Tan(angleDeg*Mathf.Deg2Rad));
			yPos = yLim;
		} 
		else if (angleDeg >= supGauche && angleDeg < infGauche)
		{
			xPos = -xLim;
			yPos = (variableWidth / 2)/(Mathf.Tan((angleDeg - 90f)*Mathf.Deg2Rad));
		} 
		else if (angleDeg >= infGauche && angleDeg < infDroit)
		{
			xPos = -(fixedHeight / 2)/(Mathf.Tan(angleDeg*Mathf.Deg2Rad));
			yPos = -yLim;
		} 
		else if (angleDeg >= infDroit || angleDeg < supDroit)
		{
			xPos = xLim;
			yPos = -(variableWidth / 2)/(Mathf.Tan((angleDeg - 90f)*Mathf.Deg2Rad));
		}
		xPos = Mathf.Clamp (xPos, -xLim, xLim);
		yPos = Mathf.Clamp (yPos, -yLim, yLim);
		
		gameObject.transform.localPosition = new Vector3(xPos, yPos, 0f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		try {
			if (center == null || target == null)
				return;

			if (target.gameObject.renderer.isVisible)
			{
				sprite.alpha = 0.0f;
				//No need to update : quick exit
				return;
			}
			else
				sprite.alpha = iniAlpha;

			//update width in case of resize
			variableWidth = fixedHeight * Screen.width / Screen.height;

			Vector3 direction = target.position - center.position;
			//Needed because arrow is pointing north by default
			float offsetRotation = 0f;
			//Follow camera rotation
			float camRotation = mainCamera.transform.rotation.eulerAngles.y;
			//Point in target direction
			float targetRotation = (Mathf.Rad2Deg*Mathf.Atan2(direction.y, direction.x) + 360f)%360f;

			ChangeArrowPositionOnScreen (targetRotation);

			transform.rotation = Quaternion.Euler(0, 0, offsetRotation + camRotation + targetRotation);
		}catch(MissingReferenceException e){
			return ;
		}
	}
}
