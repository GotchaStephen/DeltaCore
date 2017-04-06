using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DeltaCoreBE;

public class LevelData
{

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


    private enum LevelProgress { Stage1, Stage2, Stage3, Completed, Perfected }
    private const int stage2Value = 350;
    private const int stage3Value = 550;
    // private const int completedValue = 650;
    private const int completedValue = 551;
    private const int perfectedValue = 750;

    public FingerprintLevel level;
    public ScoreData scoreData;
    public ArrayList markers;
    public bool loadedSolution;

    public string actionsLog;
    public string userNotes;

    public bool completed;
    public bool ready;
    public bool canSubmit;
    public int currentScore;
    public DeltaCore.AnalysisDecision decision;

    // Analytical Testing 
    private DeltaCore.UserLevelAction lastLevelAction;
    public DeltaCore.UserLevelAction LastLevelAction
    {
        get { return lastLevelAction; }
        set { lastLevelAction = value; }
    }

    private LevelProgress progress;
    //    public LevelProgress Progress
    //    {
    //        get { return progress; }
    //    }
    public void setLevelProgress()
    {
        if (currentScore > completedValue)
        {
            canSubmit = true;
            if (currentScore > perfectedValue) { progress = LevelProgress.Perfected; }
            else { progress = LevelProgress.Completed; }
        }
        else if (currentScore > stage3Value) { progress = LevelProgress.Stage3; }
        else if (currentScore > stage2Value) { progress = LevelProgress.Stage2; }
        else { progress = LevelProgress.Stage1; }
        localLog("Level", string.Format("[Score:{0}][Progress:{1}]", currentScore, progress.ToString()));
    }

    public LevelData(FingerprintLevel level)
    {
        this.level = level;
        markers = new ArrayList();
        actionsLog = "";
        userNotes = "";
        scoreData = new ScoreData();
        loadedSolution = false;
        canSubmit = false;
        progress = LevelProgress.Stage1;
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

    public void updateLevelData()
    {
        scoreData.processAction(markers);
        currentScore = (int)scoreData.currentScore;
        setLevelProgress();
    }

    public void resetMarkers() { markers.Clear(); }
}
