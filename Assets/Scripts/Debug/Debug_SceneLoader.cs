using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Debug_SceneLoader : MonoBehaviour, IPointerClickHandler {

    // public int sceneIndex;
	public string sceneName = "NotSet" ; 

    public void OnPointerClick(PointerEventData eventData) {

        // NUN Guest Login
		if (!sceneName.Equals ("NotSet"))
		{
			// Play sound
			AudioController.instance.BigButtonClick();
			// Load scebe
			SceneManager.LoadScene(sceneName);
		}
	   //     SceneManager.LoadScene(sceneIndex);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
