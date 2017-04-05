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

[RequireComponent(typeof(SpriteRenderer))]
public class FeatureMarker : MonoBehaviour, IPointerClickHandler, IDragHandler, IEndDragHandler {

    //Variables for Unity
    [SerializeField]
    private Sprite markerWithoutOrientation, markerWithOrientation;

    private SpriteRenderer spriteRenderer;

    //Variables for the marker
    public DeltaCore.MarkerType type;
    public DeltaCore.MarkerConfidence confidenceLevel;

    private bool hasOrientation;
    public bool isInPlacingMode = true;

    //****************************************************
    public bool HasOrientation() {
        return hasOrientation;
    }

    public void ToggleOrientation() {
        hasOrientation = !hasOrientation;
        if (hasOrientation) {
            spriteRenderer.sprite = markerWithOrientation;
            AnalyseScreenScript.LogAction("Marker in position: " + transform.localPosition + " has an orientation now", "marker");
        } else {
            spriteRenderer.sprite = markerWithoutOrientation;
            AnalyseScreenScript.LogAction("Marker in position: " + transform.localPosition + " hasn't an orientation now", "marker");
        }
        
    }

    // Use this for initialization
    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if(markerWithoutOrientation == null || markerWithOrientation == null) {
            Debug.LogWarning("Error in inizialitation of the marker");
        }
    }

    public void Init(MarkerData md) {
        Start();
        transform.localPosition = md.position ;
        transform.eulerAngles = new Vector3(0, 0, md.orientation);
        if (md.hasOrientation) {
            hasOrientation = true;
            spriteRenderer.sprite = markerWithOrientation;
        }
        confidenceLevel = md.confidenceLevel;
        type = md.type;
        isInPlacingMode = false;
    }
	
	// Update is called once per frame
	void Update () {
        //THIS CODE SHOULD BE PLACED ONLY WHEN THE TYPE OF THE MARKER IS CHANGED
        //IT IS HERE JUST FOR PROTOTYPING TEST
        CameraTransformations ct = FindObjectOfType<CameraTransformations>();
        float step = (ct.zoom-ct.zoomMin)/(ct.zoomMax-ct.zoomMin);
        float alpha = Mathf.Lerp(0.5f, 0.9f, step);
        float scale = Mathf.Lerp(0.04f, 0.076f, step);
        transform.localScale = new Vector3(scale, scale, scale);
        switch (confidenceLevel) {
            case DeltaCore.MarkerConfidence.Low:
                //spriteRenderer.color = Color.red;
                spriteRenderer.color = new Color(1, 0, 0, alpha);
                break;
            case DeltaCore.MarkerConfidence.Medium:
                //spriteRenderer.color = Color.yellow;
                spriteRenderer.color = new Color(1, 0.92f, 0.16f, alpha);
                break;
            case DeltaCore.MarkerConfidence.High:
                //spriteRenderer.color = Color.green;
                spriteRenderer.color = new Color(0, 1, 0, alpha);
                break;
        }

        //JUST FOR TEST
        if (isInPlacingMode) {
            Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);

            Vector3 min = transform.parent.GetComponent<SpriteRenderer>().bounds.min + spriteRenderer.bounds.extents / 2f;
            Vector3 max = transform.parent.GetComponent<SpriteRenderer>().bounds.max - spriteRenderer.bounds.extents / 2f;

            //min *= transform.parent.localScale.x;
            //max *= transform.parent.localScale.x;

            newPos = new Vector3(Mathf.Clamp(newPos.x, min.x, max.x), Mathf.Clamp(newPos.y, min.y, max.y), newPos.z);
            transform.position = newPos;
        }

        //JUST FOR TEST
        //The call to FindObject is really expensive, leave it from the Update or take a reference to it to use it without the explicit call
        if (isInPlacingMode && (Input.GetKeyDown(KeyCode.Escape) ||FindObjectOfType<ClickyWheelMenu>().isWheelOpen() || UIMarkerButton.newMarker)) {
            Destroy(gameObject);
        }
	}

    public void changeConfidence() {
        switch (confidenceLevel) {
            case DeltaCore.MarkerConfidence.Low:
                confidenceLevel = DeltaCore.MarkerConfidence.Medium;
                AnalyseScreenScript.LogAction("Marker in position: " + transform.localPosition + " has changed confidence level into Medium", "marker");
                break;
            case DeltaCore.MarkerConfidence.Medium:
                confidenceLevel = DeltaCore.MarkerConfidence.High;
                AnalyseScreenScript.LogAction("Marker in position: " + transform.localPosition + " has changed confidence level into High", "marker");
                break;
            case DeltaCore.MarkerConfidence.High:
                confidenceLevel = DeltaCore.MarkerConfidence.Low;
                AnalyseScreenScript.LogAction("Marker in position: " + transform.localPosition + " has changed confidence level into Low", "marker");
                break;
        }
    }

    public void changeConfidence(DeltaCore.MarkerConfidence confidence) {
        confidenceLevel = confidence;
    }

    public void OnDrag(PointerEventData eventData) {

        if (Input.GetKey(KeyCode.LeftAlt)) {
            return;
        }

        //If drag is with left click
        if(eventData.button == PointerEventData.InputButton.Left) {
            Vector3 newPos = Camera.main.ScreenToWorldPoint(eventData.position) + new Vector3(0,0,10);

            Vector3 min = transform.parent.GetComponent<SpriteRenderer>().bounds.min + spriteRenderer.bounds.extents/2f;
            Vector3 max = transform.parent.GetComponent<SpriteRenderer>().bounds.max - spriteRenderer.bounds.extents/2f;

            //min *= transform.parent.localScale.x;
            //max *= transform.parent.localScale.x;

            newPos = new Vector3(Mathf.Clamp(newPos.x, min.x, max.x), Mathf.Clamp(newPos.y, min.y, max.y), newPos.z);
            transform.position = newPos;
        }

        //if drag is with middle click and there is orientation on
        if (eventData.button == PointerEventData.InputButton.Middle && hasOrientation) {

            //float angle = AngleBetweenVector2(eventData.position, eventData.pressPosition);
            float angle = AngleBetweenVector2(Camera.main.ScreenToWorldPoint(eventData.position) + new Vector3(0, 0, 10), transform.position);
            angle *= -1;
            angle += 90;
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        transform.SetAsFirstSibling();
        AnalyseScreenScript.LogAction("Marker placed in position: " + transform.localPosition, "marker");
    }

    public void OnPointerClick(PointerEventData eventData) {
        GameObject lastObjectAccessed;  

        #region Testing functions 

        #endregion

        //isInPlacingMode = false;
        if (isInPlacingMode && eventData.button == PointerEventData.InputButton.Left) {
            lastObjectAccessed = Instantiate(gameObject, transform.parent);
            isInPlacingMode = false ;
            AnalyseScreenScript.LogAction("Marker has been created in position: " + transform.localPosition + " with confidence level of: " + confidenceLevel.ToString(), "marker");
            // Score Calculation after each Add
            AnalyseScreenScript.updateAction(DeltaCore.UserLevelAction.AddMarker, lastObjectAccessed);
            return;     
        }


        //Left Contorl and Left Mouse 
        if (eventData.button == PointerEventData.InputButton.Left && Input.GetKey(KeyCode.LeftControl)) {
            if (Input.GetKey(KeyCode.LeftControl)){
                lastObjectAccessed = gameObject; 
                AnalyseScreenScript.LogAction("Marker in position: " + transform.localPosition + " has been erased", "marker");
                //Erase Marker
                GameObject.Destroy(gameObject);
                // Score Calculation after each Remove
                AnalyseScreenScript.updateAction(DeltaCore.UserLevelAction.RemoveMarker, lastObjectAccessed);
                return;
            }
        }

        //if double-click is detected
        if (eventData.clickCount == 2) {
            eventData.clickCount = 0;

            // Left double-click Change confidence
            if (eventData.button == PointerEventData.InputButton.Left) { changeConfidence(); }

            //Middle double-Click
            if (eventData.button == PointerEventData.InputButton.Middle) { ToggleOrientation(); }
        }
    }

    private float AngleBetweenVector2(Vector2 vec1, Vector2 vec2) {
        Vector2 diference = vec2 - vec1;
        float sign = (vec2.y > vec1.y) ? -1.0f : 1.0f;
        return Vector2.Angle(Vector2.right, diference) * sign;
    }


}
