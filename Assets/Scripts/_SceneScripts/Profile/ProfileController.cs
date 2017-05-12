using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileController : MonoBehaviour {

	public GameObject graph1;
	public GameObject graph2;
	public GameObject graph3;

	public void ButtonPress()
	{
		AudioController.instance.SmallButtonClick();
	}
		
	public void ShowGraph1(){graph1.SetActive(true);}
	public void HideGraph1(){graph1.SetActive(false);}
	public void ShowGraph2(){graph2.SetActive(true);}
	public void HideGraph2(){graph2.SetActive(false);}
	public void ShowGraph3(){graph3.SetActive(true);}
	public void HideGraph3(){graph3.SetActive(false);}
}
