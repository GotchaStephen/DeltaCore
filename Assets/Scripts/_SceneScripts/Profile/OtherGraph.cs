using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherGraph : MonoBehaviour {

	public GameObject pointPrefab; 

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
		CreatePoints();
	}

	public void CreatePoints()
	{
		//Reset point lists
		ClearGraph();

		int counter = 0;
		int points = GraphData.numEntries;

		//For scaling
		graphX = graph.transform.localScale.x;
		graphY = graph.transform.localScale.y;
		graphZ = graph.transform.localScale.z;

		//While there are still points to add
		while (counter < points)
		{
			if (((GraphData.database[counter].state == "NSW") && toggleNSW) ||
				((GraphData.database[counter].state == "VIC") && toggleVIC) ||
				((GraphData.database[counter].state == "QLD") && toggleQLD) ||
				((GraphData.database[counter].state == "ACT") && toggleACT) ||
				((GraphData.database[counter].state == "SA") && toggleSA) ||
				((GraphData.database[counter].state == "WA") && toggleWA) ||
				((GraphData.database[counter].state == "NT") && toggleNT))
			{
				StartCoroutine(InstantiatePoint(	(GraphData.database[counter].insertCost / GraphData.maxInsertCost), 
													(GraphData.database[counter].substituteCost / GraphData.maxSubstituteCost),
													(GraphData.database[counter].deleteCost / GraphData.maxDeleteCost)));
			}
			counter++;
		}
	}

	IEnumerator InstantiatePoint(float x, float y, float z)
	{
		yield return new WaitForSeconds(1f);

		//Instantiate
		GameObject newPoint = Instantiate(pointPrefab);

		//Point name will look like "point(x,y,z)"
		string pointName = "point(" + x.ToString() + "," + y.ToString() + "," + z.ToString() + ")";
		newPoint.name = pointName;

		//Set parent
		newPoint.transform.SetParent(graph.transform, false);

		//Set position within parent confines
		newPoint.transform.localPosition = new Vector3(	x - (graphX/2), y - (graphY/2), z - (graphZ/2));

		//Add this point GameObject to list

		yield return null;
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

	public void ClearGraph()
	{

		var children = new List<GameObject>();
		foreach (Transform child in graph.transform)
		{	
			children.Add(child.gameObject);
		}
		children.ForEach(child => Destroy(child));
		

	}

	public void NSWButton()
	{
		toggleNSW = !toggleNSW; 
		CreatePoints();
	}
	public void VICButton()
	{
		toggleQLD = !toggleVIC;
		CreatePoints();
	}
	public void QLDButton()
	{
		toggleQLD = !toggleQLD;
		CreatePoints();
	}
	public void ACTButton()
	{
		toggleACT = !toggleACT;
		CreatePoints();
	}
	public void SAButton()
	{
		toggleSA = !toggleSA;
		CreatePoints();
	}
	public void WAButton()
	{
		toggleWA = !toggleWA;
		CreatePoints();
	}
	public void NTButton()
	{
		toggleNT = !toggleNT;
		CreatePoints();
	}
}
