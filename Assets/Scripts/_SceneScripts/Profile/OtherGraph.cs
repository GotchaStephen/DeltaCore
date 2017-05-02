using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherGraph : MonoBehaviour {

	public GameObject pointPrefab; 

	//confines of graph object
	public GameObject graph;

	Mesh graphMesh;

	// Use this for initialization
	void Start ()
	{
		SetGraphConfines();
		CreatePoints();
	}
		
	public void SetGraphConfines()
	{
		//Set dimensions of graph relative to values of points received

		//Get the mesh of the graph
		graphMesh = graph.GetComponent<MeshFilter>().mesh;
	}

	public void CreatePoints()
	{
		//Reset point lists
		ClearGraph();

		int counter = 0;
		int points = GraphData.numEntries;

		//While there are still points to add
		while (counter < points)
		{
			//If conditions are met
			if (true)
			{
				InstantiatePoint(GraphData.database[counter].insertCost, GraphData.database[counter].substituteCost, GraphData.database[counter].deleteCost);
			}
			counter++;
		}
	}

	public void InstantiatePoint(float x, float y, float z)
	{

		//Instantiate
		GameObject newPoint = Instantiate(pointPrefab);

		//Point name will look like "point(x,y,z)"
		string pointName = "point(" + x.ToString() + "," + y.ToString() + "," + z.ToString() + ")";
		newPoint.name = pointName;

		//Set parent
		newPoint.transform.SetParent(graph.transform, false);

		//print(graphMesh.bounds.size.x + "," + graphMesh.bounds.size.y + "," +graphMesh.bounds.size.z);

		//Set position within parent confines
		//newPoint.transform.localPosition = new Vector3(	x / graphMesh.bounds.size.x, y / graphMesh.bounds.size.y, z / graphMesh.bounds.size.z);

		newPoint.transform.localPosition = new Vector3(	x / GraphData.maxInsertCost, y / GraphData.maxSubstituteCost, z / GraphData.maxDeleteCost);

		//Add this point GameObject to list
	}

	public void ClearGraph()
	{

		//Reset point list
		//Reset gameobject list

	}
}
