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
using UnityEngine.Events;

public class Knob : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {

    [SerializeField]
    public bool realtime;

    public RectTransform tuner;
    public Image fillImage;

    public float minValue, maxValue = 1f;

    [System.Serializable]
    public class OnValueChange : UnityEvent<float> { }

    public OnValueChange OnValueChangeEvent;

    void Start() {
        if (tuner) {
            tuner.eulerAngles = new Vector3(0, 0, -(fillImage.fillAmount-minValue)/(maxValue-minValue)*360f);
            if(minValue < 0)
                fillImage.fillAmount = (fillImage.fillAmount - minValue) / (maxValue - minValue);
        }
    }

    public void OnBeginDrag(PointerEventData eventData) {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData) {
        Vector3 cursorPosition = eventData.position;
        float angle = AngleBetweenVector2(fillImage.rectTransform.position, cursorPosition);
        angle -= 180;
        angle *= -1;

        fillImage.fillAmount = angle / 360f;

        if (tuner) {
            tuner.eulerAngles = new Vector3(0, 0, -angle);
        }
        if (realtime) {
            OnValueChangeEvent.Invoke(minValue + fillImage.fillAmount * (maxValue - minValue));
        }
    }


    private float AngleBetweenVector2(Vector2 vec1, Vector2 vec2) {
        Vector2 diference = vec2 - vec1;
        float sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
        return Vector2.Angle(Vector2.right, diference) * sign;
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (!realtime)
            OnValueChangeEvent.Invoke(minValue + fillImage.fillAmount * (maxValue - minValue));
    }

}
