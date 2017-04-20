using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TernaryPlot : MonoBehaviour {

	// Values for equation
	public float a;
	public float b;
	public float c;

	// Point value
	public float zX;
	public float zY;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DisplayGraph()
	{
		GetValues();

		//Then calculate point with these values
		CalculatePoint();

		//Finally, draw
		DrawPoint();
	}

	public void GetValues ()
	{
		// Query server by providing date for LOTD

		// Receive a, b, c

	}

	public void CalculatePoint()
	{
		// x value
		zX = ((1 / 2) * (((2 * b) + c)/(a + b + c)));

		// y value
		zY = (( Mathf.Sqrt(3) / 2) * (c / (a + b + c)));
	}

	public void DrawPoint()
	{
		// Draw point on graph

	}
}
