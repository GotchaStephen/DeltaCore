/*
 (C) 2015
 your R&D lab
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class MovieDev : MonoBehaviour {

#if !UNITY_WEBGL
    MovieTexture mt;

    void Awake() {
        RawImage rim = GetComponent<RawImage>();
        mt = (MovieTexture)rim.mainTexture;
        mt.Stop();
        mt.Play();
    }

    void Update() {
        if (Input.GetButtonDown("Jump")) {
            if (mt.isPlaying) {
                mt.Stop();
            }
            else {
                mt.Stop();
                mt.Play();
            }
        }

    }
#endif
}
