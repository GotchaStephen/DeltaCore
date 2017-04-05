using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeSetting : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Knob>().fillImage.fillAmount = AudioListener.volume;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setVolume(float volume) {
        AudioListener.volume = volume;
    }
}
