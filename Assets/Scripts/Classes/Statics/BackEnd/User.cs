using System;
using System.Data;

// Add methods to verify input 
namespace DeltaCoreBE
{

    public class User
    {
        private int id ;
        private string firstName = string.Empty;
        private string lastName = string.Empty;
        private string email = string.Empty;
        private string password = string.Empty;
        private int role ;
        private int status ;
        private string pwSalt = string.Empty;
        private string pwHash = string.Empty;
        private DateTime addedOn;
        private string addedBy = string.Empty;
        private string approver = string.Empty;
        private string pwResetHash = string.Empty;
        private string pwResetHashTimestamp = string.Empty;

        public User(DataRow data)
        {
            id = Int32.Parse(data[GotchaConstants.USR_TBL_KEY].ToString());
            firstName = data[GotchaConstants.USR_TBL_FIRSTNAME].ToString();
            lastName = data[GotchaConstants.USR_TBL_LASTNAME].ToString();
            email = data[GotchaConstants.USR_TBL_EMAIL].ToString();
            role = Int32.Parse(data[GotchaConstants.USR_TBL_ROLE].ToString());
            status = Int32.Parse(data[GotchaConstants.USR_TBL_STATUS].ToString());
            pwSalt = data[GotchaConstants.USR_TBL_SALT].ToString();
            pwHash = data[GotchaConstants.USR_TBL_HASH].ToString();
            DateTime.TryParse(data[GotchaConstants.USR_TBL_AO].ToString(), out addedOn);
            addedBy = data[GotchaConstants.USR_TBL_AB].ToString();
            approver = data[GotchaConstants.USR_TBL_APPROVER].ToString();
            pwResetHash = data[GotchaConstants.USR_TBL_PW_RESET].ToString();
            pwResetHashTimestamp = data[GotchaConstants.USR_TBL_PW_RESET_TS].ToString();
        }

        public User(string firstName, string lastName, string email, string password)
        {
            // this.id = DBConnection.GetLatestUserID();
            this.id = 1000;
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
            this.password = password;
            // System.Console.WriteLine("Creating User ID: ");
        }

        public int Id
        {
            get {return id; }
        }

        public string FirstName
        {
            get { return firstName; }

        }

        public string LastName
        {
            get { return lastName; }
        }

        public string FullName
        {
            get { return String.Format("{0} {1}", firstName, lastName); }
        }

        public string Email
        {
            get{ return email; }
        }

        public int Role
        {
            get { return role; }
        }

        public bool changePassword(string oldPassword, string newPassword)
        {
            // verify old password 
            // verify new password requirements 
            // IF old password matches then stuff otherwise other stuff 
            this.password = newPassword;
            return true;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null || !(obj is User))
            {
                return false;
            }
            return this.id == ((User)obj).id;
        }

        public override int GetHashCode()
        {
            return id;
        }

        public override string ToString()
        {
            return String.Format("[{0}:{1}]", Id, FullName);
        }
    }


    public class Player : User
    {

        public Player(string firstName, string lastName, string email, string password) : base(firstName, lastName, email, password) { }
        public Player(DataRow data) : base(data) { }
        public override string ToString()
        {
            return String.Format("P:{0}", base.ToString());
        }

    }

    public class Expert : Player
    {
        public Expert(string firstName, string lastName, string email, string password) : base(firstName, lastName, email, password) { }
        public Expert(DataRow data) : base(data) { }
        public override string ToString()
        {
            return String.Format("E:{0}", base.ToString());
        }
    }

    public class Trainee : Player
    {
        public Trainee(string firstName, string lastName, string email, string password) : base(firstName, lastName, email, password) { }
        public Trainee(DataRow data) : base(data) { }
        public override string ToString()
        {
            return base.ToString() + "[Trainee]";
        }
    }
}