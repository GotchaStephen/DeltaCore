using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePoints : MonoBehaviour {

	public int points = 100;
	public int counter = 0;

	public GameObject pointPrefab; 

	//confines of graph object
	public GameObject graph;



	Mesh graphMesh;

	// Use this for initialization
	void Start ()
	{
		RequestData();
		SetGraphConfines();
		CreatePointList();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RequestData()
	{
		//Request data from server

		//Add points to list

	}

	public void SetGraphConfines()
	{
		//Set dimensions of graph relative to values of points received

		//Get the mesh of the graph
		graphMesh = graph.GetComponent<MeshFilter>().mesh;
	}

	public void CreatePointList()
	{
		//Reset point lists
		ResetLists();

		//While there are still points to add
		while (counter < points)
		{
			InstantiatePoint(Random.Range(0f,100f), Random.Range(0f,100f), Random.Range(0f,100f));
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
		newPoint.transform.localPosition = new Vector3(	graphMesh.bounds.size.x / x,
													graphMesh.bounds.size.y / y,
													graphMesh.bounds.size.z / z);

		//Add this point GameObject to list
	}

	public void ResetLists()
	{
		counter = 0;

		//Reset point list
		//Reset gameobject list

	}
}
