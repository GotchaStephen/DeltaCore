using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_SecondarySideMenuManager : MonoBehaviour {


    [SerializeField]
    GameObject[] screens;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Set(int i) {
        if(i<screens.Length && i >= 0) {
            Reset();
            screens[i].SetActive(true);
          
        }
    }

    public void Reset() {
        foreach(GameObject g in screens) {
            g.SetActive(false);
        }
    }
}
