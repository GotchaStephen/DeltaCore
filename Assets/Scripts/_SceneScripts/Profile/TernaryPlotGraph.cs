using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TernaryPlotGraph : MonoBehaviour {

	// Point value
	public float zX;
	public float zY;

	public GameObject graph;
	public GameObject pointPrefab;

	// Use this for initialization
	void Init ()
	{
		// Get all values specific to user ID
		DisplayGraph();
	}
		
	public void DisplayGraph()
	{
		// Delete all previous data on graph
		ClearGraph();

		// Get number of values in list
		int points = GraphData.numEntries;
		int counter = 0;

		// Create new list of user specific points
		while(counter < points)
		{
			//if(GraphData.Entry[counter].uid == )
			if(true)
			{
				//Then calculate point with these values
				CalculatePoint(	GraphData.database[counter].insertCost, 
								GraphData.database[counter].substituteCost, 
								GraphData.database[counter].deleteCost);

				//Finally, draw
				DrawPoint();
			}
			counter ++;
		}
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
	}

	public void ClearGraph()
	{
		
	}
}
