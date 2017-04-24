using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TernaryPlot : MonoBehaviour {


	public int points = 100;
	public int counter = 0;

	// Point value
	public float zX;
	public float zY;

	public GameObject graph;
	public GameObject pointPrefab;

	public List<Weightings> weightingsList = new List<Weightings>();

	public struct Weightings
	{
		public float xVal;
		public float yVal;
		public float zVal;

		public Weightings(float x, float y, float z)
		{
			xVal = x;
			yVal = y;
			zVal = z;
		}
	}

	// Use this for initialization
	void Start ()
	{
		//Dummy values
		weightingsList.Add(new Weightings(0.1f, 0.1f, 0.8f));
		weightingsList.Add(new Weightings(0.1f, 0.2f, 0.7f));
		weightingsList.Add(new Weightings(0.1f, 0.3f, 0.9f));
		weightingsList.Add(new Weightings(0.3f, 0.2f, 0.5f));
		weightingsList.Add(new Weightings(0.4f, 0.1f, 0.5f));
		weightingsList.Add(new Weightings(0.5f, 0.3f, 0.2f));
		weightingsList.Add(new Weightings(0.2f, 0.7f, 0.1f));
		weightingsList.Add(new Weightings(0.1f, 0.8f, 0.1f));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DisplayGraph()
	{
		GetValues();

		// Get number of values in list
		points = weightingsList.Count;

		while(counter < points)
		{
			//Then calculate point with these values
			CalculatePoint(	weightingsList[counter].xVal, 
							weightingsList[counter].yVal,
							weightingsList[counter].zVal);

			//Finally, draw
			DrawPoint();

			counter ++;
		}

		counter = 0;
	}

	public void GetValues ()
	{
		// Query server by providing date for LOTD

		// Receive a, b, c

		// Add these to list

	}

	public void CalculatePoint(float a, float b, float c)
	{
		// x value
		zX = ((1 / 2) * (((2 * b) + c)/(a + b + c)));
		// y value
		zY = (( Mathf.Sqrt(3) / 2) * (c / (a + b + c)));
	}

	public void DrawPoint()
	{

		//Instantiate
		GameObject newPoint = Instantiate(pointPrefab);

		//Point name will look like "point(x,y)"
		string pointName = "point(" + zX.ToString() + "," + zY.ToString() + ")";
		newPoint.name = pointName;

		//Set parent
		newPoint.transform.SetParent(graph.transform, false);

		//Set position within parent confines
		newPoint.transform.localPosition = new Vector2(zX, zY);

		//Add this point GameObject to list
	}
}
