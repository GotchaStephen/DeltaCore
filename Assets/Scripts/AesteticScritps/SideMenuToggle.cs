using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SideMenuToggle : MonoBehaviour, IPointerClickHandler {

    private Animator NavMenuAnimator;

    private int isOpen = Animator.StringToHash("isOpen");

    // Use this for initialization
    void Start () {
        NavMenuAnimator = GetComponentInParent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPointerClick(PointerEventData eventData) {
        NavMenuAnimator.SetBool(isOpen, !NavMenuAnimator.GetBool(isOpen));
    }
}
