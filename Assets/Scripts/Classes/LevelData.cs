using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DeltaCoreBE;

public class LevelData
{
	//public static LevelData instance;	

    private static bool debugOn = true;
    private static void localLog(string msg) { localLog("LevelData", msg); }
    private static void localLog(string topic, string msg)
    {
        if (debugOn)
        {
            string logEntry = string.Format("{0:F}: [{1}]{2}", System.DateTime.Now, topic, msg);
            Debug.Log(logEntry);
        }
    }

    public FingerprintLevel level;
     // public ScoreData scoreData; // Need to remove and replace with class
    public ArrayList markers;
    public List<FingerPrintAnalysisPoint> solutionPoints;

    public string actionsLog;
    public string userNotes;
    public bool completed;
    public bool ready;
    
    public DeltaCore.AnalysisDecision decision;
    private DeltaCore.UserLevelAction lastLevelAction ;
    public DeltaCore.UserLevelAction LastLevelAction
    {
        get { return lastLevelAction; }
        set { lastLevelAction = value; }
    }

    public LevelData(FingerprintLevel level)
    {
        this.level = level;
        markers = new ArrayList();
        actionsLog = "";
        userNotes = "";
        solutionPoints = new List<FingerPrintAnalysisPoint>();
        LastLevelAction = DeltaCore.UserLevelAction.NoAction;
    }

    public void LogAction(string s)
    {
        actionsLog += s;
    }

    public void reset()
    {
        markers = new ArrayList();
        actionsLog = "";
        userNotes = "";
    }

    public void UpdateUserNotes(string newNotes) { userNotes = newNotes; }

    public void resetMarkers() { markers.Clear(); }
    public void clearsolutionPoints() { solutionPoints.Clear(); }

    public override string ToString()
    {
        return String.Format("CurrentLevel[{0}]", level);
    }
}
