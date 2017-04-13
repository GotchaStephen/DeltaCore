using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;


namespace DeltaCoreBE
{

    public class MatchScore
    {
        private static void localLog(string topic, string msg)
        {
            if (debugOn)
            {
                string logEntry = String.Format("{0:F}: [{1}]{2}", DateTime.Now, topic, msg);
                UnityEngine.Debug.Log(logEntry);
            }
        }
        private static bool debugOn = false ;

        // This was used when loading solution from Database
        // public static GotchaDB gotchaDBServer = new GotchaDB();

        private const float DEFAULT_COST_INSERT = 0.2f ; 
        private const float DEFAULT_COST_DELETE = 0.2f ; 
        private const float DEFAULT_WEIGHT_DISTANCE = 7 ; 
        private const float DEFAULT_WEIGHT_CONFIDENCE = 0; 
        private const float DEFAULT_WEIGHT_DIRECTION = 0;

        public float costInsert ;
        public float costDelete ;
        public float weightDistance ;
        public float weightConfidence ;
        public float weightDirection ; 
        
        private int LocalpositionScaling = 1;

        private int infValue = int.MaxValue ;

        private List<ScoreObject> playerPoints = new List<ScoreObject>();
        private List<ScoreObject> expertPoints = new List<ScoreObject>();

        // Martix Dimentsion populated after getting data 
        private int n = 0 ; //  # player points
        private int m = 0 ; //  # expert points
        private int o = 0 ; //  n + m 

        private float[,] substiteCostMatrix;
        private float[,] deleteCostMatrix;
        private float[,] insertCostMatrix;
        private float[,] theCostMatrix;

        private float insertAllCost = 0; //costNull
        private float deleteAllCost = 0;
        private float costCurrent = 0;
        private float scoreScalingConstant = 1000;

        public int numberOfSubstitutes;
        public int numberOfDeletes ;
        public int numberOfInserts ;

        public MatchScore()
        {
            insertAllCost = deleteAllCost = costCurrent = 0;
            n = m = o = 0;
            numberOfSubstitutes = numberOfDeletes = numberOfInserts = 0;
            playerPoints.Clear();
            expertPoints.Clear();
            setCosts();
        }

        private void reset()
        {
            insertAllCost = deleteAllCost = costCurrent = 0;
            n = m = o = 0;
            numberOfSubstitutes = numberOfDeletes = numberOfInserts = 0 ;
            playerPoints.Clear();
            expertPoints.Clear();
        }

        public void setCosts( 
            float cInsert = DEFAULT_COST_INSERT,
            float cDelete = DEFAULT_COST_DELETE,
            float wDistance = DEFAULT_WEIGHT_DISTANCE,
            float wConfidence = DEFAULT_WEIGHT_CONFIDENCE,
            float wDirection = DEFAULT_WEIGHT_DIRECTION
        )
        {
            // localLog("Before", cInsert.ToString() + cDelete.ToString() ); 
            costInsert = cInsert ;
            costDelete = cDelete ;
            weightDistance = wDistance ;
            weightConfidence = wConfidence ;
            weightDirection = wDirection ;
            // localLog("After", getCostString());
        }

        private float getScoreScalingVariable()
        {
            return insertAllCost + deleteAllCost;
        }


        private float getFinalScore()
        {
            return scoreScalingConstant * ((getScoreScalingVariable() - costCurrent) / getScoreScalingVariable());
        }

