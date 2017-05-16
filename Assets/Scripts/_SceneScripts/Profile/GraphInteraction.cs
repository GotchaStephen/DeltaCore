using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*

zoom in = scroll up OR
zoom out = scroll down OR
move around selected point = left click + drag
rotate around centre of screen = right click + drag

NOT YET IMPLEMENTED
select new point = singular left click

*/
using UnityEngine.EventSystems;
using System;

public class GraphInteraction : MonoBehaviour,  IPointerDownHandler, IDragHandler {

	float zoom = 0.02f;

	float prePosX0;
	float prePosY0;
	float prePosX1;
	float prePosY1;

	float rotX;
	float rotY;
	float movX;
	float movY;

	//RaycastHit hit;

	public GameObject cameraArmRotation;
	public GameObject cameraArmTranslation;

	//Defaults for resetting
	public Vector3 defaultRotationI = new Vector3(0.0f, 0.0f, 0.0f);
	public Vector3 defaultTranslationI = new Vector3(0.0f, 0.0f, -1.5f);

	//Defaults for resetting
	public Vector3 defaultRotationS = new Vector3(0.0f, 90f, 90f);
	public Vector3 defaultTranslationS = new Vector3(0.0f, 0.0f, -1.5f);

	//Defaults for resetting
	public Vector3 defaultRotationD = new Vector3(270f, 90f, 180f);
	public Vector3 defaultTranslationD = new Vector3(0.0f, 0.0f, -1.5f);

	// Update is called once per frame
	void Update ()
	{
		//Using camera arm for zooming
		if (Input.GetAxis("Mouse ScrollWheel") > 0f ) // forward
		{
			cameraArmTranslation.transform.localPosition += new Vector3(0f, 0f, zoom);
		}
		else if (Input.GetAxis("Mouse ScrollWheel") < 0f ) // backwards
		{
			cameraArmTranslation.transform.localPosition -= new Vector3(0f, 0f, zoom);
		}

	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			prePosX0 = Input.mousePosition.x;
			prePosY0 = Input.mousePosition.y;
		}
		else if (eventData.button == PointerEventData.InputButton.Right)
		{
			prePosX1 = Input.mousePosition.x;
			prePosY1 = Input.mousePosition.y;
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			// Get differences between old and new co-ordinates
			movX = Input.mousePosition.x - prePosX0;
			movY = Input.mousePosition.y - prePosY0;

			cameraArmTranslation.transform.localPosition += new Vector3((-movX / 100.0f), (-movY / 100.0f), 0f);

			// Set new position to old position
			prePosX0 = Input.mousePosition.x;
			prePosY0 = Input.mousePosition.y;
		}
		else if (eventData.button == PointerEventData.InputButton.Right)
		{
			// Get differences between old and new co-ordinates
			rotX = Input.mousePosition.x - prePosX1;
			rotY = Input.mousePosition.y - prePosY1;

			cameraArmRotation.transform.Rotate(new Vector3(-rotY / 2, rotX / 2, 0.0f));

			// Set new position to old position
			prePosX1 = Input.mousePosition.x;
			prePosY1 = Input.mousePosition.y;
		}
	}

	public void ResetViewerI()
	{
		cameraArmRotation.transform.localRotation = Quaternion.Euler(defaultRotationI);
		cameraArmTranslation.transform.localPosition = defaultTranslationI;
	}

	public void ResetViewerS()
	{
		cameraArmRotation.transform.localRotation = Quaternion.Euler(defaultRotationS);
		cameraArmTranslation.transform.localPosition = defaultTranslationS;
	}

	public void ResetViewerD()
	{
		cameraArmRotation.transform.localRotation = Quaternion.Euler(defaultRotationD);
		cameraArmTranslation.transform.localPosition = defaultTranslationD;
	}

}
