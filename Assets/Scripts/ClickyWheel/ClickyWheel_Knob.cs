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

public class ClickyWheel_Knob : ClickyWheelElement, IDragHandler, IEndDragHandler {

    [SerializeField]
    private Knob knob;

    // Use this for initialization
    void Start () {
        base.Start();
		if(knob == null) {
            Debug.LogWarning("ClickyWheel_Knob not initializated correctly");
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnDrag(PointerEventData eventData) {
        float newAmmount = Mathf.Clamp01((Vector2.Distance(eventData.position, transform.parent.position) - 93f) / 100f);
        knob.fillImage.fillAmount = newAmmount;
        highlight.fillAmount = newAmmount;

        if (knob.realtime) {
            knob.OnValueChangeEvent.Invoke(knob.minValue + newAmmount * (knob.maxValue - knob.minValue));
        }
        //Debug.Log(Vector2.Distance(eventData.position, transform.parent.position));
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (!knob.realtime) {
            knob.OnValueChangeEvent.Invoke(knob.minValue + highlight.fillAmount * (knob.maxValue - knob.minValue));
        }
    }
}
