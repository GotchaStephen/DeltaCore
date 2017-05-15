using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeltaCoreBE
{
    static class TypeConverterExtensions
    {
        /// Convert MarkerData ArrayList to List.
        public static List<MarkerData> ToList<MarkerData>(this ArrayList arrayList)
        {
            List<MarkerData> list = new List<MarkerData>(arrayList.Count);
            foreach (MarkerData md in arrayList)
            {
                list.Add(md);
            }
            return list;
        }
    }

    public class FingerPrintTrainingGameManager
    {
        public enum UserPlayAction { NoAction, FirstCorrectInsert, CorrectInsert, IncorrectInsert, CorrectDelete, IncorrectDelete, Unknown}
        public enum LevelProgress { Stage1, Stage2, Stage3, Completed, Perfected }

        private const int stage2Value = 350;
        private const int stage3Value = 550;
        private const int completedValue = 650;
        private const int perfectedValue = 750;


        private static bool debugOn = true;
        private static void localLog(string msg) { localLog("FingerPrintTrainingGameManager", msg); }
        private static void localLog(string topic, string msg)
        {
            if (debugOn)
            {
                string logEntry = string.Format("{0:F}:[{1}] {2}", System.DateTime.Now, topic, msg);
                Console.WriteLine(logEntry);
                Debug.Log(logEntry);
            }
        }
        private UserPlayAction lastUserAction;
        public UserPlayAction LastUserAction { get { return lastUserAction ;} }

        private LevelProgress currentLevelProgress;
        public LevelProgress CurrentLevelProgress { get { return currentLevelProgress; } }

        public bool canSubmit; 
        #region past Variables
        private int pastNumberOfSubstitutes;
        public int PastNumberOfSubstitutes { get { return pastNumberOfSubstitutes; } }

        private int pastNumberOfDeletes;
        public int PastNumberOfDeletes { get { return pastNumberOfDeletes; } }

        private int pastNumberOfInserts;
        public int PastNumberOfInserts { get { return pastNumberOfInserts; } }

        private float pastCostOfSubstitutes;
        public float PastCostOfSubstitutes { get { return pastCostOfSubstitutes; } }

        private float pastCostOfDeletes;
        public float PastCostOfDeletes { get { return pastCostOfDeletes; } }

        private float pastCostOfInserts;
        public float PastCostOfInserts { get { return pastCostOfInserts; } }

        private float pastScore;
        public float PastScore { get { return pastScore; } }

        private float pastCost;
        public float PastCost { get { return pastCost; } }

        private float currentScore;
        public float CurrentScore { get { return currentScore; } }

        private float currentCost;
        public float CurrentCost { get { return currentCost; } }
        #endregion

        private string pastActionText;

        private FingerPrintPlayerToSolutionAnalysis fpP2S;
        public FingerPrintTrainingGameManager(ArrayList userData, List<FingerPrintAnalysisPoint> solutionData)
        {
            lastUserAction = UserPlayAction.NoAction ;
            currentLevelProgress = LevelProgress.Stage1;
            canSubmit = false;
            pastNumberOfSubstitutes = -1 ;
            pastNumberOfDeletes = -1;
            pastNumberOfInserts = -1;
            pastCostOfInserts = -1;
            pastCostOfSubstitutes = -1;
            pastCostOfDeletes = -1 ;
            pastCost = -1;
            pastScore = -1;
            fpP2S = new FingerPrintPlayerToSolutionAnalysis(TypeConverterExtensions.ToList<MarkerData>(userData), solutionData);
            loadCurrentData(); 
        }

        private float getPastCost()
        {
            return pastCostOfSubstitutes + pastCostOfDeletes + pastCostOfInserts;
        }

        private void loadPastData()
        {
            pastNumberOfSubstitutes = fpP2S.NumberOfSubstitutes;
            pastNumberOfDeletes = fpP2S.NumberOfDeletes;
            pastNumberOfInserts = fpP2S.NumberOfInserts;
            pastCostOfInserts = fpP2S.CostOfInserts;
            pastCostOfSubstitutes = fpP2S.CostOfSubstitutes;
            pastCostOfDeletes = fpP2S.CostOfDeletes;
            pastCost = fpP2S.getOptimumPathCost() ;
            pastScore = fpP2S.getScore(); 
        }

        private void updatePastActionText()
        {
            switch (lastUserAction)
            {
                case UserPlayAction.NoAction :
                    pastActionText = "Good Luck!";
                    break;

                case UserPlayAction.FirstCorrectInsert:
                    pastActionText = "Congratulations on your first correct point! Keep going, Robocop believes in you";
                    break;

                case UserPlayAction.CorrectInsert:
                    pastActionText = "Another great find, keep at it, you are a natural";
                    break;

                case UserPlayAction.IncorrectDelete:
                    pastActionText = "Why did you remove that one? That was a correct feature";
                    break;
                case UserPlayAction.IncorrectInsert:
                    pastActionText = "Oops, I could not detect a feature at that point";
                    break;
                case UserPlayAction.CorrectDelete:
                    pastActionText = "Good spot, that wasn't a feature";
                    break;
                default:
                    pastActionText = "Unknown Unspecified Action";
                    break;
            }
        }

        private void updateTypeOfAction()
        {
            if (pastNumberOfSubstitutes == 0 && fpP2S.NumberOfSubstitutes == 1)
            {
                lastUserAction = UserPlayAction.FirstCorrectInsert ;
            }
            else if ( pastNumberOfSubstitutes == (fpP2S.NumberOfSubstitutes - 1))
            {
                lastUserAction = UserPlayAction.CorrectInsert;
            }
            else if (pastNumberOfSubstitutes == (fpP2S.NumberOfSubstitutes + 1))
            {
                lastUserAction = UserPlayAction.IncorrectDelete;
            }
            else if (pastNumberOfDeletes == (fpP2S.NumberOfDeletes + 1))
            {
                lastUserAction = UserPlayAction.CorrectDelete;
            }
            
            else if (pastNumberOfDeletes == (fpP2S.NumberOfDeletes - 1))
            {
                lastUserAction = UserPlayAction.IncorrectInsert;
            }
            else { lastUserAction = UserPlayAction.Unknown ; }
            updatePastActionText(); 
        }
        private void updateLevelProgress()
        {
            if (currentScore > completedValue)
            {
                canSubmit = true;
                if (currentScore > perfectedValue) { currentLevelProgress = LevelProgress.Perfected; }
                else { currentLevelProgress = LevelProgress.Completed; }
            }
            else if (currentScore > stage3Value) { currentLevelProgress = LevelProgress.Stage3; }
            else if (currentScore > stage2Value) { currentLevelProgress = LevelProgress.Stage2; }
            else { currentLevelProgress = LevelProgress.Stage1; }
        }

        private void loadCurrentData()
        {
            currentCost = fpP2S.getOptimumPathCost();
            currentScore = fpP2S.getScore();
            updateTypeOfAction();
            updateLevelProgress();
        }
        public void updatePlayerData(ArrayList userData)
        {
            loadPastData(); 
            fpP2S.reloadPlayerData(TypeConverterExtensions.ToList<MarkerData>(userData));
            loadCurrentData(); 
            printSummary(); 
        }

        public void printSummary()
        {
            localLog(String.Format("Past [I:{0}] [S:{1}] [D:{2}]", PastNumberOfInserts, PastNumberOfSubstitutes, PastNumberOfDeletes));
            localLog(String.Format("Current [I:{0}] [S:{1}] [D:{2}]", fpP2S.NumberOfInserts, fpP2S.NumberOfSubstitutes, fpP2S.NumberOfDeletes));
            localLog(String.Format("Past [C:{0}][S:{1}], Current [C:{2}][S:{3}]", PastCost, PastScore, currentCost, currentScore));
            localLog(String.Format("Last Action:{0}, Progress {1}", LastUserAction.ToString(), currentLevelProgress.ToString() ));
        }
    }
}