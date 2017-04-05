using System;
using System.Data;
namespace DeltaCoreBE
{

    public class AppliedFeature
    {
        private int featureID;
        private float locationX;
        private float locationY;
        private int confidence;
        private int phase;
        private int direction;
        private DateTime addedOn;

        public AppliedFeature() { }
        public AppliedFeature(int featureID, float locX, float locY, int ph, int conf, int dir)
        {
            this.featureID = featureID;
            this.locationX = locX;
            this.locationY = locY;
            this.confidence = conf;
            this.direction = dir;
            this.phase = ph;
        }

        public AppliedFeature(DataRow data)
        {
            featureID = Int32.Parse(data[GotchaConstants.APP_FTR_TBL_FTR_ID].ToString());
            locationX = Single.Parse(data[GotchaConstants.APP_FTR_TBL_LOC_X].ToString());
            locationY = Single.Parse(data[GotchaConstants.APP_FTR_TBL_LOC_Y].ToString());
            DateTime.TryParse(data[GotchaConstants.APP_FTR_TBL_AO].ToString(), out addedOn);
            confidence = Int32.Parse(data[GotchaConstants.APP_FTR_TBL_CONF].ToString());
            direction = Int32.Parse(data[GotchaConstants.APP_FTR_TBL_DIRECTION].ToString());
            if (data.Table.Columns.Contains(GotchaConstants.APP_FTR_TBL_PHS_ID))
            {
                phase = Int32.Parse(data[GotchaConstants.APP_FTR_TBL_PHS_ID].ToString());
            }
        }
        public override string ToString()
        {
            return String.Format("[[Loaction:({0},{1}):]\n[Confidence:{2}][Direction:{3}]", locationX, locationY, confidence, direction);
        }

        public int FeatureID
        {
            get { return featureID; }
            set { featureID = value; }
        }

        public int Confidence
        {
            get { return confidence; }
            set { confidence = value; }
        }
        public int Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public int Phase
        {
            get { return phase; }
            set { phase = value; }
        }

        public float LocationX
        {
            get { return locationX; }
            set { locationX = value; }
        }
        public float LocationY
        {
            get { return locationY; }
            set { locationY = value; }
        }

        public void setLocation(float x, float y)
        {
            locationX = x;
            locationY = y;
        }
    }
}