        private void populateSubstiteCostMatrix()
        {
            substiteCostMatrix = new float[n, m];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    substiteCostMatrix[i, j] = getSubstituationCost(playerPoints[i], expertPoints[j]);
                }
            }
        }
        private void populateDeleteCostMatrix()
        {
            deleteCostMatrix = new float[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                    {
                        deleteCostMatrix[i, j] = getDeleteCost(playerPoints[i]);
                        deleteAllCost += deleteCostMatrix[i, j];
                    }
                    else { deleteCostMatrix[i, j] = infValue; }
                }
            }
        }
        private void populateInsertCostMatrix()
        {
            insertCostMatrix = new float[m, m];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if (i == j)
                    {
                        insertCostMatrix[i, j] = getInsertCost(expertPoints[i]);
                        insertAllCost += insertCostMatrix[i, j];
                    }
                    else { insertCostMatrix[i, j] = infValue; }
                }
            }

        }
        private void populateTheCostMatrix()
        {
            localLog("populateTheCostMatrix", "Started");
            populateSubstiteCostMatrix();
            populateDeleteCostMatrix();
            populateInsertCostMatrix();

            o = n + m;
            localLog("populateTheCostMatrix", String.Format("{0} points found for player", n)) ;
            localLog("populateTheCostMatrix", String.Format("{0} points found for solution", m)) ;
            theCostMatrix = new float[o, o];
            float valueHolder = 0.0f;
            for (int i = 0; i < o; i++)
            {
                for (int j = 0; j < o; j++)
                {

                    if (i < n)
                    {
                        if (j < m)
                        { // Substitue Cost Matrix 
                            valueHolder = substiteCostMatrix[i, j];
                        }
                        else
                        { // Delete Cost Matrix
                            valueHolder = deleteCostMatrix[i, j - m];
                        }
                    }
                    else
                    {
                        if (j < m)
                        { // Insert Cost Matrix
                            valueHolder = insertCostMatrix[i - n, j];
                        }
                        else
                        { // Zero Cost Matrix 
                            valueHolder = 0;
                        }
                    }
                    theCostMatrix[i, j] = valueHolder;
                }
            }
            localLog("populateTheCostMatrix", "Completed");
        }

        private void calculateCostCurrent()
        {
            HungarianAlgorithm hungaryAlgorithm = new HungarianAlgorithm(theCostMatrix);
            int[] costResult = hungaryAlgorithm.Run();
            for (int i = 0; i < o; i++) {
                costCurrent += theCostMatrix[i, costResult[i]];
                // This is a Sub/Delete Path 
                if ( i < n )
                {
                    if (costResult[i] < m) { numberOfSubstitutes++; }
                    else { numberOfDeletes++; }
                }
                else // This is a Insert/Do Nothing Path 
                {
                    if(costResult[i] < m){ numberOfInserts++ ; }
                }
            }
            localLog("calculateCostCurrent", String.Format("cost calculated at {0}", costCurrent)) ;
            localLog("calculateCostCurrent", String.Format("final score calculated at {0}", getFinalScore())) ;
            // theCostMatrixtoCSV("TestingCostScore"); 
        }

        private void theCostMatrixtoCSV(string suffix = "noSuffux")
        {
            string fileName = @"I:\hypercube\Dropbox\Projects\BlackShepherdStudios\IT\ScoreTesting\theCostMatrix_" + suffix + ".csv";
            using (StreamWriter file = new StreamWriter(fileName))
            {
                for (int i = 0; i < o; i++)
                {
                    for (int j = 0; j < o; j++){ file.Write(theCostMatrix[i, j] + ","); }
                    file.Write(Environment.NewLine);
                }
            }
        }

        public float getScoreLocally(ArrayList userData, ArrayList solutionData)
        {
            reset(); 
            loadDataFromLocal(userData, solutionData);
            populateTheCostMatrix();
            calculateCostCurrent();
            localLog("Needed Actions", string.Format("I[{0}], S[{1}], D[{2}]", numberOfInserts, numberOfSubstitutes, numberOfDeletes));
            return getFinalScore();
        }

        private void loadDataFromLocal(ArrayList userData, ArrayList solutionData)
        {
            localLog("loadDataFromLocal", "Loading scores from user");
            foreach (MarkerData af in userData)
            {
                playerPoints.Add(new ScoreObject(af.position.x* LocalpositionScaling, af.position.y* LocalpositionScaling, (int) af.confidenceLevel, (int) af.orientation, (int)af.type));
            }

            localLog("loadDataFromLocal", "Loading scores from solution");
            foreach (MarkerData af in solutionData)
            {
                expertPoints.Add(new ScoreObject(af.position.x* LocalpositionScaling, af.position.y* LocalpositionScaling, (int)af.confidenceLevel, (int)af.orientation, (int)af.type));
            }

            // Loading Solutions points 
            n = playerPoints.Count;
            m = expertPoints.Count;
        }

        private float getDistanceCost(ScoreObject so1, ScoreObject so2)
        {
            float x1 = so1.LocationX;
            float y1 = so1.LocationY;
            float x2 = so2.LocationX;
            float y2 = so2.LocationY;
            // Add the Square root 
            return weightDistance * (float) Math.Sqrt (((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2))) ;
        }
        private float getConfidenceCost(ScoreObject so1, ScoreObject so2)
        {
            return weightConfidence * (Math.Abs(so1.Confidence - so2.Confidence));
        }
        private float getDirectionCost(ScoreObject so1, ScoreObject so2)
        {
            return weightDirection * (Math.Abs(so1.Direction - so2.Direction));
        }
        private float getInsertCost(ScoreObject so)
        {
            return costInsert * so.Confidence;
        }
        private float getDeleteCost(ScoreObject so)
        {
            return costDelete * so.Confidence;
        }
        private float getSubstituationCost(ScoreObject so1, ScoreObject so2)
        {
            return getDistanceCost(so1, so2) + getConfidenceCost(so1, so2) + getDirectionCost(so1, so2);
        }
    }
}