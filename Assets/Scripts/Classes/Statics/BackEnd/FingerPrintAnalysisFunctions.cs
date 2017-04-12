using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeltaCoreBE
{
	static class FingerPrintAnalysisFunctions
	{
		private static bool debugOn = true;
		private static void localLog(string msg) { localLog("FingerPrintAnalysisFunctions", msg); }
		private static void localLog(string topic, string msg)
		{
			if (debugOn)
			{
				string logEntry = string.Format("{0:F}: [{1}]{2}", System.DateTime.Now, topic, msg);
				Console.WriteLine(logEntry);
			}
		}

		private const float DEFAULT_COST_INSERT = 0.2f;
		private const float DEFAULT_COST_DELETE = 0.2f;
		private const float DEFAULT_WEIGHT_DISTANCE = 7;
		private const float DEFAULT_WEIGHT_CONFIDENCE = 0;
		private const float DEFAULT_WEIGHT_DIRECTION = 0;
		private const float infValue = float.MaxValue;

		public const float DEFAULT_SCORE_SCALING = 1000;

		public static float costInsert;
		public static float costDelete;
		public static float weightDistance;
		public static float weightConfidence;
		public static float weightDirection;

		static FingerPrintAnalysisFunctions()
		{
			setCosts(); 
		}

		public static void setCosts(
			float cInsert = DEFAULT_COST_INSERT,
			float cDelete = DEFAULT_COST_DELETE,
			float wDistance = DEFAULT_WEIGHT_DISTANCE,
			float wConfidence = DEFAULT_WEIGHT_CONFIDENCE,
			float wDirection = DEFAULT_WEIGHT_DIRECTION
		)
		{
			// localLog("Before", cInsert.ToString() + cDelete.ToString() ); 
			costInsert = cInsert;
			costDelete = cDelete;
			weightDistance = wDistance;
			weightConfidence = wConfidence;
			weightDirection = wDirection;
			// localLog("After", getCostString());
		}

		public static float getInsertAllCost<T>(List<T> analysisList) where T : FingerPrintAnalysisPoint
		{
			float sum = 0;
			foreach (FingerPrintAnalysisPoint fp in analysisList){
				sum += fp.getInsertCost();
			}
			return sum ; 
		}
		public static float getdeleteAllCost<T>(List<T> analysisList) where T : FingerPrintAnalysisPoint
		{
			float sum = 0;
			foreach (FingerPrintAnalysisPoint fp in analysisList)
			{
				sum += fp.getDeleteCost();
			}
			return sum;
		}

		public static void getCostMatrix<T>(ref float[,] costMatrix, List<FingerPrintAnalysisPoint> List1, List<T> List2, bool writetoCSV = false ) where T : FingerPrintAnalysisPoint
		{
			localLog("Starting the Cost Matrix generation"); 
			int list1Size = List1.Count ; // n Player 
			int list2Size = List2.Count ; // m Expert 

			#region Generating InsertDeleteSubstitue Marticies 
			// Sub Matrix 
			float[,] substiteCostMatrix = new float[list1Size, list2Size];
			for (int i = 0; i < list1Size; i++)
			{
				for (int j = 0; j < list2Size; j++)
				{
					substiteCostMatrix[i, j] = List2[j].getSubstituationCost(List1[i]);
				}
			}

			// Delete Matrix
			float[,] deleteCostMatrix = new float[list1Size, list1Size];
			for (int i = 0; i < list1Size; i++)
			{
				for (int j = 0; j < list1Size; j++)
				{
					if (i == j)
					{
						deleteCostMatrix[i, j] = List1[i].getDeleteCost();
					}
					else { deleteCostMatrix[i, j] = infValue; }
				}
			}

			// insert Matrix
			float[,] insertCostMatrix = new float[list2Size, list2Size];
			for (int i = 0; i < list2Size; i++)
			{
				for (int j = 0; j < list2Size; j++)
				{
					if (i == j)
					{
						insertCostMatrix[i, j] = List2[i].getInsertCost();
					}
					else { insertCostMatrix[i, j] = infValue; }
				}
			}

			#endregion

			// Cost Matrix 
			int costMatrixSize = list1Size + list2Size; 
			costMatrix = new float[costMatrixSize, costMatrixSize];
			float valueHolder = 0.0f;
			for (int i = 0; i < costMatrixSize; i++)
			{
				for (int j = 0; j < costMatrixSize; j++)
				{

					if (i < list1Size)
					{
						if (j < list2Size)
						{ // Substitue Cost Matrix 
							valueHolder = substiteCostMatrix[i, j];
						}
						else
						{ // Delete Cost Matrix
							valueHolder = deleteCostMatrix[i, j - list2Size];
						}
					}
					else
					{
						if (j < list2Size)
						{ // Insert Cost Matrix
							valueHolder = insertCostMatrix[i - list1Size, j];
						}
						else
						{ // Zero Cost Matrix 
							valueHolder = 0;
						}
					}
					costMatrix[i, j] = valueHolder;
				}
			}
			if (writetoCSV)
			{
				localLog("Writing to CSV");
				string dirName = "I:/hypercube/Dropbox/Projects/BlackShepherdStudios/IT/ScoreTesting";
				string fileName = dirName + "/" + string.Format("{0:HH_m_ss}_P2E.csv", DateTime.Now);
				using (StreamWriter file = new StreamWriter(fileName))
				{
					for (int i = 0; i < costMatrixSize; i++)
					{
						for (int j = 0; j < costMatrixSize; j++) { file.Write(costMatrix[i, j] + ","); }
						file.Write(Environment.NewLine);
					}
				}
			}
		}

		public static void getOptimumPathIndex(float[,] costMatric, ref int[] optimumPathIndex)
		{
			HungarianAlgorithm hungaryAlgorithm = new HungarianAlgorithm(costMatric);
			optimumPathIndex = hungaryAlgorithm.Run();
		}
	}
}
