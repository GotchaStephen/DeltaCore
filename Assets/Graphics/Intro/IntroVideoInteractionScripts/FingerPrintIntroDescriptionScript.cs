using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class FingerPrintIntroDescriptionScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public GameObject trigger;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPointerEnter(PointerEventData eventData) {
        trigger.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData) {
        trigger.SetActive(false);
    }
}
