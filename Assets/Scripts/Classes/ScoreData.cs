using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DeltaCoreBE;

public class ScoreData
{

    private static bool debugOn = true;
    private static void localLog(string msg) { localLog("ScoreData", msg);  }
    private static void localLog(string topic, string msg )
    {
        if (debugOn)
        {
            string logEntry = string.Format("{0:F}: [{1}]{2}", System.DateTime.Now, topic, msg);
            Debug.Log(logEntry);
        }
    }

    public float currentScore ;
    public float pastScore ; 
    public bool scoreSet;
    public MatchScore scoreCalculator; 

    public int neededInserts ;
    public int neededDeletes ;
    public int neededSubstituations ;
    public int pastNeededInserts;
    public int pastNeededDeletes;
    public int pastNeededSubstituations;

    public string pastActionHint;
    public string nextActionHint;

    public ArrayList solutionMarkers ;
    public float solutionThreshold;
    public float solutionclose ;
    public ScoreData()
    {
        currentScore = 0.0f;
        pastScore = 0.0f;

        neededInserts = -1 ;
        neededDeletes = -1 ;
        neededSubstituations = -1 ;
        pastNeededInserts = -1 ;
        pastNeededDeletes = -1 ;
        pastNeededSubstituations = -1 ;

        pastActionHint = "No Past Action" ;
        nextActionHint = "No Next Action" ;

        scoreSet = false;
        solutionMarkers = new ArrayList();
        scoreCalculator = new MatchScore();
        solutionThreshold = 0.9f;
        solutionclose = 0.7f;
    }

    public int getScoreThreshold() { return (int)(solutionThreshold * solutionMarkers.Count); }

    public int getScoreClose(){ return (int)(solutionclose * solutionMarkers.Count); }


    private void updatePastActionHint()
    {
        
        if ( pastNeededSubstituations == 0 && neededSubstituations == 1)
        {
            pastActionHint = string.Format("Congradulations on your first correct point, Keep Going Robocop believes in you");
        }
        else if ( pastNeededSubstituations == (neededSubstituations - 1))
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

    private void updateNextActionHint()
    {
        if (neededInserts == solutionMarkers.Count && pastNeededDeletes == 0 && neededSubstituations == 0)
        {
            nextActionHint = string.Format("How many features can you find on this print ? (HINT: you need at least {0} to pass)", getScoreThreshold());
        }
        else
        {
            nextActionHint = string.Format(" Before I:{0}, S:{1}, D{2}\n After I:{3}, S:{4}, D{5}",
                pastNeededInserts, pastNeededSubstituations, pastNeededDeletes,
                neededInserts, neededSubstituations, neededDeletes);
        }
    }
    public void clearsolutionMarkers() { solutionMarkers.Clear() ; }

    private void updateActionHint()
    {
        updatePastActionHint();
        updateNextActionHint();
    }

    private void updateNeededActions()
    {
        pastNeededInserts = neededInserts;
        pastNeededSubstituations = neededSubstituations;
        pastNeededDeletes = neededDeletes;

        neededInserts = scoreCalculator.numberOfInserts;
        neededSubstituations = scoreCalculator.numberOfSubstitutes;
        neededDeletes = scoreCalculator.numberOfDeletes;

        localLog("Score", string.Format("Before I:{0}, S:{1}, D{2}\n After I:{3}, S:{4}, D{5}", pastNeededInserts, pastNeededSubstituations, pastNeededDeletes,
            neededInserts, neededSubstituations, neededDeletes));
    }

    public void processAction(ArrayList markers)
    {
        // update new Score 
        scoreSet = true ;
        pastScore = currentScore;
        currentScore = scoreCalculator.getScoreLocally(markers, solutionMarkers) ;
        updateNeededActions();
        updateActionHint();
    }

    public int getLevelScore() { return (int)currentScore; }

}
