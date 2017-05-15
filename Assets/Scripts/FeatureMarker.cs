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
public class FeatureMarker : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IDragHandler,  IEndDragHandler {

    //Variables for Unity
    [SerializeField]
    private Sprite markerWithoutOrientation, markerWithOrientation;

    private SpriteRenderer spriteRenderer;

	private Vector3 initialPos;
	private Vector3 finalPos;

    //Variables for the marker
    public DeltaCore.MarkerType type;
    public DeltaCore.MarkerConfidence confidenceLevel;

    private bool hasOrientation;
    public bool isInPlacingMode = true;
	public bool clickedDown = false;
	public bool placed = false;

	public GameObject markerPrefab;

    //****************************************************
    public bool HasOrientation() {
        return hasOrientation;
    }

    public void ToggleOrientation()
	{
        hasOrientation = !hasOrientation;
        if (hasOrientation) {
            spriteRenderer.sprite = markerWithOrientation;
            //AnalyseScreenScript.LogAction("Marker in position: " + transform.localPosition + " has an orientation now", "marker");
        } else {
            spriteRenderer.sprite = markerWithoutOrientation;
            //AnalyseScreenScript.LogAction("Marker in position: " + transform.localPosition + " hasn't an orientation now", "marker");
        }

		ChangeMarker();
        
    }

    // Use this for initialization
    void Start () {

		print("started feature marker");

        spriteRenderer = GetComponent<SpriteRenderer>();

        if(markerWithoutOrientation == null || markerWithOrientation == null) {
            Debug.LogWarning("Error in inizialitation of the marker");
        }

		// Play sound
		AudioController.instance.Play(6, 1f, 1f);

		ChangeMarker();
    }

    public void Init(MarkerData md)
	{

        Start();
        transform.localPosition = md.position ;
        transform.eulerAngles = new Vector3(0, 0, md.orientation);
        if (md.hasOrientation)
		{
            hasOrientation = true;
            spriteRenderer.sprite = markerWithOrientation;
        }
        confidenceLevel = md.confidenceLevel;
        type = md.type;
        //isInPlacingMode = false;


    }
		

