using System;
using System.Data;
namespace DeltaCoreBE
{

    public class Sample
    {
        private int id = -1;
        private string imageName = string.Empty;
        private DateTime addedOn;
        private int addedBy = -1;

        public Sample(DataRow data)
        {
            id = Int32.Parse(data[GotchaConstants.SMPL_TBL_KEY].ToString());
            imageName = data[GotchaConstants.SMPL_TBL_IMG_NAME].ToString();
            DateTime.TryParse(data[GotchaConstants.SMPL_TBL_AO].ToString(), out addedOn);
            addedBy = Int32.Parse(data[GotchaConstants.SMPL_TBL_AB].ToString());
        }

        public Sample(string imageName, int addedby)
        {
            this.id = 10000;
            this.imageName = imageName;
            this.addedBy = 3;
        }

        public int Id
        {
            get
            {
                return id;
            }
        }

        public string ImageName
        {
            get
            {
                return imageName;
            }
        }


        public override string ToString()
        {
            return String.Format("[{0}]{1}", id, imageName);
        }


    }
    public class FingerprintSample : Sample
    {
        public FingerprintSample(string image, int ab) : base(image, ab)
        {
            // Insert into Expert Database 
        }
    }
}