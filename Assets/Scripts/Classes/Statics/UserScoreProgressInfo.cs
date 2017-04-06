using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DeltaCoreBE; 

public static class UserScoreProgressInfo
{
    private static bool debugOn = true;
    private static void localLog(string msg) { localLog("UserScoreProgressInfo", msg); }
    private static void localLog(string topic, string msg)
    {
        if (debugOn)
        {
            string logEntry = string.Format("{0:F}: [{1}]{2}", System.DateTime.Now, topic, msg);
            Debug.Log(logEntry);
        }
    }
    /*
    private const int stage2Value = 350;
    private const int stage3Value = 550;
    private const int completedValue = 650;
    private const int perfectedValue = 750;

    public static DeltaCore.LevelProgress progress ;
    public static DeltaCore.LevelProgress getLevelProgress() { return progress; }

    public static bool canSubmit;

    public static float currentScore;
    public static float pastScore;

    public static int numberOfInserts ;
    public static int neededInserts ;
    public static int neededDeletes ;
    public static int neededSubstituations ;
    public static int pastNeededInserts ;
    public static int pastNeededDeletes ;
    public static int pastNeededSubstituations ;

    public static string pastActionHint;
    public static string nextActionHint;

    public static ArrayList solutionMarkers;
    public static MatchScore scoreCalculator;
    static UserScoreProgressInfo()
    {
        canSubmit = false ;
        currentScore = 0 ;
        numberOfInserts = 0;
        progress = DeltaCore.LevelProgress.NotAttempted;

        currentScore = 0.0f;
        pastScore = 0.0f;

        neededInserts = -1;
        neededDeletes = -1;
        neededSubstituations = -1;
        pastNeededInserts = -1;
        pastNeededDeletes = -1;
        pastNeededSubstituations = -1;

        pastActionHint = "No Past Action";
        nextActionHint = "No Next Action";

        solutionMarkers = new ArrayList(); ;
        scoreCalculator = new MatchScore();
    }

    private void updatePastActionHint()
    {

        if (pastNeededSubstituations == 0 && neededSubstituations == 1)
        {
            pastActionHint = string.Format("Congradulations on your first correct point, Keep Going Robocop believes in you");
        }
        else if (pastNeededSubstituations == (neededSubstituations - 1))
        {
            pastActionHint = string.Format("Another great find, keep going you are a natural");
        }
        else if (neededSubstituations == (pastNeededSubstituations - 1))
        {
            pastActionHint = string.Format("Why did you remove that one ? that was a correct feature");
        }
        else if (pastNeededDeletes == (neededDeletes - 1))
        {
            pastActionHint = string.Format("Oops, I could not detect a feature at that point");
        }
        else if (neededDeletes == (pastNeededDeletes - 1))
        {
            pastActionHint = string.Format("good spot, that wasnt a feature");
        }

        else
        {
            pastActionHint = string.Format(" Before I:{0}, S:{1}, D{2}\n After I:{3}, S:{4}, D{5}",
            pastNeededInserts, pastNeededSubstituations, pastNeededDeletes,
            neededInserts, neededSubstituations, neededDeletes);
        }
    }

    public static void setLevelProgress()
    {
        if (currentScore > completedValue)
        {
            canSubmit = true;
            if (currentScore > perfectedValue) { progress = DeltaCore.LevelProgress.Perfected; }
            else { progress = DeltaCore.LevelProgress.Completed; }
        }
        else if (currentScore > stage3Value) { progress = DeltaCore.LevelProgress.Threshold3; }
        else if (currentScore > stage2Value) { progress = DeltaCore.LevelProgress.Threshold2; }
        else { progress = DeltaCore.LevelProgress.Threshold1; }
        localLog("Level", string.Format("[Score:{0}][Progress:{1}]", currentScore, progress.ToString()));
    }

    public static void processAction(ArrayList markers)
    {
        // update new Score 
        pastScore = currentScore;
        currentScore = scoreCalculator.getScoreLocally(markers, solutionMarkers);
        updateNeededActions();
        updateActionHint();
    }
    */
}
