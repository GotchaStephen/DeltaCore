using System;
using System.Data;
namespace DeltaCoreBE
{
	public class AnalysisPoint
	{
		private static bool debugOn = false;
		private static void localLog(string msg) { localLog("Class_AnalysisPoint", msg); }
		private static void localLog(string topic, string msg)
		{
			if (debugOn)
			{
				string logEntry = string.Format("{0:F}: [{1}]{2}", System.DateTime.Now, topic, msg);
				Console.WriteLine(logEntry);
			}
		}

		private float confidence;
		public float Confidence
		{
			get { return confidence; }
			set { confidence = value; }
		}
		private float direction;
		public float Direction
		{
			get { return direction; }
			set { direction = value; }
		}

		private float locationX;
		public float LocationX
		{
			get { return locationX; }
			set { locationX = value; }
		}

		private float locationY;
		public float LocationY
		{
			get { return locationY; }
			set { locationY = value; }
		}

		private int feature;
		public int Feature
		{
			get { return feature; }
			set { feature = value; }
		}

		public AnalysisPoint() { }
		public AnalysisPoint(float x, float y, float conf, float dir, int feat)
		{
			this.locationX = x;
			this.locationY = y;
			this.confidence = conf;
			this.direction = dir;
			this.feature = feat;
		}
		public override string ToString()
		{
			return String.Format("[Loc:({0},{1}):][C:{2}][D:{3}]", locationX, locationY, confidence, direction);
		}
	}

	public class AppliedFeature : AnalysisPoint
	{
		private int featureID;
		private int phase;
		private DateTime addedOn;

		public AppliedFeature() { }
		public AppliedFeature(int featureID, float locX, float locY, int ph, float conf, float dir) :base (locX, locY, conf, dir, featureID)
		{
			this.featureID = featureID;
			this.phase = ph;
		}

		public AppliedFeature(DataRow data)
		{
			featureID = Int32.Parse(data[GotchaConstants.APP_FTR_TBL_FTR_ID].ToString());
			LocationX = Single.Parse(data[GotchaConstants.APP_FTR_TBL_LOC_X].ToString());
			LocationY = Single.Parse(data[GotchaConstants.APP_FTR_TBL_LOC_Y].ToString());
			DateTime.TryParse(data[GotchaConstants.APP_FTR_TBL_AO].ToString(), out addedOn);
			Confidence = Single.Parse(data[GotchaConstants.APP_FTR_TBL_CONF].ToString());
			Direction = Single.Parse(data[GotchaConstants.APP_FTR_TBL_DIRECTION].ToString());
			if (data.Table.Columns.Contains(GotchaConstants.APP_FTR_TBL_PHS_ID))
			{
				phase = Int32.Parse(data[GotchaConstants.APP_FTR_TBL_PHS_ID].ToString());
			}
		}
		public override string ToString()
		{
			return String.Format("[Loc:({0},{1}):][C:{2}][D:{3}]", LocationX, LocationY, Confidence, Direction);
		}

		public int FeatureID
		{
			get { return Feature; }
			set { Feature = value; }
		}

		public int Phase
		{
			get { return phase; }
			set { phase = value; }
		}

		public void setLocation(float x, float y)
		{
			LocationX = x;
			LocationY = y;
		}
	}

	public class FingerPrintAnalysisPoint : AnalysisPoint
	{
		private static bool debugOn = true;
		private static void localLog(string msg) { localLog("FingerPrintAnalysisPoint", msg); }
		private static void localLog(string topic, string msg)
		{
			if (debugOn)
			{
				string logEntry = string.Format("{0:F}: [{1}]{2}", System.DateTime.Now, topic, msg);
				Console.WriteLine(logEntry);
			}
		}

		public FingerPrintAnalysisPoint() : base() { }
		public FingerPrintAnalysisPoint(float x, float y, float conf, float dir, int feat) : base(x, y, conf, dir, feat)
		{ }

		public FingerPrintAnalysisPoint(FingerPrintAnalysisPoint fpap)
		{
			this.LocationX = fpap.LocationX;
			this.LocationY = fpap.LocationY;
			this.Confidence = fpap.Confidence;
			this.Direction = fpap.Direction;
			this.Feature = fpap.Feature;
		}
		public FingerPrintAnalysisPoint(AppliedFeature af)
		{
			this.LocationX = af.LocationX;
			this.LocationY = af.LocationY;
			this.Confidence = af.Confidence;
			this.Direction = af.Direction;
			this.Feature = af.FeatureID;
		}

		public override string ToString()
		{
			return String.Format("[FingerPrint{0}]", base.ToString());
		}

		public float getInsertCost()
		{
			return FingerPrintAnalysisFunctions.costInsert * this.Confidence;
		}
		public float getDeleteCost()
		{
			return FingerPrintAnalysisFunctions.costDelete * this.Confidence;
		}
		public float getSubstituationCost(FingerPrintAnalysisPoint otherPoint)
		{
			return getDistanceWeight(otherPoint) + getConfidenceWeight(otherPoint) + getDirectionWeight(otherPoint);
		}


		private float getConfidenceWeight(FingerPrintAnalysisPoint otherPoint)
		{
			return FingerPrintAnalysisFunctions.weightConfidence * (Math.Abs(Confidence - otherPoint.Confidence));
		}
		private float getDirectionWeight(FingerPrintAnalysisPoint otherPoint)
		{
			return FingerPrintAnalysisFunctions.weightDirection * (Math.Abs(Direction - otherPoint.Direction));
		}
		private float getDistanceWeight(FingerPrintAnalysisPoint otherPoint)
		{
			float dist = (float)Math.Sqrt(
				Math.Pow(LocationX - otherPoint.LocationX, 2) +
				Math.Pow(LocationY - otherPoint.LocationY, 2)
			);
			return FingerPrintAnalysisFunctions.weightDistance * dist;
		}

	}
	public class FingerPrintConsensusPoint : FingerPrintAnalysisPoint
	{
		public FingerPrintConsensusPoint() { }
		public FingerPrintConsensusPoint(float x, float y, float conf, float dir, float feat) : base(x, y, (int)conf, (int)dir, (int)feat)
		{
			weight = 1;
			active = false;
		}
		public FingerPrintConsensusPoint(AppliedFeature af) : base(af)
		{
			weight = 1;
			active = false;
		}
		public FingerPrintConsensusPoint(FingerPrintAnalysisPoint fpap) : base(fpap)
		{
			weight = 1;
			active = false;
		}

		private int weight;
		public int Weight
		{
			get { return weight; }
			set { weight = value; }
		}

		private bool active;
		public bool Active
		{
			get { return active; }
			set { active = value; }
		}

		public void averageOutWithPoint(FingerPrintAnalysisPoint af)
		{
			// Get Average Distance
			this.LocationX = (this.LocationX + af.LocationX) / 2;
			this.LocationY = (this.LocationY + af.LocationY) / 2;

			// Get Average Confidence
			this.Confidence = (this.Confidence + af.Confidence) / 2;

			// Get Average direction
			if (this.Direction == -1) { this.Direction = af.Direction; }
			else if (af.Direction == -1) { /* Nothing Direction stays the same */ }
			else { this.Direction = (this.Direction + af.Direction) / 2; }

			// Get Average Feature 
			this.Feature = Math.Max(this.Feature, af.Feature);

			// Add weght 
			this.Weight++;
		}
		public override string ToString()
		{
			return String.Format("[Loc:({0},{1})][W:{2}][A:{3}]", LocationX, LocationY, Weight, Active);
		}
	}
}