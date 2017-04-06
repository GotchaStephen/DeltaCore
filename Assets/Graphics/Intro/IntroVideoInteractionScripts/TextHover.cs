using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TextHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public GameObject target;

    public void OnPointerEnter(PointerEventData eventData) {
        target.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData) {
        target.SetActive(false);
    }


}
