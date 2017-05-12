using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_SecondarySideMenuManager : MonoBehaviour {


    [SerializeField]
    GameObject[] screens;

    [HideInInspector]
    public int currentScreen;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Set(int i)
	{
        if(i<screens.Length && i >= 0) {
            Reset();
            screens[i].SetActive(true);
            currentScreen = i;

			//Play sound
			AudioController.instance.BigButtonClick();
        }
    }

    public void Reset() {
        foreach(GameObject g in screens) {
            g.SetActive(false);
        }
    }
}
