using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMarkerButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler {

    public GameObject markerPrefab;
    public Transform fingerPrint;
    public DeltaCore.MarkerConfidence confidence;

    public void OnPointerClick(PointerEventData eventData) {
        GameObject marker = Instantiate(markerPrefab, fingerPrint);
        marker.GetComponent<FeatureMarker>().confidenceLevel = confidence;
        //FindObjectOfType<ClickyWheelMenu>().Close();
    }

    //THIS CODE IS TEMPORANY, UNTIL A BEST SOLUTION IS FOUND:
    public static bool newMarker;
    public void OnPointerDown(PointerEventData eventData) {
        newMarker = true;
    }

    public void OnPointerUp(PointerEventData eventData) {
        newMarker = false;
    }
}
