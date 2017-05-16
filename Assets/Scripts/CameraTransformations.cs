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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransformations : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public float zoom = 5;
    public float zoomSpeed;

    public float zoomMin;
    public float zoomMax;


    Vector2 lastMouseClick;
    Vector3 lastRotation;
    Vector3 lastPosition;
	[HideInInspector]
	public float mouseSensitivity = -0.005f;


    public void setMouseSensitivity(float value) {
        if (mouseSensitivity < 0) {
            mouseSensitivity = -value;
        } else {
            mouseSensitivity = value;
        }
    }

    public void InvertMouseSensitivity() {
        mouseSensitivity = -mouseSensitivity;
    }


    // Update is called once per frame
    void Update () {

        zoomSpeed = Mathf.Lerp(30, 100, zoom / 4.5f);

        zoom += (-Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomSpeed);
        zoom = Mathf.Clamp(zoom, zoomMin, zoomMax);
        Camera.main.orthographicSize = zoom;

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            lastMouseClick = Input.mousePosition;
            lastRotation = transform.eulerAngles;
        }

        /*
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.Mouse0)) {
            float angle = AngleBetweenVector2(Input.mousePosition, lastMouseClick);
            angle += 180;
            transform.eulerAngles = new Vector3(0, 0, angle) + lastRotation;
        }*/

        if (Input.GetMouseButtonDown(1)) {
            lastPosition = Input.mousePosition;
        }

        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.Mouse1))
		{
            Vector3 delta  = Input.mousePosition - lastPosition;
            transform.Translate(delta.x * mouseSensitivity, delta.y * mouseSensitivity, 0);
            lastPosition = Input.mousePosition;
        }
    }




    private float AngleBetweenVector2(Vector2 vec1, Vector2 vec2) {
        Vector2 diference = vec2 - vec1;
        float sign = (vec2.y > vec1.y) ? -1.0f : 1.0f;
        return Vector2.Angle(Vector2.right, diference) * sign;
    }
}
