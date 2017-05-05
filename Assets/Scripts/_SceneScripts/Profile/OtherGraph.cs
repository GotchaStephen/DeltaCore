using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OtherGraph : MonoBehaviour {

	public static List<GraphData.Entry> databasecopy = new List<GraphData.Entry>();

	public GameObject pointPrefab; 
	float maxTransparency = 0.8f;
	float fadeRate = 0.05f;

	//confines of graph object
	public GameObject graph;

	//Grid
	public GameObject block;
	public GameObject blockParent;

	Mesh graphMesh;
	float graphX;
	float graphY;
	float graphZ;

	// State toggles
	bool toggleNSW = true;
	bool toggleVIC = true;
	bool toggleQLD = true;
	bool toggleACT = true;
	bool toggleSA = true;
	bool toggleWA = true;
	bool toggleNT = true;

	// Use this for initialization
	void Start ()
	{
		StartCoroutine(CreateGrid());

		//For scaling
		graphX = graph.transform.localScale.x;
		graphY = graph.transform.localScale.y;
		graphZ = graph.transform.localScale.z;

		CreatePoints();
	}

	public void CreatePoints()
	{
		int counter = 0;

		//While there are still points to add
		while (counter < GraphData.numEntries)
		{
			InstantiatePoint(GraphData.database[counter], counter);
			counter++;
		}

		AdjustGraph();
	}

	public void AdjustGraph()
	{
		int counter = 0;

		Transform[] allChildren = graph.GetComponentsInChildren<Transform>();
		foreach (Transform child in allChildren)
		{
			int childName;
			if (Int32.TryParse(child.gameObject.name, out childName))
			{	
				if (((GraphData.database[childName].state == "NSW") && toggleNSW) ||
					((GraphData.database[childName].state == "VIC") && toggleVIC) ||
					((GraphData.database[childName].state == "QLD") && toggleQLD) ||
					((GraphData.database[childName].state == "ACT") && toggleACT) ||
					((GraphData.database[childName].state == "SA") && toggleSA) ||
					((GraphData.database[childName].state == "WA") && toggleWA) ||
					((GraphData.database[childName].state == "NT") && toggleNT))
				{
					StartCoroutine(FadeIn(child.gameObject));
				}
				else
				{
					StartCoroutine(FadeOut(child.gameObject));
				}
			}
		}
	}

	public void InstantiatePoint(GraphData.Entry entry, int counter)
	{
		float x = entry.insertCost / GraphData.maxInsertCost;
		float y = entry.substituteCost / GraphData.maxSubstituteCost;
		float z = entry.deleteCost / GraphData.maxDeleteCost;

		//Instantiate
		GameObject newPoint = Instantiate(pointPrefab);

		string pointName = counter.ToString();
		newPoint.name = pointName;

		//Set parent
		newPoint.transform.SetParent(graph.transform, false);

		//Set position within parent confines
		newPoint.transform.localPosition = new Vector3(	x - (graphX/2), y - (graphY/2), z - (graphZ/2));

		//Initiate the line renderer
		LineRenderer lr = newPoint.GetComponent<LineRenderer>();
		lr.SetPosition(0, new Vector3(-graphX/2, -graphY/2, -graphZ/2));
		lr.SetPosition(1, newPoint.transform.position);
	}

	IEnumerator CreateGrid()
	{
		float blockScale = blockParent.transform.localScale.x * 0.5f;

		for(float x = 0f; x <= 1; x = x + 0.1f)
		{
			for(float y = 0f; y <= 1; y = y + 0.1f)
			{  
				for(float z = 0f; z <= 1; z = z + 0.1f)
				{                
					GameObject gridBlock = Instantiate(block);
					gridBlock.transform.localPosition = new Vector3(x - blockScale, y - blockScale, z - blockScale);
					gridBlock.transform.SetParent(blockParent.transform, false);
				}
			}
		}
		yield return null;
	}

	IEnumerator FadeIn(GameObject point)
	{
		float startingTransparency = point.GetComponent<Renderer>().material.color.a;

		point.GetComponent<Renderer>().enabled = true;

		// Animate this change
		for(float i = startingTransparency; i < maxTransparency ; i = i + fadeRate)
		{
			Color color = point.GetComponent<Renderer>().material.color;
			color.a += maxTransparency * fadeRate;
			point.GetComponent<Renderer>().material.color = color;

			yield return new WaitForSeconds(fadeRate);
		}

		//Finally
		Color colorF = point.GetComponent<Renderer>().material.color;
		colorF.a = maxTransparency;
		point.GetComponent<Renderer>().material.color = colorF;

		//Initiate the line renderer
		LineRenderer lr = point.GetComponent<LineRenderer>();
		lr.enabled = true;
	}

	IEnumerator FadeOut(GameObject point)
	{
		float startingTransparency = point.GetComponent<Renderer>().material.color.a;

		// Animate this change
		for(float i = startingTransparency; i > 0f ; i = i - fadeRate)
		{
			Color color = point.GetComponent<Renderer>().material.color;
			color.a -= maxTransparency * fadeRate;
			point.GetComponent<Renderer>().material.color = color;

			yield return new WaitForSeconds(fadeRate);
		}

		//Finally
		Color colorF = point.GetComponent<Renderer>().material.color;
		colorF.a = 0f;
		point.GetComponent<Renderer>().material.color = colorF;

		// Accounts for extra things like emission
		point.GetComponent<Renderer>().enabled = false;

		//Initiate the line renderer
		LineRenderer lr = point.GetComponent<LineRenderer>();
		lr.enabled = false;

		yield return null;
	}

	public void NSWButton()
	{
		toggleNSW = !toggleNSW; 
		AdjustGraph();
	}
	public void VICButton()
	{
		toggleVIC = !toggleVIC;
		AdjustGraph();
	}
	public void QLDButton()
	{
		toggleQLD = !toggleQLD;
		AdjustGraph();
	}
	public void ACTButton()
	{
		toggleACT = !toggleACT;
		AdjustGraph();
	}
	public void SAButton()
	{
		toggleSA = !toggleSA;
		AdjustGraph();
	}
	public void WAButton()
	{
		toggleWA = !toggleWA;
		AdjustGraph();
	}
	public void NTButton()
	{
		toggleNT = !toggleNT;
		AdjustGraph();
	}
}
