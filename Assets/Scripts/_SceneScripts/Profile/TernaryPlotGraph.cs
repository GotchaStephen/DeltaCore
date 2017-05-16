using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TernaryPlotGraph : MonoBehaviour {

	// Point value
	public float zX;
	public float zY;

	public GameObject graph;
	private float graphWidth;
	private float graphHeight;

	public GameObject pointPrefab0;
	public GameObject pointPrefab1;
	public GameObject pointPrefab2;

	private float maxTransparency = 0.8f;

	public GameObject graphInfo;
	public GameObject descriptionPrefab;
	[HideInInspector]
	public static Text descriptionText;

	// Use this for initialization
	void Start ()
	{
		// Get all values specific to user ID
		StartCoroutine(DisplayDescription());
		StartCoroutine(DisplayGraph());
	}
		
	IEnumerator DisplayGraph()
	{
		yield return new WaitForSeconds(2f);

		// Delete all previous data on graph
		ClearGraph();

		// Get number of values in list
		int points = GraphData.numEntries;
		graphWidth = graph.GetComponent<RectTransform>().rect.width;
		graphHeight = graph.GetComponent<RectTransform>().rect.height;

		// Create new list of user specific points
		for( int counter = 0 ; counter < points; counter++)
		{
			if(GraphData.database[counter].userID == UserInfo.id)
			{
				if(true)
				{
					//Then calculate point with these values
					CalculatePoint(	(GraphData.database[counter].insertCost / GraphData.maxInsertCost), 
									(GraphData.database[counter].substituteCost / GraphData.maxSubstituteCost), 
									(GraphData.database[counter].deleteCost / GraphData.maxDeleteCost));

					//Finally, draw
					DrawPoint(counter, GraphData.database[counter].verdict);
				}
			}

			yield return new WaitForSeconds(0.2f);
		}

		yield return null;
	}

	IEnumerator DisplayDescription()
	{
		//Instantiate
		GameObject d = Instantiate(descriptionPrefab);

		//Set parent
		d.transform.SetParent(graphInfo.transform, false);
		d.name = "GraphOneDescription";
		descriptionText = d.GetComponent<Text>();

		print ("created");

		yield return null;
	}

	public void CalculatePoint(float a, float b, float c)
	{
		// x value
		zX = (0.5f * (((2 * b) + c)/(a + b + c)));
		// y value
		zY = (( Mathf.Sqrt(3) / 2) * (c / (a + b + c)));
		print(zX + "," + zY);
	}

	public void DrawPoint(int id, int verdict)
	{
		string pointName = id.ToString();

		if (verdict == 0)
		{
			GameObject newPoint = Instantiate(pointPrefab0);
			newPoint.name = pointName;
			newPoint.transform.SetParent(graph.transform, false);
			newPoint.transform.localPosition = new Vector3(((zX * graphWidth) - (graphWidth/2)), ((zY * graphHeight) - (graphHeight/2)), 0f);
			StartCoroutine(FadeIn(newPoint));
		}
		else if (verdict == 1)
		{
			GameObject newPoint = Instantiate(pointPrefab1);
			newPoint.name = pointName;
			newPoint.transform.SetParent(graph.transform, false);
			newPoint.transform.localPosition = new Vector3(((zX * graphWidth) - (graphWidth/2)), ((zY * graphHeight) - (graphHeight/2)), 0f);
			StartCoroutine(FadeIn(newPoint));
		}
		else if (verdict == 2)
		{
			GameObject newPoint = Instantiate(pointPrefab2);
			newPoint.name = pointName;
			newPoint.transform.SetParent(graph.transform, false);
			newPoint.transform.localPosition = new Vector3(((zX * graphWidth) - (graphWidth/2)), ((zY * graphHeight) - (graphHeight/2)), 0f);
			StartCoroutine(FadeIn(newPoint));
		}
	}

	IEnumerator FadeIn(GameObject point)
	{
		float fadeRate = 0.05f;

		// Animate this change
		for(float i = 0f; i < maxTransparency ; i += fadeRate)
		{
			print ("fading in...");
			Color color = point.GetComponent<Image>().color;
			color.a += maxTransparency * fadeRate;
			point.GetComponent<Image>().color = color;

			yield return new WaitForSeconds(0.05f);
		}
	}

	public void PointClicked(GameObject point)
	{

		print("clicked point!");

		int id = Int32.Parse(point.name);

		string newDescription = 
			"Sample ID : " + "\r\n" +
			GraphData.database[id].sampleID + "\r\n" +
			"\r\n" +
			"Date Completed : " + "\r\n" +
			GraphData.database[id].addedOn + "\r\n" +
			"\r\n" +
			"Verdict: " + "\r\n" +
			GraphData.database[id].verdict + "\r\n" +
			"\r\n" +
			"Insert Cost : " + "\r\n" +
			GraphData.database[id].insertCost + "\r\n" +
			"\r\n" +
			"Substitute Cost : " + "\r\n" +
			GraphData.database[id].substituteCost + "\r\n" +
			"\r\n" +
			"Delete Cost : " + "\r\n" +
			GraphData.database[id].deleteCost;

		descriptionText.text = newDescription;

	}

	public void ClearGraph()
	{
		
	}
}
