using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BarGraph: MonoBehaviour {

	// 9 graphics that make up the display
	public Image x1Graphic;
	public Image y1Graphic;
	public Image z1Graphic;
	public Image x2Graphic;
	public Image y2Graphic;
	public Image z2Graphic;
	public Image x3Graphic;
	public Image y3Graphic;
	public Image z3Graphic;

	public int day;
	public int month;
	public int year;

	public bool showByDay = true;
	public bool showByMonth = false;
	public bool showByYear = false;


	//The database currently being displayed
	public List<GraphData.Entry> weightingsGraph = new List<GraphData.Entry>();

	// Use this for initialization
	void Start () {
		
	}

	public void DisplayGraph()
	{
		/*
		int counter = 0;
		// Get number of values in list
		//int points = someList.Count;
	
		GetValues();

		while(counter < points)
		{
			//Then draw these points

			//Finally, draw
			//DrawColumns(someList[counter]);

			counter ++;
		}
		*/
	}

	public void DrawColumns(GraphData.Entry entry, int graphic)
	{
		//Animations can be applied to these through script
		if (graphic == 0)
		{
			x1Graphic.fillAmount = entry.insertCost;
			y1Graphic.fillAmount = entry.substituteCost;
			z1Graphic.fillAmount = entry.deleteCost;
		}
		else if (graphic == 1)
		{
			x2Graphic.fillAmount = entry.insertCost;
			y2Graphic.fillAmount = entry.substituteCost;
			z2Graphic.fillAmount = entry.deleteCost;
		}
		else if (graphic == 2)
		{
			x3Graphic.fillAmount = entry.insertCost;
			y3Graphic.fillAmount = entry.substituteCost;
			z3Graphic.fillAmount = entry.deleteCost;
		}
	}

	public void GetValues ()
	{
		// Get number of values in list
		int points = GraphData.numEntries;
		int counter = 0;

		//Requires day, month, year - use datetime
		if (showByDay)
		{

		}
		//Requires month, year - use datetime
		else if(showByMonth)
		{
			// month & year +/-1
			int MonthBefore = GetMonth("na");
			int MonthAfter = GetMonth("na");



		}
		//Requires year, simplest to do this explicity
		else if(showByYear)
		{
			List<GraphData.Entry> weightingsPreviousYear = new List<GraphData.Entry>();
			List<GraphData.Entry> weightingsCurrentYear = new List<GraphData.Entry>();
			List<GraphData.Entry> weightingsFutureYear = new List<GraphData.Entry>();

			while(counter < points)
			{
				/*
				if(GraphData.database[counter].year == year - 1)
				{
					weightingsPreviousYear.Add(GraphData.database[counter]);
				}
				else if(GraphData.database[counter].year == year)
				{
					weightingsCurrentYear.Add(GraphData.database[counter]);
				}
				else if(GraphData.database[counter].year == year + 1)
				{
					weightingsFutureYear.Add(GraphData.database[counter]);
				}
				*/

				counter ++;
			}

			//Finally, get the averages for each of these lists
		}
	}
		
	public void GetAverage(List<GraphData.Entry> weightingsTemp, List<GraphData.Entry> weightingsToAdd)
	{
		//Already have the list of values that suit

		int counter = 0;
		// Get number of values in list
		int numEntries = weightingsTemp.Count;

		//temp variables
		float averageX = 0f;
		float averageY = 0f;
		float averageZ = 0f;

		while(counter < numEntries)
		{
			// Add these values onto the totals
			averageX += weightingsTemp[counter].insertCost;
			averageY += weightingsTemp[counter].substituteCost;
			averageZ += weightingsTemp[counter].deleteCost;

			counter ++;
		}

		counter = 0;

		//Make the new entry with averaged values
		weightingsToAdd.Add(new GraphData.Entry(0, 0, 0, DateTime.Today, (averageX / numEntries), 
							(averageY / numEntries), 
							(averageZ / numEntries)));
	}

	public int GetMonth(string dateToUse)
	{
		//initialise
		int monthToGet = 0;

		return monthToGet;
	}

	public void SetDay(int setDay){ day = setDay;}
	public void SetMonth(int setMonth){month = setMonth;}
	public void SetYear(int setYear){year = setYear;}
}
