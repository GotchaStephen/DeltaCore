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

public abstract class ClickyWheelElement : MonoBehaviour, IPointerEnterHandler {


    private static Text label = null;

    [SerializeField]
    private string elementName;

    [HideInInspector]
    protected Image highlight;

    // Use this for initialization
    protected void Start () {
		if(label == null) {
            label = transform.parent.GetComponentInChildren<Text>();
        }

        highlight = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void OnPointerEnter(PointerEventData eventData) {
        label.text = elementName;
    }
}
