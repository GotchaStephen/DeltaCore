/* Copyright (C) 2017 Francesco Sapio - All Rights Reserved
* The license use of this code and/or any portion is restricted to the 
* the project DELTA CORE, subject to the condition that this
* copyright notice shall remain.
*
* The particular script where this copyright is attached
* cannot be modified under any circumstances without permission.
*  
* Modification, distribution, reproduction or sale of this 
* code for purposes outside of the license agreement is 
* strictly forbidden.
* 
* NOTICE:  All information, intellectual and technical
* concepts contained herein are, and remain, the property of Francesco Sapio.
*
* Attribution is required.
* Please write to: contact@francescosapio.com
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

[RequireComponent(typeof(Image))]
public class MyToggle : MonoBehaviour, IPointerClickHandler {

    private Image uiImage;

    public Sprite toggleOff, toggleOn;

    public bool state;

    [System.Serializable]
    public class OnStateChange : UnityEvent<bool> { }

    public OnStateChange OnStateChangeEvent;


    // Use this for initialization
    void Start () {
        uiImage = GetComponent<Image>();
        if (state) {
            uiImage.sprite = toggleOn;
        }
        else {
            uiImage.sprite = toggleOff;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPointerClick(PointerEventData eventData) {
        toggle();
    }

    public void toggle() {
        state = !state;
        if (state) {
            uiImage.sprite = toggleOn;
        }else {
            uiImage.sprite = toggleOff;
        }
        OnStateChangeEvent.Invoke(state);
        
    }
}
