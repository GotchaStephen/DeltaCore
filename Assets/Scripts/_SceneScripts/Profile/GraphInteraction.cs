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

public class GraphInteraction : MonoBehaviour {

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

	void Start()
	{
	}

	// Update is called once per frame
	void Update ()
	{

		//Using camera for translation / rotations
		// On left click
		if (Input.GetMouseButtonDown(0))
		{
			if (EventSystem.current.IsPointerOverGameObject())
			{
				prePosX0 = Input.mousePosition.x;
				prePosY0 = Input.mousePosition.y;
			}
		}

		// Begin rotating once left dragging commences
		if (Input.GetMouseButton(0))
		{
			if (EventSystem.current.IsPointerOverGameObject())
			{

				// Get differences between old and new co-ordinates
				movX = Input.mousePosition.x - prePosX0;
				movY = Input.mousePosition.y - prePosY0;

				cameraArmTranslation.transform.localPosition += new Vector3((-movX / 100.0f), (-movY / 100.0f), 0f);

				// Set new position to old position
				prePosX0 = Input.mousePosition.x;
				prePosY0 = Input.mousePosition.y;
			}
		}   

		// On right click
		if (Input.GetMouseButtonDown(1))
		{
			if (EventSystem.current.IsPointerOverGameObject())
			{
				prePosX1 = Input.mousePosition.x;
				prePosY1 = Input.mousePosition.y;
			}
		}

		// Begin rotating once right dragging commences
		if (Input.GetMouseButton(1))
		{
			if (EventSystem.current.IsPointerOverGameObject())
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
