using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarGraphs : MonoBehaviour {

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

	//All the values to be accessed for the graph being shown
	public List<Weightings> weightingsDatabase = new List<Weightings>();
	//The database currently being displayed
	public List<Weightings> weightingsGraph = new List<Weightings>();

	public struct Weightings
	{
		// x + y + z add up to 1
		//These are set to 0 by default in case this day does no exist?
		public float x;
		public float y;
		public float z;

		//what else to include?? not using these for moment
		//Day of month
		// 01 - 31
		public int day;
		// 01 - 12
		public int month;
		// 2017 - etc
		public int year;

		public Weightings(float xVal, float yVal, float zVal, int dayVal, int monthVal, int yearVal)
		{
			x = xVal;
			y = yVal;
			z = zVal;
			day = dayVal;
			month = monthVal;
			year = yearVal;
		}
	}

	// Use this for initialization
	void Start () {
		
	}

	// Request this upon graph tab being clicked
	public void RequestData()
	{
		// Reset the list
		weightingsDatabase.Clear();

		// Request data from server

		// Add all points to list
		//e.g. weightingsDatabase.Add(new Weightings(0.3, 0.3, 0.4, 01, 01, 2017));

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

	public void DrawColumns(Weightings entry, int graphic)
	{
		//Animations can be applied to these through script
		if (graphic == 0)
		{
			x1Graphic.fillAmount = entry.x;
			y1Graphic.fillAmount = entry.y;
			z1Graphic.fillAmount = entry.z;
		}
		else if (graphic == 1)
		{
			x2Graphic.fillAmount = entry.x;
			y2Graphic.fillAmount = entry.y;
			z2Graphic.fillAmount = entry.z;
		}
		else if (graphic == 2)
		{
			x3Graphic.fillAmount = entry.x;
			y3Graphic.fillAmount = entry.y;
			z3Graphic.fillAmount = entry.z;
		}
	}

	public void GetValues ()
	{
		// Get number of values in list
		int points = weightingsDatabase.Count;
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
			List<Weightings> weightingsPreviousYear = new List<Weightings>();
			List<Weightings> weightingsCurrentYear = new List<Weightings>();
			List<Weightings> weightingsFutureYear = new List<Weightings>();

			while(counter < points)
			{
				if(weightingsDatabase[counter].year == year - 1)
				{
					weightingsPreviousYear.Add(weightingsDatabase[counter]);
				}
				else if(weightingsDatabase[counter].year == year)
				{
					weightingsCurrentYear.Add(weightingsDatabase[counter]);
				}
				else if(weightingsDatabase[counter].year == year + 1)
				{
					weightingsFutureYear.Add(weightingsDatabase[counter]);
				}

				counter ++;
			}

			//Finally, get the averages for each of these lists
		}
	}
		
	public void GetAverage(List<Weightings> weightingsTemp, List<Weightings> weightingsToAdd)
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
			averageX += weightingsTemp[counter].x;
			averageY += weightingsTemp[counter].y;
			averageZ += weightingsTemp[counter].z;

			counter ++;
		}

		counter = 0;

		//Make the new entry with averaged values
		weightingsToAdd.Add(new Weightings((averageX / numEntries), 
							(averageY / numEntries), 
							(averageZ / numEntries),
							0, 0, 0));
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