    private void logAction(string action, string tag = "")
    {
        if (UserInfo.currentGameMode == DeltaCore.GameMode.Training)
        {
            TrainingAnalyseScreenScript.LogAction(action, tag);
        }
        else
        {
            AnalyseScreenScript.LogAction(action, tag);
        }
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
			
		if (Input.GetKeyDown(KeyCode.LeftControl))
		{
			if (!placed)
			{
				BoxCollider2D collider = this.GetComponent<BoxCollider2D>();
				collider.enabled = false;
			}
		}
		else if (Input.GetKeyUp(KeyCode.LeftControl))
		{
			if (!placed)
			{
				BoxCollider2D collider = this.GetComponent<BoxCollider2D>();
				collider.enabled = true;
			}
		}

        //JUST FOR TEST

		//if (isInPlacingMode)
		if (!clickedDown)
		{
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
        //if (isInPlacingMode && (Input.GetKeyDown(KeyCode.Escape) || FindObjectOfType<ClickyWheelMenu>().isWheelOpen() || UIMarkerButton.newMarker))
		if (!placed && FindObjectOfType<ClickyWheelMenu>().isWheelOpen())
		{
			print(gameObject.name);
            Destroy(gameObject);
        }
	}

	public void ChangeMarker()
	{
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
	}

    public void changeConfidence() {
        switch (confidenceLevel) {
            case DeltaCore.MarkerConfidence.Low:
                confidenceLevel = DeltaCore.MarkerConfidence.Medium;
                logAction("Marker in position: " + transform.localPosition + " has changed confidence level into Medium", "marker");
                break;
            case DeltaCore.MarkerConfidence.Medium:
                confidenceLevel = DeltaCore.MarkerConfidence.High;
                logAction("Marker in position: " + transform.localPosition + " has changed confidence level into High", "marker");
                break;
            case DeltaCore.MarkerConfidence.High:
                confidenceLevel = DeltaCore.MarkerConfidence.Low;
                logAction("Marker in position: " + transform.localPosition + " has changed confidence level into Low", "marker");
                break;
        }

		ChangeMarker();
    }

    public void changeConfidence(DeltaCore.MarkerConfidence confidence)
	{
        confidenceLevel = confidence;
		ChangeMarker();
    }

    //public void OnDrag(PointerEventData eventData) {
	public void OnPointerDown(PointerEventData eventData) {
		print ("begin drag");

		if (eventData.button == PointerEventData.InputButton.Left)
		{
			clickedDown = true;

	        if (Input.GetKey(KeyCode.LeftAlt))
			{
	            return;
	        }
		}
    }
		
	public void OnDrag(PointerEventData eventData)
	{
		//if drag is with middle click and there is orientation on
		if (hasOrientation && eventData.button == PointerEventData.InputButton.Left)
		{
			//float angle = AngleBetweenVector2(eventData.position, eventData.pressPosition);
			float angle = AngleBetweenVector2(Camera.main.ScreenToWorldPoint(eventData.position) + new Vector3(0, 0, 10), initialPos);
			angle *= -1;
			angle += 90;
			transform.eulerAngles = new Vector3(0, 0, angle);
		}
	}

    public void OnEndDrag(PointerEventData eventData)
	{
        transform.SetAsFirstSibling();
        logAction("Marker placed in position: " + transform.localPosition, "marker");
		PlaceMarker();
    }

    private void updateAction(DeltaCore.UserLevelAction action, GameObject lastObjectAccessed)
    {
        if ( UserInfo.currentGameMode == DeltaCore.GameMode.Training)
        {
            TrainingAnalyseScreenScript.updateAction(action, lastObjectAccessed);
        } else
        {
            AnalyseScreenScript.updateAction(action, lastObjectAccessed);
        }
    }

	public void PlaceMarker()
	{
		GameObject lastObjectAccessed; 
		lastObjectAccessed = Instantiate(gameObject, transform.parent);
		FeatureMarker script = lastObjectAccessed.GetComponent<FeatureMarker>();
		script.placed = true;

		logAction("Marker has been created in position: " + transform.localPosition + " with confidence level of: " + confidenceLevel.ToString(), "marker");
		print ("success!");
		// Score Calculation after each Add
		updateAction(DeltaCore.UserLevelAction.AddMarker, lastObjectAccessed);
		//updateAction(DeltaCore.UserLevelAction.AddMarker, gameObject);
		isInPlacingMode = true;
		clickedDown = false;
	}

    public void OnPointerClick(PointerEventData eventData) {
        
		GameObject lastObjectAccessed;  

		//Left Control and Left Mouse 
		if (eventData.button == PointerEventData.InputButton.Left) {

			print ("attempting delete");
			if (Input.GetKey(KeyCode.LeftControl))
			{
				print ("lc pressed");
				//if (placed)
				//{
					lastObjectAccessed = gameObject;
					logAction("Marker in position: " + transform.localPosition + " has been erased", "marker");
					//Erase Marker
					GameObject.Destroy(gameObject);
					// Score Calculation after each Remove
					updateAction(DeltaCore.UserLevelAction.RemoveMarker, lastObjectAccessed);
				//}
				clickedDown = false;
				return;
			}
		}

        //isInPlacingMode = false;
        //if (isInPlacingMode)
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			PlaceMarker();
            return;     
        }


        //if double-click is detected
        if (eventData.clickCount == 2) {
            eventData.clickCount = 0;

            // Right double-click Change confidence
            //if (eventData.button == PointerEventData.InputButton.Right) { changeConfidence(); }

            //Middle double-Click
            if (eventData.button == PointerEventData.InputButton.Middle) { ToggleOrientation(); }
        }

		clickedDown = false;
    }

    private float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
	{
        Vector2 diference = vec2 - vec1;
        float sign = (vec2.y > vec1.y) ? -1.0f : 1.0f;
        return Vector2.Angle(Vector2.right, diference) * sign;
    }


}
