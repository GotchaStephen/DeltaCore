using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserDetails : MonoBehaviour {

	public string setUsername;
	public string tempUsername;
	public string setEmail;
	public string tempEmail;
	public string setState;
	public string tempState;
	public string setCountry;
	public string tempCountry;

	public bool updateUsername;
	public bool updateEmail;
	public bool updateState;
	public bool updateCountry;

	public Text usernameText;
	public Text emailText;
	public Text locationText;

	// Use this for initialization
	void Start ()
	{
		GetDetails();
		ResetFlags();
	}

	public void DisplayDetails()
	{
		if (UserInfo.firstName != null && UserInfo.lastName != null){usernameText.text = UserInfo.firstName + " " + UserInfo.lastName;}
		else {usernameText.text = "NOT SET";}

		if (UserInfo.username != null ){emailText.text = UserInfo.username;}
		else {emailText.text = "NOT SET";}

		locationText.text = "Australia";
	}

	public void UpdateEmail()
	{
		
	}

	public void UpdateLocation()
	{
		
	}

	public void ConfirmUpdates()
	{
		/*
		//Check if any changes have been made
		if ()
		{
			// Send updated information to server
		}
		*/

		UpdateDetails();
		ResetFlags();
	}

	public void GetDetails()
	{
		// Update fields upon scene load
		DisplayDetails();

		// Get trophies
		DisplayTrophies();
	}

	public void UpdateDetails()
	{
		if (updateUsername){setUsername = tempUsername;}
		if (updateEmail){setEmail = tempEmail;}
		if (updateState){setState = tempState;}
		if (updateCountry){setCountry = tempCountry;}
	}

	public void ResetFlags()
	{
		updateUsername = false;
		updateEmail = false;
		updateState = false;
		updateCountry = false;
	}

	public void DisplayTrophies()
	{
		
	}
}
