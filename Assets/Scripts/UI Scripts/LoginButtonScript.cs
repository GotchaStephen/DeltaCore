using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class LoginButtonScript : MonoBehaviour, IPointerClickHandler {

    //Reference to the Inputfileds for Username and Pass
    [SerializeField]
    InputField usernameInputField, passInputField;

    public void OnPointerClick(PointerEventData eventData) {
        Login();
    }

    private void Login() {
        bool isLoginSuccess = Database.Login(usernameInputField.text, passInputField.text);
        if (isLoginSuccess) {}
        else { Debug.Log(UserInfo.errorString); }
    }


    // Use this for initialization
    void Start () {
		if(usernameInputField == null || passInputField == null) {
            Debug.LogWarning("The Username (or pass) Input Field is not properly set up");
        }
	}


}
