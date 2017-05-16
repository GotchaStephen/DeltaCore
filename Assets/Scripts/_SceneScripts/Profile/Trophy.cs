using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Trophy : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	//the prefab
	public GameObject textDescription;

	GameObject textBox;

	public void OnPointerEnter (PointerEventData eventData)
	{
		ShowDescription(true);
	}

	public void OnPointerExit (PointerEventData eventData)
	{
		ShowDescription(false);
	}

	void ShowDescription(bool show)
	{
		if (show)
		{
			textBox = Instantiate(textDescription, transform.parent);
			textBox.transform.localPosition += this.transform.localPosition;
			Text textDes = textBox.gameObject.transform.GetChild(0).GetComponent<Text>();
			textDes.text = this.name;
		}
		else
		{
			Destroy(textBox);
		}
	}
}
