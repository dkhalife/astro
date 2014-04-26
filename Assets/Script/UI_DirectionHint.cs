using UnityEngine;
using System.Collections;

public class UI_DirectionHint : MonoBehaviour
{
	public GameObject hintPrefab ;
	public Sprite spriteOverride = null;
	private GameObject hint;


	void CreateUIPrefab()
	{
		hint = GameObject.Instantiate(hintPrefab, transform.position, Quaternion.identity) as GameObject;
		//Copy sprite
		UI2DSprite sprite = hint.GetComponent<UI2DSprite>();
		sprite.sprite2D = spriteOverride!=null ? spriteOverride : GetComponent<SpriteRenderer>().sprite;

		//Setup arrow controller
		ArrowController ac = hint.GetComponent<ArrowController>();
		ac.mainCamera = Camera.main.transform;
		ac.target = transform;
		ac.center = GameObject.FindGameObjectWithTag("Player").transform;
	}

	// Object initialisation
	void Start()
	{
		CreateUIPrefab();
	}

	void OnDestroy()
	{
		GameObject.Destroy(hint);
	}
}
