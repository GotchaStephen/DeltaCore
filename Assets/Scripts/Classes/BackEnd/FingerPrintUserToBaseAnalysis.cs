using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DeltaCoreBE
{

    public class FingerPrintUserToBaseAnalysis
    {
        private static bool debugOn = true;
        private static void localLog(string msg) { localLog("FPU2BA", msg); }
        private static void localLog(string topic, string msg)
        {
            if (debugOn)
            {
                string logEntry = string.Format("{0:F}: [{1}]{2}", System.DateTime.Now, topic, msg);
                Console.WriteLine(logEntry);
                Debug.Log(logEntry);
            }
        }

        protected List<FingerPrintAnalysisPoint> userPoints = new List<FingerPrintAnalysisPoint>();
        protected List<FingerPrintAnalysisPoint> basePoints = new List<FingerPrintAnalysisPoint>();

        // Martix Dimentsion populated after getting data 
        protected int userPointCount = 0;
        protected int basePointCount = 0;
        protected int totalPointCount = 0;
        protected float insertAllCost = 0; //costNull
        protected float deleteAllCost = 0;

        protected float[,] costMatrix;
        protected int[] optimumPathIndex;

        public FingerPrintUserToBaseAnalysis()
        {
            reset();
        }
        public FingerPrintUserToBaseAnalysis(List<MarkerData> userData, List<FingerPrintAnalysisPoint> baseData)
        {
            reset();
            loadUserData(userData);
            loadBaseData(baseData);
            generateAnalysisInfo();
        }
        public FingerPrintUserToBaseAnalysis(List<AppliedFeature> userData, List<FingerPrintAnalysisPoint> baseData)
        {
            reset();
            loadUserData(userData);
            loadBaseData(baseData);
            generateAnalysisInfo();
        }
        public FingerPrintUserToBaseAnalysis(List<FingerPrintAnalysisPoint> userData, List<FingerPrintAnalysisPoint> baseData)
        {
            reset();
            loadUserData(userData);
            loadBaseData(baseData);
            generateAnalysisInfo();
        }
        public FingerPrintUserToBaseAnalysis(List<AppliedFeature> userData, List<FingerPrintConsensusPoint> baseData)
        {
            reset();
            loadUserData(userData);
            loadBaseData(baseData);
            generateAnalysisInfo();
        }
        public FingerPrintUserToBaseAnalysis(List<FingerPrintAnalysisPoint> userData, List<FingerPrintConsensusPoint> baseData)
        {
            reset();
            loadUserData(userData);
            loadBaseData(baseData);
            generateAnalysisInfo();
        }
        private void reset()
        {
            resetUserData();
            resetBaseData();
        }
        protected virtual void resetUserData()
        {
            userPoints.Clear();
            userPointCount = 0;
        }
        private void resetBaseData()
        {
            basePoints.Clear();
            basePointCount = 0;
        }


        private void costMatrixtoCSV(string suffix = "")
        {
            string dirName = "I:/hypercube/Dropbox/Projects/BlackShepherdStudios/IT/ScoreTesting";
            string fileName = dirName + "/" + string.Format("{0:HH_m_ss}_P2E_{1}.csv", DateTime.Now, suffix);
            using (StreamWriter file = new StreamWriter(fileName))
            {
                for (int i = 0; i < totalPointCount; i++)
                {
                    for (int j = 0; j < totalPointCount; j++) { file.Write(costMatrix[i, j] + ","); }
                    file.Write(Environment.NewLine);
                }
            }
        }

        protected void generateAnalysisInfo()
        {
            totalPointCount = basePointCount + userPointCount;
            costMatrix = new float[totalPointCount, totalPointCount];
            localLog(String.Format("T{0},P{1}, S{2}", totalPointCount, userPointCount, basePointCount));
            FingerPrintAnalysisFunctions.getCostMatrix(ref costMatrix, userPoints, basePoints, true);
            FingerPrintAnalysisFunctions.getOptimumPathIndex(costMatrix, ref optimumPathIndex);
            insertAllCost = FingerPrintAnalysisFunctions.getInsertAllCost(basePoints);
            deleteAllCost = FingerPrintAnalysisFunctions.getdeleteAllCost(userPoints);
        }

        protected void generateCustomAnalysisInfo<T>(List<T> userDefinedBaseData) where T : FingerPrintAnalysisPoint
        {
            reloadBaseData(userDefinedBaseData);
            generateAnalysisInfo();
        }
        private void loadUserData(List<MarkerData> userData)
        {
            foreach (MarkerData md in userData)
            {
                userPoints.Add(new FingerPrintAnalysisPoint(md));
            }
            userPointCount = userPoints.Count;
            totalPointCount = basePointCount + userPointCount;
        }
        private void loadUserData(List<AppliedFeature> userData)
        {
            foreach (AppliedFeature af in userData)
            {
                userPoints.Add(new FingerPrintAnalysisPoint(af.LocationX, af.LocationY, af.Confidence, af.Direction, af.FeatureID));
            }
            userPointCount = userPoints.Count;
            totalPointCount = basePointCount + userPointCount;
        }

        private void loadUserData<T>(List<T> userData) where T : FingerPrintAnalysisPoint
        {
            foreach (FingerPrintAnalysisPoint cso in userData) { userPoints.Add(cso); }
            userPointCount = userPoints.Count;
            totalPointCount = basePointCount + userPointCount;
        }
        private void loadBaseData<T>(List<T> baseData) where T : FingerPrintAnalysisPoint
        {
            foreach (FingerPrintAnalysisPoint cso in baseData)
            { basePoints.Add(cso); }
            basePointCount = basePoints.Count;
            totalPointCount = basePointCount + userPointCount;
        }

        public void reloadUserData(List<MarkerData> userData)
        {
            resetUserData();
            loadUserData(userData);
            generateAnalysisInfo();
        }
        public void reloadUserData(List<AppliedFeature> userData)
        {
            resetUserData();
            loadUserData(userData);
            generateAnalysisInfo();
        }
        public void reloadUserData<T>(List<T> userData) where T : FingerPrintAnalysisPoint
        {
            resetUserData();
            loadUserData(userData);
            generateAnalysisInfo();
        }
        public void reloadBaseData<T>(List<T> baseData) where T : FingerPrintAnalysisPoint
        {
            resetBaseData();
            loadBaseData(baseData);
            generateAnalysisInfo();
        }

        public void printUserData()
        {
            foreach(FingerPrintAnalysisPoint fpAp in userPoints)
            {
                localLog(String.Format("{0}", fpAp.ToString())); 
            }
        }
        public void printBaseData()
        {
            foreach (FingerPrintAnalysisPoint fpAp in basePoints)
            {
                localLog(String.Format("{0}", fpAp.ToString()));
            }
        }
        public virtual void printSummary()
        {
            localLog(String.Format("User {0} Points, Base {1} Points", userPointCount, basePointCount));
        }
    }


    public class FingerPrintPlayerToSolutionAnalysis : FingerPrintUserToBaseAnalysis
    {
        private bool debugOn = true;
        private void localLog(string msg) { localLog("FingerPrintPlayerToSolutionAnalysis", msg); }
        private void localLog(string topic, string msg)
        {
            if (debugOn)
            {
                string logEntry = string.Format("{0:F}: [{1}]{2}", System.DateTime.Now, topic, msg);
                Console.WriteLine(logEntry);
                Debug.Log(logEntry);
            }
        }

        public FingerPrintPlayerToSolutionAnalysis(List<MarkerData> userData, List<FingerPrintAnalysisPoint> baseData) : base(userData, baseData)
        {
            localLog("Staring"); 
            calculatePathCosts();
        }
        public FingerPrintPlayerToSolutionAnalysis(List<AppliedFeature> userData, List<FingerPrintAnalysisPoint> baseData) : base(userData, baseData)
        {
            calculatePathCosts();
        }
        public FingerPrintPlayerToSolutionAnalysis(List<AppliedFeature> userData, List<FingerPrintConsensusPoint> baseData) : base(userData, baseData)
        {
            calculatePathCosts();
        }

        private int numberOfSubstitutes;
        public int NumberOfSubstitutes { get { return numberOfSubstitutes; } }

        private int numberOfDeletes;
        public int NumberOfDeletes { get { return numberOfDeletes; } }

        private int numberOfInserts;
        public int NumberOfInserts { get { return numberOfInserts; } }

        private float costOfSubstitutes;
        public float CostOfSubstitutes { get { return costOfSubstitutes; } }

        private float costOfDeletes;
        public float CostOfDeletes { get { return costOfDeletes; } }

        private float costOfInserts;
        public float CostOfInserts { get { return costOfInserts; } }

        private float getHighestCost() { return insertAllCost + deleteAllCost; }
        private float getOptimumPathCost() { return costOfInserts + costOfSubstitutes + costOfDeletes; }


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

        protected override void resetUserData()
        {
            base.resetUserData();
            insertAllCost = deleteAllCost = 0;
            costOfInserts = costOfSubstitutes = costOfDeletes = 0.0f;
            numberOfSubstitutes = numberOfDeletes = numberOfInserts = 0;
        }

        private void calculatePathCosts()
        {
            for (int i = 0; i < this.totalPointCount; i++)
            {
                float tempCost = costMatrix[i, optimumPathIndex[i]];
                // This is a Sub/Delete Path 
                if (i < userPointCount)
                {
                    if (optimumPathIndex[i] < basePointCount)
                    {
                        // This is a Sub Path 
                        numberOfSubstitutes++;
                        costOfSubstitutes += tempCost;
                    }
                    else
                    {
                        // This is a Delete Path 
                        numberOfDeletes++;
                        costOfDeletes += tempCost;
                    }
                }
                else // This is a Insert/Do Nothing Path 
                {
                    if (optimumPathIndex[i] < basePointCount)
                    {
                        // This is a Insert Path
                        numberOfInserts++;
                        costOfInserts += tempCost;
                    }
                }
            }

            localLog(String.Format("Cost breakdown [I:{0}][S:{1}][D:{2}]", costOfInserts, costOfSubstitutes, costOfDeletes));
            localLog(String.Format("Point breakdown  [I:{0}][S:{1}][D:{2}]", numberOfInserts, numberOfSubstitutes, numberOfDeletes));
        }
        public float getScore()
        {
            return FingerPrintAnalysisFunctions.DEFAULT_SCORE_SCALING * ((getHighestCost() - getOptimumPathCost()) / getHighestCost());
        }

        public void reloadPlayerData(List<MarkerData> playerData)
        {
            pastNumberOfSubstitutes = NumberOfSubstitutes ;
            pastNumberOfDeletes = NumberOfDeletes ;
            pastNumberOfInserts = NumberOfInserts ;

            pastCostOfInserts = CostOfInserts ;
            pastCostOfSubstitutes = CostOfSubstitutes;
            pastCostOfDeletes = CostOfDeletes;

            resetUserData();
            reloadUserData(playerData);
            calculatePathCosts();
        }

        public override void printSummary()
        {
            base.printSummary();
            localLog(String.Format("Cost Values [I:{0}] [S:{1}] [D:{2}]", CostOfInserts, costOfSubstitutes, CostOfDeletes));
            localLog(String.Format("Cost Points [I:{0}] [S:{1}] [D:{2}]", NumberOfInserts, NumberOfSubstitutes, NumberOfDeletes));
        }
    }

    public class FingerPrintExpertToConsensusAnalysis : FingerPrintUserToBaseAnalysis
    {
        private static bool debugOn = true;
        private static void localLog(string msg) { localLog("FPE2CA", msg); }
        private static void localLog(string topic, string msg)
        {
            if (debugOn)
            {
                string logEntry = string.Format("{0:F}: [{1}]{2}", System.DateTime.Now, topic, msg);
                Console.WriteLine(logEntry);
            }
        }

        public List<FingerPrintConsensusPoint> consensusPoints = new List<FingerPrintConsensusPoint>();
        protected int consensusPointCount;
        protected int maxPointWeight = 0;
        protected int thresholdPointWeight = 0;

        public FingerPrintExpertToConsensusAnalysis()
        {
            consensusPointCount = 0;
        }

        private void updateConsensusCount()
        {
            consensusPointCount = consensusPoints.Count;
            totalPointCount = consensusPointCount + userPointCount;
        }

        private void createConsensus(List<AppliedFeature> userAnalysis)
        {
            consensusPoints.Clear();
            basePoints.Clear();
            foreach (AppliedFeature af in userAnalysis)
            {
                consensusPoints.Add(new FingerPrintConsensusPoint(af));
                basePoints.Add(new FingerPrintConsensusPoint(af));
            }
            consensusPointCount = consensusPoints.Count;
            basePointCount = basePoints.Count;
        }
        private void addAnalysisToConsensus(List<AppliedFeature> userAnalysis)
        {
            reloadUserData(userAnalysis);
            reloadBaseData(consensusPoints);

            for (int costMatrixIndex = 0; costMatrixIndex < totalPointCount; costMatrixIndex++)
            {
                // localLog(String.Format("costMatrixIndex:{0}", costMatrixIndex));
                int pathIndex = optimumPathIndex[costMatrixIndex];

                // This is a Sub/Delete Path 
                // No Action Needed for Delete 
                if (costMatrixIndex < userPointCount)
                {
                    if (pathIndex < basePointCount)
                    {
                        consensusPoints[pathIndex].averageOutWithPoint(userPoints[costMatrixIndex]);
                    }
                    else
                    {
                        consensusPoints.Add(new FingerPrintConsensusPoint(userPoints[costMatrixIndex]));
                    }
                }
                else // This is a Insert/Do Nothing Path 
                {
                    if (pathIndex < basePointCount) { }
                }
            }
        }
        public void updateConsensus(List<AppliedFeature> userAnalysis)
        {
            if (consensusPoints.Count == 0) { createConsensus(userAnalysis); }
            else { addAnalysisToConsensus(userAnalysis); }
        }

        private void getThreshold()
        {
            foreach (FingerPrintConsensusPoint cso in consensusPoints)
            {
                if (cso.Weight > maxPointWeight) { maxPointWeight = cso.Weight; }
            }
            thresholdPointWeight = (int)Math.Ceiling(maxPointWeight / 2.0);
        }
        public void FilterConsensusPointOnThreshold()
        {
            getThreshold();
            foreach (FingerPrintConsensusPoint cso in consensusPoints)
            {
                if (cso.Weight >= thresholdPointWeight) { cso.Active = true; }
            }
        }

        public List<FingerPrintConsensusPoint> getfilteredConsensus()
        {
            return new List<FingerPrintConsensusPoint>(from cso in consensusPoints where cso.Active == true select cso);
        }

        public void printConsensusPoints()
        {
            foreach (FingerPrintConsensusPoint cso in consensusPoints) { localLog(cso.ToString()); }
        }

        public void printSummary()
        {
            localLog(string.Format("Consensus now has {0} points.[Max_weight:{1}][Threshold{2}]", consensusPointCount, maxPointWeight, thresholdPointWeight));
        }
    }
}