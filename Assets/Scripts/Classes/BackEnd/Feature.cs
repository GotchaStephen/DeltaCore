using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace DeltaCoreBE
{
    /*
    public class Feature
    {
        private int id = 0;
        private string name = string.Empty;
        private string image = string.Empty;
        private int sampleType = 0;

        public Feature(int id)
        {
            switch (id)
            {
                case (int)GotchaConstants.FingerprintFeatures.BIFURCATION:
                    this.name = "bifurcation";
                    this.image = "bifurcation.png";
                    break;

                case (int)GotchaConstants.FingerprintFeatures.RIDGE_ENDING:
                    this.name = "ridge ending";
                    this.image = "ridge_ending.png";
                    break;

                case (int)GotchaConstants.FingerprintFeatures.SHORT_RIDGE:
                    this.name = "short ridge";
                    this.image = "short_ridge.png";
                    break;

                default:
                    this.name = "unknown";
                    this.image = "unknown.png";
                    break;
            }
            this.id = id;
        }

        public Feature(int id, string name)
        {
            this.id = id;
            this.name = name;
            this.image = "basic.png";
            this.sampleType = 1;
        }

        public Feature(int id, string name, string image, int sampleType)
        {
            this.id = id;
            this.name = name;
            this.image = image;
            this.sampleType = sampleType;
        }

        public int Id
        {
            get
            {
                return id;
            }
        }

        public override string ToString()
        {
            return String.Format("[{0}:{1}]", id, name);
        }
    }
    */
}