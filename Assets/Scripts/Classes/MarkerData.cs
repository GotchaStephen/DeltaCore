using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DeltaCoreBE;

public class MarkerData {

    public Vector2 position;
    public DeltaCore.MarkerType type;
    public DeltaCore.MarkerConfidence confidenceLevel;
    public bool hasOrientation;
    public float orientation;

    public MarkerData(float fLocationX, float fLocationY, DeltaCore.MarkerType fType, DeltaCore.MarkerConfidence fConfidence, bool fHasOrientaton, float fOrientation)
    {
        position = new Vector2(fLocationX, fLocationY);
        type = fType ;
        confidenceLevel = fConfidence ;
        hasOrientation = fHasOrientaton;
        orientation = fOrientation ; 
    }

    public MarkerData (FeatureMarker marker) {
        position = marker.transform.localPosition;
        type = marker.type;
        confidenceLevel = marker.confidenceLevel;
        hasOrientation = marker.HasOrientation();
        orientation = marker.transform.localRotation.eulerAngles.z;
    }

    public MarkerData(MarkerData marker)
    {
        position = marker.position;
        type = marker.type;
        confidenceLevel = marker.confidenceLevel;
        hasOrientation = marker.hasOrientation;
        orientation = marker.orientation ;
    }
}
