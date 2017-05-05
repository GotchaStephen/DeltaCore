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
	public Quaternion defaultRotation;
	public Vector3 defaultTranslation;


	void Start()
	{
		defaultRotation = cameraArmRotation.transform.localRotation;
		defaultTranslation = cameraArmTranslation.transform.position;
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

	public void ResetViewer()
	{
		cameraArmRotation.transform.localRotation = defaultRotation;
		cameraArmTranslation.transform.position = defaultTranslation;
	}

}
