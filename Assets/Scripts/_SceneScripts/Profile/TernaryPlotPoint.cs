using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class TernaryPlotPoint : MonoBehaviour {

	public void OnPointerClick(PointerEventData eventData)
	{
		print("clicked point!");

		Text description = GameObject.Find("GraphOneDescription").GetComponent<Text>();

		int id = Int32.Parse(this.name);

		string newDescription = 
			"Sample ID : " + GraphData.database[id].sampleID + "\r\n" +
			"Date Completed : " + GraphData.database[id].addedOn + "\r\n" +
			"Insert Cost : " + GraphData.database[id].insertCost + "\r\n" +
			"Substitute Cost : " + GraphData.database[id].substituteCost + "\r\n" +
			"Delete Cost : " + GraphData.database[id].deleteCost;

		description.text = newDescription;

	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
