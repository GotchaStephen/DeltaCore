
using System;
using System.Data;
namespace DeltaCoreBE
{

    public class ScoreObject
    {
        private int confidence;
        private int direction;
        private int feature;
        private float locationX;
        private float locationY;

        public ScoreObject() { }

        public ScoreObject(float x, float y, int conf, int dir, int feat)
        {
            this.locationX = x;
            this.locationY = y;
            this.confidence = conf;
            this.direction = dir;
            this.feature = feat;
        }

        public override string ToString()
        {
            return String.Format("[Location:({0},{1})][Confidence:{2}][Direction:{3}][Feature:{4}:]", locationX, locationY, confidence, direction, feature);
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
    }
}