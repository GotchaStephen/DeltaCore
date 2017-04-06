using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressAnyKeyToContinue : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public GameObject target;
    public float timeToWait;

    private float time;
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if (Input.anyKeyDown && time >= timeToWait) {
            gameObject.SetActive(false);
            target.SetActive(true);
        }
	}
}
