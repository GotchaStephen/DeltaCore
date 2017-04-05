/* Copyright (C) 2017 Francesco Sapio - All Rights Reserved
 * The license use of this code and/or any portion is restricted to the 
 * the project DELTA CORE, subject to the condition that this
 * copyright notice shall remain.
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickyWheel_Marker : ClickyWheelElement, IPointerClickHandler {

    public Transform fingerPrint;
    public GameObject markerPrefab;
    public DeltaCore.MarkerConfidence confidence;
    public DeltaCore.MarkerType type;


    // Use this for initialization
    void Start () {
        base.Start();
        if (fingerPrint == null || markerPrefab == null) {
            Debug.LogWarning("ClickyWheel_Marker not initializated correctly");
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPointerClick(PointerEventData eventData) {
        GameObject marker = Instantiate(markerPrefab, fingerPrint);
        marker.GetComponent<FeatureMarker>().confidenceLevel = confidence;
        FindObjectOfType<ClickyWheelMenu>().Close();
    }
}
