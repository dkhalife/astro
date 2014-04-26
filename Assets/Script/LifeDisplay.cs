using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UILabel))]
public class LifeDisplay : MonoBehaviour
{
	public LifeOverlay lifeComponent;
	private UILabel label;
	private string iniText;
	// Object initialisation
	void Start()
	{
		label = GetComponent<UILabel>();
		iniText = label.text;
	}

	// Update the object (once per frame)
	void Update()
	{
		label.text = iniText + (lifeComponent.life + 1).ToString();
	}
}
