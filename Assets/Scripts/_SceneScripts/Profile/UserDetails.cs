using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	// Use this for initialization
	void Start () {
		GetDetails();
		ResetFlags();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DisplayDetails()
	{
		
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

		// Get username
		// Get email
		// Get state
		// Get country

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
