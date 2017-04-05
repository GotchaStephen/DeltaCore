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
using UnityEngine.EventSystems;
using System;

public class ClickyWheel_MyToggle : ClickyWheelElement, IPointerClickHandler{

    [SerializeField]
    private MyToggle toggle;


    // Use this for initialization
    void Start () {
        base.Start();
        if (toggle == null) {
            Debug.LogWarning("ClicklyWheel_MyToggle not properly initialized");
        }
        toggle.OnStateChangeEvent.AddListener(changeState);
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void changeState(bool newState) {
        if (newState) {
            highlight.fillAmount = 1;
        }
        else {
            highlight.fillAmount = 0;
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        toggle.toggle();
    }
}
