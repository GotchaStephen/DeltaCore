using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GraphData : MonoBehaviour {

	//All the values to be accessed for the graph being shown
	public static List<Entry> database = new List<Entry>();
	public static int numEntries = 0;

	public static float maxInsertCost = 0.8f;
	public static float maxSubstituteCost = 0.182534f  ;
	public static float maxDeleteCost = 1.4f;
    

	public struct Entry
	{

		//what else to include?
		public int userID;
		public string state;
		public int sampleID;
		public int verdict;
		public DateTime addedOn;

		// x + y + z add up to 1
		//These are set to 0 by default in case this day does no exist?
		public float insertCost;
		public float substituteCost;
		public float deleteCost;

		public Entry(int uid, string s, int sid, int ver, DateTime date, float x, float y, float z)
		{
			userID = uid;
			state = s;
			sampleID = sid;
			verdict = ver;
			addedOn = date;
			insertCost = x;
			substituteCost = y;
			deleteCost = z;
		}
	}

	void Start()
	{
		RequestData();
	}

	// Request this upon graph tab being clicked
	public void RequestData()
	{
		// Reset the list
		database.Clear();

		// Request data from server

		//For moment, use test data
		TestData();

		// Add all points to list
		//e.g. database.Add(new Entry(,,,,,,));

		//Finally, get number of total entries
		numEntries = database.Count;

	}

	public void TestData()
	{
        database.Add(new Entry(60, "NSW", 15, 1, DateTime.Parse(" 2017-04-01 01:49:21"), 0f, 0.121195f, 0.2f));
        database.Add(new Entry(60, "NSW", 16, 0, DateTime.Parse(" 2017-04-02 01:49:21"), 0.4f, 0.182534f, 0.8f));
        database.Add(new Entry(60, "NSW", 17, 1, DateTime.Parse(" 2017-04-03 01:49:21"), 0.6f, 0.121315f, 0.8f));
        database.Add(new Entry(60, "NSW", 18, 1, DateTime.Parse(" 2017-04-04 01:49:21"), 0.4f, 0.120993f, 0.2f));
        database.Add(new Entry(60, "NSW", 19, 1, DateTime.Parse(" 2017-04-05 01:49:21"), 0.2f, 0.122203f, 0.4f));
        database.Add(new Entry(60, "NSW", 23, 0, DateTime.Parse(" 2017-04-06 01:49:21"), 0.2f, 0.122319f, 0f));
        database.Add(new Entry(60, "NSW", 24, 2, DateTime.Parse(" 2017-04-07 01:49:21"), 0f, 0.122146f, 0.4f));
        database.Add(new Entry(60, "NSW", 25, 1, DateTime.Parse(" 2017-04-08 01:49:21"), 0f, 0.120966f, 0.4f));
        database.Add(new Entry(70, "VIC", 15, 0, DateTime.Parse(" 2017-04-01 01:49:21"), 0.4f, 0.121019f, 0.2f));
        database.Add(new Entry(70, "VIC", 16, 1, DateTime.Parse(" 2017-04-02 01:49:21"), 0.2f, 0.0644148f, 0f));
        database.Add(new Entry(70, "VIC", 17, 0, DateTime.Parse(" 2017-04-03 01:49:21"), 0.4f, 0.12103f, 0f));
        database.Add(new Entry(70, "VIC", 18, 2, DateTime.Parse(" 2017-04-04 01:49:21"), 0f, 0.120773f, 0f));
        database.Add(new Entry(70, "VIC", 19, 2, DateTime.Parse(" 2017-04-05 01:49:21"), 0.4f, 0.121522f, 0.2f));
        database.Add(new Entry(70, "VIC", 23, 1, DateTime.Parse(" 2017-04-06 01:49:21"), 0.6f, 0.121629f, 0f));
        database.Add(new Entry(70, "VIC", 24, 1, DateTime.Parse(" 2017-04-07 01:49:21"), 0.4f, 0.121594f, 0.2f));
        database.Add(new Entry(70, "VIC", 25, 0, DateTime.Parse(" 2017-04-08 01:49:21"), 0.8f, 0.120834f, 0f));
        database.Add(new Entry(80, "QLD", 15, 1, DateTime.Parse(" 2017-04-01 01:49:21"), 0.2f, 0.000246682f, 0.6f));
        database.Add(new Entry(80, "QLD", 16, 2, DateTime.Parse(" 2017-04-02 01:49:21"), 0.2f, 0.0029914f, 1f));
        database.Add(new Entry(80, "QLD", 17, 1, DateTime.Parse(" 2017-04-03 01:49:21"), 0f, 0.000301288f, 0.4f));
        database.Add(new Entry(80, "QLD", 18, 0, DateTime.Parse(" 2017-04-04 01:49:21"), 0.2f, 0.000194444f, 1.2f));
        database.Add(new Entry(80, "QLD", 19, 2, DateTime.Parse(" 2017-04-05 01:49:21"), 0f, 0.000583156f, 1f));
        database.Add(new Entry(80, "QLD", 23, 2, DateTime.Parse(" 2017-04-06 01:49:21"), 0f, 0.000628491f, 0.2f));
        database.Add(new Entry(80, "QLD", 24, 0, DateTime.Parse(" 2017-04-07 01:49:21"), 0.2f, 0.000559906f, 0f));
        database.Add(new Entry(80, "QLD", 25, 1, DateTime.Parse(" 2017-04-08 01:49:21"), 0f, 0.000176494f, 1.4f));

        /* old Data
        database.Add(new Entry(15, "NSW", 16, 0, DateTime.Today, 0f, 0.022043f, 0f));
		database.Add(new Entry(17, "QLD", 16, 2, DateTime.Today, 0.8f, 0.0406216f, 1.6f));
		database.Add(new Entry(16, "VIC", 16, 1, DateTime.Today, 1.6f, 0.0120743f, 0f));
		database.Add(new Entry(15, "NSW", 17, 0, DateTime.Today, 0f, 0.019543f, 0.8f));
		database.Add(new Entry(17, "QLD", 17, 2, DateTime.Today, 0.8f, 0.0381216f, 1.2f));
		database.Add(new Entry(16, "VIC", 17, 1, DateTime.Today, 0.4f, 0.0095743f, 0f));
		database.Add(new Entry(15, "NSW", 18, 0, DateTime.Today, 0.4f, 0.017043f, 1.2f));
		database.Add(new Entry(17, "QLD", 18, 2, DateTime.Today, 1.6f, 0.0356216f, 0.8f));
		database.Add(new Entry(16, "VIC", 18, 1, DateTime.Today, 0f, 0.0070743f, 0.4f));
        */
        print("database added");
	}
}
