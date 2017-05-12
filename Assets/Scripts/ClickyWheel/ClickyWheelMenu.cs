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

public class ClickyWheelMenu : MonoBehaviour, IPointerExitHandler {

    bool isOpen, inTransition;
    Image uiImage;

	// Use this for initialization
	void Start () {
        uiImage = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1) && !Input.GetKey(KeyCode.LeftAlt)) {
            
            isOpen = !isOpen;
            if (isOpen) {
                transform.position = Input.mousePosition;
                StartCoroutine(openMenu());
            }
            else {
                StartCoroutine(closeMenu());
            }
        }
	}

    IEnumerator openMenu() {
        inTransition = true;

		//Play sound
		AudioController.instance.Play(9, 1f, 1f);

        for(float f = 0; f < 0.2f;)
		{
            f += Time.deltaTime;
            float scale = Mathf.Lerp(0, 1, f / 0.2f);
            transform.localScale = new Vector3(scale, scale, scale);
            yield return new WaitForEndOfFrame();
        }
        inTransition = false;
    }

    IEnumerator closeMenu()
	{
        inTransition = true;

        for (float f = 0; f < 0.2f;) {
            f += Time.deltaTime;
            float scale = Mathf.Lerp(1, 0, f/0.2f);
            transform.localScale = new Vector3(scale, scale, scale);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(0.2f);
        inTransition = false;
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (inTransition)
            return;
        isOpen = false;
        StartCoroutine(closeMenu());
    }

    public void Open() {
        isOpen = true;
        transform.position = Input.mousePosition;
        StartCoroutine(openMenu());
    }

    public void Close() {
        isOpen = false;
        StartCoroutine(closeMenu());
    }

    public bool isWheelOpen() {
        return isOpen;
    }
}
