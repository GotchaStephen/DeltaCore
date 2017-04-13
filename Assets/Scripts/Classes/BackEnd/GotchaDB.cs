using System;
using System.Data;
using System.Collections.Generic;
namespace DeltaCoreBE
{
	public class GotchaDB
	{
		private DBConnection g;
		private string sampleFileExtension1 = "tif";
		private string sampleFileExtension2 = "png";

		private static bool debugOn = false ;
		private static void localLog(string msg) { localLog("GotchaDB", msg); }
		private static void localLog(string topic, string msg)
		{
			if (debugOn)
			{
				string logEntry = string.Format("{0:F}: [{1}]{2}", System.DateTime.Now, topic, msg);
				Console.WriteLine(logEntry);
			}
		}

		public GotchaDB()
		{
			g = new DBConnection(GotchaConstants.GOTCHA_HOST, GotchaConstants.GOTCHA_DBNAME, GotchaConstants.GOTCHA_USERNAME, GotchaConstants.GOTCHA_PASSWORD);
		}

		// Generic Calls 
		private DataTable GenericSelectStatement(string query)
		{
			return g.Select(query) ;
		}


		private int GenericOneValueSelectStatement(string query)
		{
			int res = Convert.ToInt32(g.Select(query).Rows[0]["return_val"].ToString());
			return res; 
		}

		// USER FUNCTIONS 
		private List<User> UserSelectStatement(string query)
		{
			DataTable userData = g.Select(query);
			List<User> userList = new List<User>();

			if (userData != null)
			{
				foreach (DataRow row in userData.Rows)
				{
					userList.Add(new User(row));
				}
			}
			if (userList.Count == 0)
			{
				return null;
			}
			return userList;
		}

		public List<User> GetAllUsers()
		{
			string SQLStatement = @"
            SELECT * 
            FROM :USR_TBL_NAME";
			SQLStatement = SQLStatement.Replace(":USR_TBL_NAME", GotchaConstants.USR_TBL_NAME);
			return UserSelectStatement(SQLStatement);
		}

		/* TODO: Addd proper login
        public User loginUser(string email, string pass)
        {

            string SQLStatement = @"
                SELECT * 
                FROM :USR_TBL_NAME
                WHERE :USR_TBL_EMAIL = {0}";
            SQLStatement = SQLStatement.Replace(":USR_TBL_NAME", GotchaConstants.USR_TBL_NAME);
            SQLStatement = SQLStatement.Replace(":USR_TBL_EMAIL", GotchaConstants.USR_TBL_EMAIL);
            SQLStatement = string.Format(SQLStatement, email);

            returns a single record 
            return UserSelectStatement(SQLStatement)[0];
        }
        */

		public User GetUserbyemail(string email)
		{

			string SQLStatement = @"
            SELECT * 
            FROM :USR_TBL_NAME
            WHERE :USR_TBL_EMAIL = '{0}'";
			SQLStatement = SQLStatement.Replace(":USR_TBL_NAME", GotchaConstants.USR_TBL_NAME);
			SQLStatement = SQLStatement.Replace(":USR_TBL_EMAIL", GotchaConstants.USR_TBL_EMAIL);
			SQLStatement = string.Format(SQLStatement, email);
			// returns a single record 
			List<User> tempUserList = UserSelectStatement(SQLStatement);
			if (tempUserList != null)
			{
				return tempUserList[0];
			}
			return null;
		}

		public User GetUserbyID(int id)
		{
			string SQLStatement = @"
            SELECT * 
            FROM :USR_TBL_NAME
            WHERE :USR_TBL_KEY = {0}";
			SQLStatement = SQLStatement.Replace(":USR_TBL_NAME", GotchaConstants.USR_TBL_NAME);
			SQLStatement = SQLStatement.Replace(":USR_TBL_KEY", GotchaConstants.USR_TBL_KEY);
			SQLStatement = string.Format(SQLStatement, id.ToString());
			// returns a single record 
			return UserSelectStatement(SQLStatement)[0];
		}


		public Sample GetSampleIDbyName(string name)
		{
			string SQLStatement = @"
            SELECT * 
            FROM :SMPL_TBL_NAME
            WHERE :SMPL_TBL_IMG_NAME = '{0}.{1}' 
            OR :SMPL_TBL_IMG_NAME = '{0}.{2}'";
			SQLStatement = SQLStatement.Replace(":SMPL_TBL_KEY", GotchaConstants.SMPL_TBL_KEY);
			SQLStatement = SQLStatement.Replace(":SMPL_TBL_NAME", GotchaConstants.SMPL_TBL_NAME);
			SQLStatement = SQLStatement.Replace(":SMPL_TBL_IMG_NAME", GotchaConstants.SMPL_TBL_IMG_NAME);
			SQLStatement = string.Format(SQLStatement, name, sampleFileExtension1, sampleFileExtension2);
			// returns a single record 
			return SampleSelectStatement(SQLStatement)[0];
		}


		public int GetNumberOfUsers()
		{
			string SQLStatement = @"
            SELECT COUNT(*) 
            FROM :USR_TBL_NAME";
			SQLStatement = SQLStatement.Replace(":USR_TBL_NAME", GotchaConstants.USR_TBL_NAME);
			return g.Count(SQLStatement);
		}

		private bool isRecordInTable(string query)
		{
			DataTable queryData = g.Select(query);
			if (queryData != null) { return true; }
			return false; 
		}

		// PLAYER FUNCTIONS
		private List<Player> PlayerSelectStatement(string query)
		{
			DataTable playerData = g.Select(query);
			List<Player> playerList = new List<Player>();

			if (playerData != null)
			{
				foreach (DataRow row in playerData.Rows)
				{
					playerList.Add(new Player(row));
				}
			}
			return playerList;
		}

		public List<Player> GetAllPlayers()
		{
			string SQLStatement = @"
            SELECT * 
            FROM :USR_TBL_NAME";
			SQLStatement = SQLStatement.Replace(":USR_TBL_NAME", GotchaConstants.USR_TBL_NAME);
			return PlayerSelectStatement(SQLStatement);
		}

		public Player GetPlayerbyID(int id)
		{
			string SQLStatement = @"
            SELECT * 
            FROM :USR_TBL_NAME
            WHERE :USR_TBL_KEY = {0}";
			SQLStatement = SQLStatement.Replace(":USR_TBL_NAME", GotchaConstants.USR_TBL_NAME);
			SQLStatement = SQLStatement.Replace(":USR_TBL_KEY", GotchaConstants.USR_TBL_KEY);
			SQLStatement = string.Format(SQLStatement, id.ToString());
			// returns a single record 
			return PlayerSelectStatement(SQLStatement)[0];
		}

		public Player GetPlayerbyemail(string email)
		{
			string SQLStatement = @"
            SELECT * 
            FROM :USR_TBL_NAME
            WHERE :USR_TBL_EMAIL = '{0}'";
			SQLStatement = SQLStatement.Replace(":USR_TBL_NAME", GotchaConstants.USR_TBL_NAME);
			SQLStatement = SQLStatement.Replace(":USR_TBL_EMAIL", GotchaConstants.USR_TBL_EMAIL);
			SQLStatement = string.Format(SQLStatement, email);
			// returns a single record 
			List<Player> tempPlayerList = PlayerSelectStatement(SQLStatement);
			if (tempPlayerList != null) { return tempPlayerList[0]; }

			return null;
		}

		// EXPERT FUNCTIONS
		private List<Expert> ExpertSelectStatement(string query)
		{
			DataTable expertData = g.Select(query);
			List<Expert> expertList = new List<Expert>();

			if (expertData != null)
			{
				foreach (DataRow row in expertData.Rows)
				{
					expertList.Add(new Expert(row));
				}
			}
			return expertList;
		}

		public List<Expert> GetAllExperts()
		{
			string SQLStatement = @"
            SELECT * 
            FROM :USR_TBL_NAME";
			SQLStatement = SQLStatement.Replace(":USR_TBL_NAME", GotchaConstants.USR_TBL_NAME);
			return ExpertSelectStatement(SQLStatement);
		}

		public Player GetExpertbyID(int id)
		{
			string SQLStatement = @"
            SELECT * 
            FROM :USR_TBL_NAME
            WHERE :USR_TBL_KEY = {0}";
			SQLStatement = SQLStatement.Replace(":USR_TBL_NAME", GotchaConstants.USR_TBL_NAME);
			SQLStatement = SQLStatement.Replace(":USR_TBL_KEY", GotchaConstants.USR_TBL_KEY);
			SQLStatement = string.Format(SQLStatement, id.ToString());
			// returns a single record 
			return ExpertSelectStatement(SQLStatement)[0];
		}
		// TRAINEE FUNCTIONS

		/**/
		// SAMPLE FUNCTIONS 
		private List<Sample> SampleSelectStatement(string query)
		{
			DataTable sampleData = g.Select(query);
			List<Sample> sampleList = new List<Sample>();

			if (sampleData != null)
			{
				foreach (DataRow row in sampleData.Rows)
				{
					sampleList.Add(new Sample(row));
				}
			}
			else
			{
				return null;
			}
			return sampleList;
		}

		public List<Sample> GetAllSamples()
		{
			string SQLStatement = @"
                SELECT * 
                FROM :SMPL_TBL_NAME";
			SQLStatement = SQLStatement.Replace(":SMPL_TBL_NAME", GotchaConstants.SMPL_TBL_NAME);
			return SampleSelectStatement(SQLStatement);
		}

		public Sample GetSamplebyID(int id)
		{
			string SQLStatement = @"
                SELECT * 
                FROM :SMPL_TBL_NAME
                WHERE :SMPL_TBL_KEY = {0}";
			SQLStatement = SQLStatement.Replace(":SMPL_TBL_NAME", GotchaConstants.SMPL_TBL_NAME);
			SQLStatement = SQLStatement.Replace(":SMPL_TBL_KEY", GotchaConstants.SMPL_TBL_KEY);
			SQLStatement = string.Format(SQLStatement, id.ToString());
			localLog(SQLStatement);
			// returns a single record 
			List<Sample> s = SampleSelectStatement(SQLStatement);
			if (s != null)
			{
				return s[0];
			}
			System.Console.WriteLine("Sample[ID:{0}] Not found.", id);
			return null;
		}


		public Sample GetSamplebyName(string name)
		{
			string SQLStatement = @"
                SELECT * 
                FROM :SMPL_TBL_NAME
                WHERE :SMPL_TBL_IMG_NAME = {0}";
			SQLStatement = SQLStatement.Replace(":SMPL_TBL_NAME", GotchaConstants.SMPL_TBL_NAME);
			SQLStatement = SQLStatement.Replace(":SMPL_TBL_IMG_NAME", GotchaConstants.SMPL_TBL_IMG_NAME);
			SQLStatement = string.Format(SQLStatement, name);
			localLog(SQLStatement);
			// returns a single record 
			List<Sample> s = SampleSelectStatement(SQLStatement);
			if (s != null)
			{
				return s[0];
			}
			System.Console.WriteLine("Sample[name:{0}] Not found.", name);
			return null;
		}
		// FEATURE FUNCTIONS 


		// APPLIED FEATURE FUNCTIONS
		private List<AppliedFeature> AppliedFeatureSelectStatement(string query)
		{
			DataTable featureData = g.Select(query);
			List<AppliedFeature> featureList = new List<AppliedFeature>();
			localLog("Getting Feature List");
			if (featureData != null)
			{
				foreach (DataRow row in featureData.Rows)
				{
					featureList.Add(new AppliedFeature(row));
				}
			}
			return featureList;
		}

		public AppliedFeature GetAppliedFeaturebyID(int id)
		{
			string SQLStatement = @"
                SELECT * 
                FROM :APP_FTR_TBL_NAME
                WHERE :APP_FTR_TBL_KEY = {0}";
			SQLStatement = SQLStatement.Replace(":APP_FTR_TBL_NAME", GotchaConstants.APP_FTR_TBL_NAME);
			SQLStatement = SQLStatement.Replace(":APP_FTR_TBL_KEY", GotchaConstants.APP_FTR_TBL_KEY);
			SQLStatement = string.Format(SQLStatement, id.ToString());

			// returns a single record 
			List<AppliedFeature> rec = AppliedFeatureSelectStatement(SQLStatement);
			if (rec != null)
			{
				return rec[0];
			}
			System.Console.WriteLine("AppliedFeature[ID:{0}] Not found.", id);
			return null;
		}


		// Needs Polishing but needed at this point
		public bool AddSampleToDB(Sample s, int approver)
		{

			string SQLStatement = @"
                INSERT INTO :SMPL_TBL_NAME 
                (:SMPL_TBL_KEY, :SMPL_TBL_IMG_NAME, :SMPL_TBL_AB) 
                VALUES ('{0}','{1}','{2}')";
			SQLStatement = SQLStatement.Replace(":SMPL_TBL_NAME", GotchaConstants.SMPL_TBL_NAME);
			SQLStatement = SQLStatement.Replace(":SMPL_TBL_KEY", GotchaConstants.SMPL_TBL_KEY);
			SQLStatement = SQLStatement.Replace(":SMPL_TBL_IMG_NAME", GotchaConstants.SMPL_TBL_IMG_NAME);
			SQLStatement = SQLStatement.Replace(":SMPL_TBL_AB", GotchaConstants.SMPL_TBL_AB);
			SQLStatement = string.Format(SQLStatement, s.Id.ToString(), s.ImageName, approver);
			g.Modify(SQLStatement);
			return true;
		}

		// PLAYER-SAMPLE functions 

		public List<Sample> GetNewSamplesForPlayer(int playedId)
		{
			string SQLStatement = @"
                SELECT * 
                FROM :SMPL_TBL_NAME
                WHERE :SMPL_TBL_KEY NOT IN 
                (
                    SELECT :SMPL_PHS_VIEW_SMPL_ID 
                    FROM :SMPL_PHS_VIEW_NAME
                    WHERE :SMPL_PHS_VIEW_USR_ID = {0}
                )
            ";
			SQLStatement = SQLStatement.Replace(":SMPL_TBL_NAME", GotchaConstants.SMPL_TBL_NAME);
			SQLStatement = SQLStatement.Replace(":SMPL_TBL_KEY", GotchaConstants.SMPL_TBL_KEY);

			SQLStatement = SQLStatement.Replace(":SMPL_PHS_VIEW_NAME", GotchaConstants.SMPL_PHS_VIEW_NAME);
			SQLStatement = SQLStatement.Replace(":SMPL_PHS_VIEW_SMPL_ID", GotchaConstants.SMPL_PHS_VIEW_SMPL_ID);
			SQLStatement = SQLStatement.Replace(":SMPL_PHS_VIEW_USR_ID", GotchaConstants.SMPL_PHS_VIEW_USR_ID);
			SQLStatement = string.Format(SQLStatement, playedId);

			localLog(SQLStatement);
			return SampleSelectStatement(SQLStatement);
		}

		private List<Sample> GetUsedSamplesForPlayerbyPhase(Player p, int phaseId)
		{
			string SQLStatement = @"
                SELECT * 
                FROM :SMPL_TBL_NAME
                WHERE :SMPL_TBL_KEY IN 
                (
                    SELECT :SMPL_PHS_VIEW_SMPL_ID 
                    FROM :SMPL_PHS_VIEW_NAME
                    WHERE 
            :SMPL_PHS_VIEW_USR_ID = {0} AND 
                        :SMPL_PHS_VIEW_PHS_ID = {1}
                )
    ";
			SQLStatement = SQLStatement.Replace(":SMPL_TBL_NAME", GotchaConstants.SMPL_TBL_NAME);
			SQLStatement = SQLStatement.Replace(":SMPL_TBL_KEY", GotchaConstants.SMPL_TBL_KEY);

			SQLStatement = SQLStatement.Replace(":SMPL_PHS_VIEW_NAME", GotchaConstants.SMPL_PHS_VIEW_NAME);
			SQLStatement = SQLStatement.Replace(":SMPL_PHS_VIEW_SMPL_ID", GotchaConstants.SMPL_PHS_VIEW_SMPL_ID);
			SQLStatement = SQLStatement.Replace(":SMPL_PHS_VIEW_PHS_ID", GotchaConstants.SMPL_PHS_VIEW_PHS_ID);
			SQLStatement = SQLStatement.Replace(":SMPL_PHS_VIEW_USR_ID", GotchaConstants.SMPL_PHS_VIEW_USR_ID);

			SQLStatement = string.Format(SQLStatement, p.Id.ToString(), phaseId);
			return SampleSelectStatement(SQLStatement);
		}

		/*
        public List<Sample> GetAnalysisSamplesForPlayer(int p)
        {
            return GetUsedSamplesForPlayerbyPhase(p, (int)GotchaConstants.GyroPhases.ANALYSIS);
        }

        public List<Sample> GetComparisonSamplesForPlayer(Player p)
        {
            return GetUsedSamplesForPlayerbyPhase(p, (int)GotchaConstants.GyroPhases.COMPARISON);
        }
        */

		// Need function to get sample phase 
		public bool markSampleCompleted(int playerId, int sampleId)
		{
			string SQLStatement = @"
                INSERT INTO :SMPL_PHS_TBL_NAME (
                    :SMPL_PHS_TBL_SMPL_ID, 
                    :SMPL_PHS_TBL_USR_ID
                ) 
                VALUES ('{0}','{1}')";
			SQLStatement = SQLStatement.Replace(":SMPL_PHS_TBL_NAME", GotchaConstants.SMPL_PHS_TBL_NAME);
			SQLStatement = SQLStatement.Replace(":SMPL_PHS_TBL_SMPL_ID", GotchaConstants.SMPL_PHS_TBL_SMPL_ID);
			SQLStatement = SQLStatement.Replace(":SMPL_PHS_TBL_USR_ID", GotchaConstants.SMPL_PHS_TBL_USR_ID);
			SQLStatement = string.Format(SQLStatement, sampleId, playerId);
			g.Modify(SQLStatement);
			return true;
		}
		public bool isSampleComplete(int playerId, int SampleId)
		{
			string SQLStatement = @"
                SELECT * 
                FROM :SMPL_PHS_TBL_NAME
                WHERE :SMPL_PHS_TBL_SMPL_ID = {0}
                    AND :SMPL_PHS_TBL_USR_ID = {1}";
			SQLStatement = SQLStatement.Replace(":SMPL_PHS_TBL_NAME", GotchaConstants.SMPL_PHS_TBL_NAME);
			SQLStatement = SQLStatement.Replace(":SMPL_PHS_TBL_SMPL_ID", GotchaConstants.SMPL_PHS_TBL_SMPL_ID);
			SQLStatement = SQLStatement.Replace(":SMPL_PHS_TBL_USR_ID", GotchaConstants.SMPL_PHS_TBL_USR_ID);
			SQLStatement = string.Format(SQLStatement, SampleId, playerId);
			// returns a single record 
			if(isRecordInTable(SQLStatement))
			{
				return true;
			}
			return false;
		}

		public bool isSampleReady(int SampleId)
		{
			string SQLStatement = @"
                SELECT DISTINCT(:APP_FTR_RDY_TBL_SMPL_ID)
                FROM :APP_FTR_RDY_TBL_NAME
                WHERE :APP_FTR_RDY_TBL_SMPL_ID = {0}";
			SQLStatement = SQLStatement.Replace(":APP_FTR_RDY_TBL_NAME", GotchaConstants.APP_FTR_RDY_TBL_NAME);
			SQLStatement = SQLStatement.Replace(":APP_FTR_RDY_TBL_SMPL_ID", GotchaConstants.APP_FTR_RDY_TBL_SMPL_ID);
			SQLStatement = string.Format(SQLStatement, SampleId);
			// returns a single record 
			if (isRecordInTable(SQLStatement))
			{
				return true;
			}
			return false;
		}

		// PLAYER-SAMPLE-FEATURE functions 
		public List<AppliedFeature> GetAppliedFeaturesForExpertSample(int sampleId)
		{
			string SQLStatement = @"
                    SELECT * 
                    FROM :APP_FTR_EXP_TBL_NAME
                    WHERE :APP_FTR_EXP_TBL_SMPL_ID = {0}
                    ";

			SQLStatement = SQLStatement.Replace(":APP_FTR_EXP_TBL_NAME", GotchaConstants.APP_FTR_EXP_TBL_NAME);
			SQLStatement = SQLStatement.Replace(":APP_FTR_EXP_TBL_SMPL_ID", GotchaConstants.APP_FTR_EXP_TBL_SMPL_ID);
			SQLStatement = string.Format(SQLStatement, sampleId);
			return AppliedFeatureSelectStatement(SQLStatement);
		}
		public List<AppliedFeature> GetAppliedFeaturesForPlayerSample(int pId, int sId)
		{
			string SQLStatement = @"
                    SELECT * 
                    FROM :APP_FTR_TBL_NAME
                    WHERE
                        :APP_FTR_TBL_SMPL_ID = {0}
                        AND :APP_FTR_TBL_USR_ID = {1}";

			SQLStatement = SQLStatement.Replace(":APP_FTR_TBL_NAME", GotchaConstants.APP_FTR_TBL_NAME);
			SQLStatement = SQLStatement.Replace(":APP_FTR_TBL_SMPL_ID", GotchaConstants.APP_FTR_TBL_SMPL_ID);
			SQLStatement = SQLStatement.Replace(":APP_FTR_TBL_USR_ID", GotchaConstants.APP_FTR_TBL_USR_ID);
			SQLStatement = string.Format(SQLStatement, sId.ToString(), pId.ToString());
			return AppliedFeatureSelectStatement(SQLStatement);
		}

		public List<AppliedFeature> GetAppliedFeaturesForExpertSample(int pId, int sId)
		{
			string SQLStatement = @"
                    SELECT * 
                    FROM :APP_FTR_EXP_TBL_NAME
                    WHERE
                        :APP_FTR_EXP_TBL_SMPL_ID = {0}
                        AND :APP_FTR_EXP_TBL_USR_ID = {1}";

			SQLStatement = SQLStatement.Replace(":APP_FTR_EXP_TBL_NAME", GotchaConstants.APP_FTR_EXP_TBL_NAME);
			SQLStatement = SQLStatement.Replace(":APP_FTR_EXP_TBL_SMPL_ID", GotchaConstants.APP_FTR_EXP_TBL_SMPL_ID);
			SQLStatement = SQLStatement.Replace(":APP_FTR_EXP_TBL_USR_ID", GotchaConstants.APP_FTR_EXP_TBL_USR_ID);
			SQLStatement = string.Format(SQLStatement, sId.ToString(), pId.ToString());
			return AppliedFeatureSelectStatement(SQLStatement);
		}


		public List<AppliedFeature> GetSolutionAppliedFeaturesForPlayerSample(int sId)
		{
			string SQLStatement = @"
                    SELECT * 
                    FROM :APP_FTR_RDY_TBL_NAME
                    WHERE :APP_FTR_RDY_TBL_SMPL_ID = {0}";

			SQLStatement = SQLStatement.Replace(":APP_FTR_RDY_TBL_NAME", GotchaConstants.APP_FTR_RDY_TBL_NAME);
			SQLStatement = SQLStatement.Replace(":APP_FTR_RDY_TBL_SMPL_ID", GotchaConstants.APP_FTR_RDY_TBL_SMPL_ID);
			SQLStatement = string.Format(SQLStatement, sId.ToString());
			return AppliedFeatureSelectStatement(SQLStatement);
		}

		public int AddAppliedFeatureForPlayerSample(Player p, Sample s, AppliedFeature af)
		{
			string SQLStatement = @"
                INSERT INTO :APP_FTR_TBL_NAME (
                    :APP_FTR_TBL_SMPL_ID, 
                    :APP_FTR_TBL_USR_ID, 
                    :APP_FTR_TBL_FTR_ID,
                    :APP_FTR_TBL_CONF,
                    :APP_FTR_TBL_LOC_X,
                    :APP_FTR_TBL_LOC_Y,
                    :APP_FTR_TBL_DIRECTION,
                    :APP_FTR_TBL_PHS_ID
                ) 
                VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')";
			SQLStatement = SQLStatement.Replace(":APP_FTR_TBL_NAME", GotchaConstants.APP_FTR_TBL_NAME);
			SQLStatement = SQLStatement.Replace(":APP_FTR_TBL_SMPL_ID", GotchaConstants.APP_FTR_TBL_SMPL_ID);
			SQLStatement = SQLStatement.Replace(":APP_FTR_TBL_USR_ID", GotchaConstants.APP_FTR_TBL_USR_ID);
			SQLStatement = SQLStatement.Replace(":APP_FTR_TBL_FTR_ID", GotchaConstants.APP_FTR_TBL_FTR_ID);
			SQLStatement = SQLStatement.Replace(":APP_FTR_TBL_CONF", GotchaConstants.APP_FTR_TBL_CONF);
			SQLStatement = SQLStatement.Replace(":APP_FTR_TBL_LOC_X", GotchaConstants.APP_FTR_TBL_LOC_X);
			SQLStatement = SQLStatement.Replace(":APP_FTR_TBL_LOC_Y", GotchaConstants.APP_FTR_TBL_LOC_Y);
			SQLStatement = SQLStatement.Replace(":APP_FTR_TBL_DIRECTION", GotchaConstants.APP_FTR_TBL_DIRECTION);
			SQLStatement = SQLStatement.Replace(":APP_FTR_TBL_PHS_ID", GotchaConstants.APP_FTR_TBL_PHS_ID);

			SQLStatement = string.Format(SQLStatement, s.Id, p.Id, af.FeatureID , af.Confidence,
				af.LocationX, af.LocationY, af.Direction, af.Phase);
			return g.InsertReturnID(SQLStatement);
		}

		public int AddAppliedFeatureForPlayerSample(int playerId, int sampleId, AppliedFeature af)
		{
			string SQLStatement = @"
                INSERT INTO :APP_FTR_TBL_NAME (
                    :APP_FTR_TBL_SMPL_ID, 
                    :APP_FTR_TBL_USR_ID, 
                    :APP_FTR_TBL_FTR_ID,
                    :APP_FTR_TBL_CONF,
                    :APP_FTR_TBL_LOC_X,
                    :APP_FTR_TBL_LOC_Y,
                    :APP_FTR_TBL_DIRECTION,
                    :APP_FTR_TBL_PHS_ID
                ) 
                VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')";
			SQLStatement = SQLStatement.Replace(":APP_FTR_TBL_NAME", GotchaConstants.APP_FTR_TBL_NAME);
			SQLStatement = SQLStatement.Replace(":APP_FTR_TBL_SMPL_ID", GotchaConstants.APP_FTR_TBL_SMPL_ID);
			SQLStatement = SQLStatement.Replace(":APP_FTR_TBL_USR_ID", GotchaConstants.APP_FTR_TBL_USR_ID);
			SQLStatement = SQLStatement.Replace(":APP_FTR_TBL_FTR_ID", GotchaConstants.APP_FTR_TBL_FTR_ID);
			SQLStatement = SQLStatement.Replace(":APP_FTR_TBL_CONF", GotchaConstants.APP_FTR_TBL_CONF);
			SQLStatement = SQLStatement.Replace(":APP_FTR_TBL_LOC_X", GotchaConstants.APP_FTR_TBL_LOC_X);
			SQLStatement = SQLStatement.Replace(":APP_FTR_TBL_LOC_Y", GotchaConstants.APP_FTR_TBL_LOC_Y);
			SQLStatement = SQLStatement.Replace(":APP_FTR_TBL_DIRECTION", GotchaConstants.APP_FTR_TBL_DIRECTION);
			SQLStatement = SQLStatement.Replace(":APP_FTR_TBL_PHS_ID", GotchaConstants.APP_FTR_TBL_PHS_ID);

			SQLStatement = string.Format(SQLStatement, sampleId, playerId, af.FeatureID, af.Confidence,
				af.LocationX, af.LocationY, af.Direction, af.Phase);
			return g.InsertReturnID(SQLStatement);
		}

		public int AddAppliedFeatureForExpertSample(int playerId, int sampleId, AppliedFeature af)
		{
			string SQLStatement = @"
                INSERT INTO :APP_FTR_EXP_TBL_NAME (
                    :APP_FTR_EXP_TBL_SMPL_ID, 
                    :APP_FTR_EXP_TBL_USR_ID, 
                    :APP_FTR_EXP_TBL_FTR_ID,
                    :APP_FTR_EXP_TBL_CONF,
                    :APP_FTR_EXP_TBL_LOC_X,
                    :APP_FTR_EXP_TBL_LOC_Y,
                    :APP_FTR_EXP_TBL_DIRECTION
                ) 
                VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')";
			SQLStatement = SQLStatement.Replace(":APP_FTR_EXP_TBL_NAME", GotchaConstants.APP_FTR_EXP_TBL_NAME);
			SQLStatement = SQLStatement.Replace(":APP_FTR_EXP_TBL_SMPL_ID", GotchaConstants.APP_FTR_EXP_TBL_SMPL_ID);
			SQLStatement = SQLStatement.Replace(":APP_FTR_EXP_TBL_USR_ID", GotchaConstants.APP_FTR_EXP_TBL_USR_ID);
			SQLStatement = SQLStatement.Replace(":APP_FTR_EXP_TBL_FTR_ID", GotchaConstants.APP_FTR_EXP_TBL_FTR_ID);
			SQLStatement = SQLStatement.Replace(":APP_FTR_EXP_TBL_CONF", GotchaConstants.APP_FTR_EXP_TBL_CONF);
			SQLStatement = SQLStatement.Replace(":APP_FTR_EXP_TBL_LOC_X", GotchaConstants.APP_FTR_EXP_TBL_LOC_X);
			SQLStatement = SQLStatement.Replace(":APP_FTR_EXP_TBL_LOC_Y", GotchaConstants.APP_FTR_EXP_TBL_LOC_Y);
			SQLStatement = SQLStatement.Replace(":APP_FTR_EXP_TBL_DIRECTION", GotchaConstants.APP_FTR_EXP_TBL_DIRECTION);

			SQLStatement = string.Format(SQLStatement, sampleId, playerId, af.FeatureID, af.Confidence,
				af.LocationX, af.LocationY, af.Direction);
			return g.InsertReturnID(SQLStatement);
		}

		public bool removeSampleFromCompleted(int playerId, int sampleId)
		{
			string SQLStatement = @"
                DELETE 
                FROM :SMPL_PHS_TBL_NAME
                WHERE :SMPL_PHS_TBL_SMPL_ID = {0}
                    AND :SMPL_PHS_TBL_USR_ID = {1}";
			SQLStatement = SQLStatement.Replace(":SMPL_PHS_TBL_NAME", GotchaConstants.SMPL_PHS_TBL_NAME);
			SQLStatement = SQLStatement.Replace(":SMPL_PHS_TBL_SMPL_ID", GotchaConstants.SMPL_PHS_TBL_SMPL_ID);
			SQLStatement = SQLStatement.Replace(":SMPL_PHS_TBL_USR_ID", GotchaConstants.SMPL_PHS_TBL_USR_ID);
			SQLStatement = string.Format(SQLStatement, sampleId, playerId);
			g.Modify(SQLStatement); 
			return true ; 
		}
		public bool deleteAllAppliedFeaturesSampleExpert(int playerId, int sampleId)
		{

			string SQLStatement = @"
                    DELETE * 
                    FROM :APP_FTR_EXP_TBL_NAME
                    WHERE
                        :APP_FTR_EXP_TBL_SMPL_ID = {0}
                        AND :APP_FTR_EXP_TBL_USR_ID = {1}";

			SQLStatement = SQLStatement.Replace(":APP_FTR_EXP_TBL_NAME", GotchaConstants.APP_FTR_EXP_TBL_NAME);
			SQLStatement = SQLStatement.Replace(":APP_FTR_EXP_TBL_SMPL_ID", GotchaConstants.APP_FTR_EXP_TBL_SMPL_ID);
			SQLStatement = SQLStatement.Replace(":APP_FTR_EXP_TBL_USR_ID", GotchaConstants.APP_FTR_EXP_TBL_USR_ID);
			SQLStatement = string.Format(SQLStatement, sampleId, playerId);
			g.Modify(SQLStatement);
			return true;
		}

		public bool deleteAllAppliedFeaturesSamplePlayer(int playerId, int sampleId)
		{
			string SQLStatement = @"
                    DELETE 
                    FROM :APP_FTR_TBL_NAME
                    WHERE
                        :APP_FTR_TBL_SMPL_ID = {0}
                        AND :APP_FTR_TBL_USR_ID = {1}";

			SQLStatement = SQLStatement.Replace(":APP_FTR_TBL_NAME", GotchaConstants.APP_FTR_TBL_NAME);
			SQLStatement = SQLStatement.Replace(":APP_FTR_TBL_SMPL_ID", GotchaConstants.APP_FTR_TBL_SMPL_ID);
			SQLStatement = SQLStatement.Replace(":APP_FTR_TBL_USR_ID", GotchaConstants.APP_FTR_TBL_USR_ID);
			SQLStatement = string.Format(SQLStatement, sampleId, playerId);
			g.Modify(SQLStatement);
			return true; 
		}

		public bool removeAppliedFeatureForPlayerSampleById(int id)
		{
			string SQLStatement = @"
                    DELETE 
                    FROM :APP_FTR_TBL_NAME
                    WHERE :APP_FTR_TBL_KEY = {0}";
			SQLStatement = SQLStatement.Replace(":APP_FTR_TBL_NAME", GotchaConstants.APP_FTR_TBL_NAME);
			SQLStatement = SQLStatement.Replace(":APP_FTR_TBL_KEY", GotchaConstants.APP_FTR_TBL_KEY);
			SQLStatement = string.Format(SQLStatement, id);
			g.Modify(SQLStatement);
			return true;
		}

		public int addAnalysisVerdictForSample(int playerID,int sampleID , int verdict)
		{
			string SQLStatement = @"
                    INSERT INTO :ANALYSIS_VER_TBL_NAME (
                        :ANALYSIS_VER_TBL_SMPL, 
                        :ANALYSIS_VER_TBL_USR,
                        :ANALYSIS_VER_TBL_VER 
                    ) 
                    VALUES ('{0}','{1}','{2}')";
			SQLStatement = SQLStatement.Replace(":ANALYSIS_VER_TBL_NAME", GotchaConstants.ANALYSIS_VER_TBL_NAME);
			SQLStatement = SQLStatement.Replace(":ANALYSIS_VER_TBL_SMPL", GotchaConstants.ANALYSIS_VER_TBL_SMPL);
			SQLStatement = SQLStatement.Replace(":ANALYSIS_VER_TBL_VER", GotchaConstants.ANALYSIS_VER_TBL_VER);
			SQLStatement = SQLStatement.Replace(":ANALYSIS_VER_TBL_USR", GotchaConstants.ANALYSIS_VER_TBL_USR);
			SQLStatement = string.Format(SQLStatement, sampleID, playerID, verdict);
			localLog(SQLStatement);
			return g.InsertReturnID(SQLStatement);
		}

		public DataTable GetLOTDSamplesForUser(int pId)
		{
			string SQLStatement = @"
                SELECT 
                	vs.:SMPL_LOTD_V_SAMPLE_ID, 
                    vs.:SMPL_LOTD_V_USED_DATE, 
                    vs.:SMPL_LOTD_V_IMG_NAME,
                    cs.:SMPL_LOTD_COMPL_TBL_I_COST,
                    cs.:SMPL_LOTD_COMPL_TBL_S_COST,
                    cs.:SMPL_LOTD_COMPL_TBL_D_COST,
                     cs.:SMPL_LOTD_COMPL_TBL_VERDICT,
                    cs.:SMPL_LOTD_COMPL_TBL_AO
                FROM 
                    :SMPL_LOTD_V_NAME vs 
                    LEFT JOIN (
                        SELECT  * 
                        FROM    :SMPL_LOTD_COMPL_TBL_NAME 
                        WHERE   :SMPL_LOTD_COMPL_TBL_USR_ID = {0}
                    ) cs
                    ON vs.:SMPL_LOTD_V_SAMPLE_ID = cs.:SMPL_LOTD_COMPL_TBL_SAMPLE_ID
                WHERE 
                	vs.:SMPL_LOTD_V_USED_DATE BETWEEN NOW() - INTERVAL 30 DAY AND NOW() ; 
            ";

			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_NAME", GotchaConstants.SMPL_LOTD_COMPL_TBL_NAME);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_I_COST", GotchaConstants.SMPL_LOTD_COMPL_TBL_I_COST);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_S_COST", GotchaConstants.SMPL_LOTD_COMPL_TBL_S_COST);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_D_COST", GotchaConstants.SMPL_LOTD_COMPL_TBL_D_COST);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_VERDICT", GotchaConstants.SMPL_LOTD_COMPL_TBL_VERDICT);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_AO", GotchaConstants.SMPL_LOTD_COMPL_TBL_AO);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_USR_ID", GotchaConstants.SMPL_LOTD_COMPL_TBL_USR_ID);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_SAMPLE_ID", GotchaConstants.SMPL_LOTD_COMPL_TBL_SAMPLE_ID);

			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_V_NAME", GotchaConstants.SMPL_LOTD_V_NAME);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_V_SAMPLE_ID", GotchaConstants.SMPL_LOTD_V_SAMPLE_ID);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_V_USED_DATE", GotchaConstants.SMPL_LOTD_V_USED_DATE);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_V_IMG_NAME", GotchaConstants.SMPL_LOTD_V_IMG_NAME);

			SQLStatement = string.Format(SQLStatement, pId);
			// localLog(SQLStatement); 
			return GenericSelectStatement(SQLStatement);
		}

		public DataTable GetLOTDSampleForUser(int pId)
		{
			string SQLStatement = @"
                SELECT 
                	vs.:SMPL_LOTD_V_SAMPLE_ID, 
                    vs.:SMPL_LOTD_V_USED_DATE, 
                    vs.:SMPL_LOTD_V_IMG_NAME,
                    cs.:SMPL_LOTD_COMPL_TBL_I_COST,
                    cs.:SMPL_LOTD_COMPL_TBL_S_COST,
                    cs.:SMPL_LOTD_COMPL_TBL_D_COST,
                    cs.:SMPL_LOTD_COMPL_TBL_VERDICT,
                    cs.:SMPL_LOTD_COMPL_TBL_AO
                FROM 
                    :SMPL_LOTD_V_NAME vs 
                    LEFT JOIN (
                        SELECT  * 
                        FROM    :SMPL_LOTD_COMPL_TBL_NAME 
                        WHERE   :SMPL_LOTD_COMPL_TBL_USR_ID = {0}
                    ) cs
                    ON vs.:SMPL_LOTD_V_SAMPLE_ID = cs.:SMPL_LOTD_COMPL_TBL_SAMPLE_ID
                    WHERE DATE(vs.:SMPL_LOTD_V_USED_DATE) = DATE(NOW()) ; 
            ";

			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_NAME", GotchaConstants.SMPL_LOTD_COMPL_TBL_NAME);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_I_COST", GotchaConstants.SMPL_LOTD_COMPL_TBL_I_COST);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_S_COST", GotchaConstants.SMPL_LOTD_COMPL_TBL_S_COST);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_D_COST", GotchaConstants.SMPL_LOTD_COMPL_TBL_D_COST);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_VERDICT", GotchaConstants.SMPL_LOTD_COMPL_TBL_VERDICT);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_AO", GotchaConstants.SMPL_LOTD_COMPL_TBL_AO);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_USR_ID", GotchaConstants.SMPL_LOTD_COMPL_TBL_USR_ID);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_SAMPLE_ID", GotchaConstants.SMPL_LOTD_COMPL_TBL_SAMPLE_ID);

			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_V_NAME", GotchaConstants.SMPL_LOTD_V_NAME);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_V_SAMPLE_ID", GotchaConstants.SMPL_LOTD_V_SAMPLE_ID);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_V_USED_DATE", GotchaConstants.SMPL_LOTD_V_USED_DATE);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_V_IMG_NAME", GotchaConstants.SMPL_LOTD_V_IMG_NAME);

			SQLStatement = string.Format(SQLStatement, pId);
			localLog(SQLStatement);
			return GenericSelectStatement(SQLStatement);
		}

		public int playerCompletedLatentOfTheDay(int pId)
		{
			string SQLStatement = @"
                SELECT IFNULL(cs.:SMPL_LOTD_COMPL_TBL_VERDICT, -1) as return_val
                FROM 
                    :SMPL_LOTD_V_NAME vs 
                    LEFT JOIN (
                        SELECT  * 
                        FROM    :SMPL_LOTD_COMPL_TBL_NAME 
                        WHERE   :SMPL_LOTD_COMPL_TBL_USR_ID = {0}
                    ) cs
                    ON vs.:SMPL_LOTD_V_SAMPLE_ID = cs.:SMPL_LOTD_COMPL_TBL_SAMPLE_ID
                WHERE DATE(vs.:SMPL_LOTD_V_USED_DATE) = DATE(NOW()) ; 
            ";

			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_NAME", GotchaConstants.SMPL_LOTD_COMPL_TBL_NAME);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_VERDICT", GotchaConstants.SMPL_LOTD_COMPL_TBL_VERDICT);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_USR_ID", GotchaConstants.SMPL_LOTD_COMPL_TBL_USR_ID);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_SAMPLE_ID", GotchaConstants.SMPL_LOTD_COMPL_TBL_SAMPLE_ID);

			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_V_NAME", GotchaConstants.SMPL_LOTD_V_NAME);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_V_SAMPLE_ID", GotchaConstants.SMPL_LOTD_V_SAMPLE_ID);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_V_USED_DATE", GotchaConstants.SMPL_LOTD_V_USED_DATE);

			SQLStatement = string.Format(SQLStatement, pId);
			// localLog(SQLStatement);
			return GenericOneValueSelectStatement(SQLStatement);
		}
		// public DataTable getUserListForAnalysedSample(int sampleID)
		// {
		//     string SQLStatement = @"
		//         Select * 
		//         FROM :SMPL_LOTD_COMPL_TBL_NAME
		//         where :SMPL_LOTD_COMPL_TBL_SAMPLE_ID = {0} ;
		//     ";
		//     SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_NAME", GotchaConstants.SMPL_LOTD_COMPL_TBL_NAME);
		//     SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_SAMPLE_ID", GotchaConstants.SMPL_LOTD_COMPL_TBL_SAMPLE_ID);
		//     SQLStatement = string.Format(SQLStatement, sampleID) ;
		//     return GenericSelectStatement(SQLStatement);
		// }

		public DataTable getUserListForLotdAnalysedSample(int sampleID)
		{
			string SQLStatement = @"
                Select * 
                FROM :SMPL_LOTD_COMPL_TBL_NAME
                where 
                    :SMPL_LOTD_COMPL_TBL_SAMPLE_ID = {0} 
                    AND :SMPL_LOTD_COMPL_TBL_USR_ID != 1"; 


			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_NAME", GotchaConstants.SMPL_LOTD_COMPL_TBL_NAME);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_SAMPLE_ID", GotchaConstants.SMPL_LOTD_COMPL_TBL_SAMPLE_ID);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_USR_ID", GotchaConstants.SMPL_LOTD_COMPL_TBL_USR_ID);
			SQLStatement = string.Format(SQLStatement, sampleID);
			return GenericSelectStatement(SQLStatement);
		}

		public List<AppliedFeature> GetAppliedFeaturesForLOTDSampleUser(int sampleId, int playerId)
		{
			string SQLStatement = @"
                    SELECT * 
                    FROM :APP_FTR_LOTD_TBL_NAME
                    WHERE 
                        :APP_FTR_LOTD_TBL_SMPL_ID = {0}
                        AND :APP_FTR_LOTD_TBL_USR_ID = {1}
                    ";
			SQLStatement = SQLStatement.Replace(":APP_FTR_LOTD_TBL_NAME", GotchaConstants.APP_FTR_LOTD_TBL_NAME);
			SQLStatement = SQLStatement.Replace(":APP_FTR_LOTD_TBL_SMPL_ID", GotchaConstants.APP_FTR_LOTD_TBL_SMPL_ID);
			SQLStatement = SQLStatement.Replace(":APP_FTR_LOTD_TBL_USR_ID", GotchaConstants.APP_FTR_LOTD_TBL_USR_ID);
			SQLStatement = string.Format(SQLStatement, sampleId, playerId);
			// localLog(SQLStatement); 
			return AppliedFeatureSelectStatement(SQLStatement);
		}


		public int AddAppliedFeatureForSolutionLOTD(int sampleID, FingerPrintConsensusPoint cso)
		{
			string SQLStatement = @"
                INSERT INTO :APP_FTR_SOL_LOTD_TBL_NAME (
                    :APP_FTR_SOL_LOTD_TBL_SMPL_ID, 
                    :APP_FTR_SOL_LOTD_TBL_FTR_ID,
                    :APP_FTR_SOL_LOTD_TBL_CONF,
                    :APP_FTR_SOL_LOTD_TBL_LOC_X,
                    :APP_FTR_SOL_LOTD_TBL_LOC_Y,
                    :APP_FTR_SOL_LOTD_TBL_DIRECTION,
                    :APP_FTR_SOL_LOTD_TBL_WEIGHT
                ) 
                VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')";
			SQLStatement = SQLStatement.Replace(":APP_FTR_SOL_LOTD_TBL_NAME", GotchaConstants.APP_FTR_SOL_LOTD_TBL_NAME);
			SQLStatement = SQLStatement.Replace(":APP_FTR_SOL_LOTD_TBL_SMPL_ID", GotchaConstants.APP_FTR_SOL_LOTD_TBL_SMPL_ID);
			SQLStatement = SQLStatement.Replace(":APP_FTR_SOL_LOTD_TBL_FTR_ID", GotchaConstants.APP_FTR_SOL_LOTD_TBL_FTR_ID);
			SQLStatement = SQLStatement.Replace(":APP_FTR_SOL_LOTD_TBL_CONF", GotchaConstants.APP_FTR_SOL_LOTD_TBL_CONF);
			SQLStatement = SQLStatement.Replace(":APP_FTR_SOL_LOTD_TBL_LOC_X", GotchaConstants.APP_FTR_SOL_LOTD_TBL_LOC_X);
			SQLStatement = SQLStatement.Replace(":APP_FTR_SOL_LOTD_TBL_LOC_Y", GotchaConstants.APP_FTR_SOL_LOTD_TBL_LOC_Y);
			SQLStatement = SQLStatement.Replace(":APP_FTR_SOL_LOTD_TBL_DIRECTION", GotchaConstants.APP_FTR_SOL_LOTD_TBL_DIRECTION);
			SQLStatement = SQLStatement.Replace(":APP_FTR_SOL_LOTD_TBL_WEIGHT", GotchaConstants.APP_FTR_SOL_LOTD_TBL_WEIGHT);
			SQLStatement = SQLStatement.Replace(":APP_FTR_SOL_LOTD_TBL_AO", GotchaConstants.APP_FTR_SOL_LOTD_TBL_AO);

			SQLStatement = string.Format(SQLStatement, sampleID, cso.Feature, cso.Confidence,
				cso.LocationX, cso.LocationY, cso.Direction, cso.Weight);
			// localLog(SQLStatement); 
			return g.InsertReturnID(SQLStatement);
		}
		public bool DeleteAppliedFeaturesForSolutionLOTD(int sampleId)
		{
			string SQLStatement = @"
                    DELETE 
                    FROM    :APP_FTR_SOL_LOTD_TBL_NAME
                    WHERE   :APP_FTR_SOL_LOTD_TBL_SMPL_ID = {0}";

			SQLStatement = SQLStatement.Replace(":APP_FTR_SOL_LOTD_TBL_NAME", GotchaConstants.APP_FTR_SOL_LOTD_TBL_NAME);
			SQLStatement = SQLStatement.Replace(":APP_FTR_SOL_LOTD_TBL_SMPL_ID", GotchaConstants.APP_FTR_SOL_LOTD_TBL_SMPL_ID);
			SQLStatement = string.Format(SQLStatement, sampleId);
			// localLog(SQLStatement); 
			g.Modify(SQLStatement);
			return true;
		}

		public bool removeSolutionLOTDSampleFromCompleted(int sampleId)
		{
			string SQLStatement = @"
                DELETE 
                FROM :SMPL_LOTD_COMPL_TBL_NAME
                WHERE :SMPL_LOTD_COMPL_TBL_USR_ID = 1
                    AND :SMPL_LOTD_COMPL_TBL_SAMPLE_ID = {0}";
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_NAME", GotchaConstants.SMPL_LOTD_COMPL_TBL_NAME);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_USR_ID", GotchaConstants.SMPL_LOTD_COMPL_TBL_USR_ID);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_SAMPLE_ID", GotchaConstants.SMPL_LOTD_COMPL_TBL_SAMPLE_ID);
			SQLStatement = string.Format(SQLStatement, sampleId);
			g.Modify(SQLStatement);
			return true;
		}

		public bool markLOTDSampleAsComplete(int sampleId)
		{
			string SQLStatement = @"
                INSERT INTO :SMPL_LOTD_COMPL_TBL_NAME 
                (
                    :SMPL_LOTD_COMPL_TBL_SAMPLE_ID, 
                    :SMPL_LOTD_COMPL_TBL_USR_ID
                ) 
                VALUES  ('{0}','1')";
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_NAME", GotchaConstants.SMPL_LOTD_COMPL_TBL_NAME);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_SAMPLE_ID", GotchaConstants.SMPL_LOTD_COMPL_TBL_SAMPLE_ID);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_USR_ID", GotchaConstants.SMPL_LOTD_COMPL_TBL_USR_ID);
			SQLStatement = string.Format(SQLStatement, sampleId);
			// localLog(SQLStatement); 
			g.Modify(SQLStatement);
			return true;
		}
		public bool updateLotdScoreForUser(int sampleId, int userID, float insertCost, float substitueCost, float deleteCost)
		{
			string SQLStatement = @"
                UPDATE  :SMPL_LOTD_COMPL_TBL_NAME
                SET     
                        :SMPL_LOTD_COMPL_TBL_I_COST = {2}, 
                        :SMPL_LOTD_COMPL_TBL_S_COST = {3}, 
                        :SMPL_LOTD_COMPL_TBL_D_COST = {4}
                WHERE   
                    :SMPL_LOTD_COMPL_TBL_SAMPLE_ID = {0} 
                    AND :SMPL_LOTD_COMPL_TBL_USR_ID = {1} 
                ";
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_NAME", GotchaConstants.SMPL_LOTD_COMPL_TBL_NAME);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_I_COST", GotchaConstants.SMPL_LOTD_COMPL_TBL_I_COST);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_S_COST", GotchaConstants.SMPL_LOTD_COMPL_TBL_S_COST);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_D_COST", GotchaConstants.SMPL_LOTD_COMPL_TBL_D_COST);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_SAMPLE_ID", GotchaConstants.SMPL_LOTD_COMPL_TBL_SAMPLE_ID);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_USR_ID", GotchaConstants.SMPL_LOTD_COMPL_TBL_USR_ID);
			SQLStatement = string.Format(SQLStatement, sampleId, userID, insertCost, substitueCost, deleteCost);
			localLog(SQLStatement);
			g.Modify(SQLStatement);
			return true;
		}

		public int AddAppliedFeatureForLotdSample(int playerId, int sampleId, AppliedFeature af)
		{
			string SQLStatement = @"
                INSERT INTO :APP_FTR_LOTD_TBL_NAME (
                    :APP_FTR_LOTD_TBL_SMPL_ID, 
                    :APP_FTR_LOTD_TBL_USR_ID, 
                    :APP_FTR_LOTD_TBL_FTR_ID,
                    :APP_FTR_LOTD_TBL_CONF,
                    :APP_FTR_LOTD_TBL_LOC_X,
                    :APP_FTR_LOTD_TBL_LOC_Y,
                    :APP_FTR_LOTD_TBL_DIRECTION
                ) 
                VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')";
			SQLStatement = SQLStatement.Replace(":APP_FTR_LOTD_TBL_NAME", GotchaConstants.APP_FTR_LOTD_TBL_NAME);
			SQLStatement = SQLStatement.Replace(":APP_FTR_LOTD_TBL_SMPL_ID", GotchaConstants.APP_FTR_LOTD_TBL_SMPL_ID);
			SQLStatement = SQLStatement.Replace(":APP_FTR_LOTD_TBL_USR_ID", GotchaConstants.APP_FTR_LOTD_TBL_USR_ID);
			SQLStatement = SQLStatement.Replace(":APP_FTR_LOTD_TBL_FTR_ID", GotchaConstants.APP_FTR_LOTD_TBL_FTR_ID);
			SQLStatement = SQLStatement.Replace(":APP_FTR_LOTD_TBL_CONF", GotchaConstants.APP_FTR_LOTD_TBL_CONF);
			SQLStatement = SQLStatement.Replace(":APP_FTR_LOTD_TBL_LOC_X", GotchaConstants.APP_FTR_LOTD_TBL_LOC_X);
			SQLStatement = SQLStatement.Replace(":APP_FTR_LOTD_TBL_LOC_Y", GotchaConstants.APP_FTR_LOTD_TBL_LOC_Y);
			SQLStatement = SQLStatement.Replace(":APP_FTR_LOTD_TBL_DIRECTION", GotchaConstants.APP_FTR_LOTD_TBL_DIRECTION);

			SQLStatement = string.Format(SQLStatement, sampleId, playerId, af.FeatureID, af.Confidence,
				af.LocationX, af.LocationY, af.Direction);
			return g.InsertReturnID(SQLStatement);
		}

		public bool markLotdSampleCompleted(int sampleId, int playerID, int verdict)
		{
			string SQLStatement = @"
                INSERT INTO :SMPL_LOTD_COMPL_TBL_NAME (
                    :SMPL_LOTD_COMPL_TBL_SAMPLE_ID, 
                    :SMPL_LOTD_COMPL_TBL_USR_ID,
                    :SMPL_LOTD_COMPL_TBL_VERDICT
                ) 
                VALUES ('{0}','{1}','{2}')";
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_NAME", GotchaConstants.SMPL_LOTD_COMPL_TBL_NAME);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_SAMPLE_ID", GotchaConstants.SMPL_LOTD_COMPL_TBL_SAMPLE_ID);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_USR_ID", GotchaConstants.SMPL_LOTD_COMPL_TBL_USR_ID);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_COMPL_TBL_VERDICT", GotchaConstants.SMPL_LOTD_COMPL_TBL_VERDICT);
			SQLStatement = string.Format(SQLStatement, sampleId, playerID, verdict);
			g.Modify(SQLStatement);
			return true;
		}

		public Sample GetLotdSampleIDbyName(string name)
		{
			string SQLStatement = @"
            SELECT 
                 :SMPL_LOTD_V_SAMPLE_ID as fp_sample_id, 
                 :SMPL_LOTD_V_IMG_NAME, 
                :SMPL_LOTD_V_AO,
                :SMPL_LOTD_V_AB
            FROM :SMPL_LOTD_V_NAME
            WHERE :SMPL_LOTD_V_IMG_NAME = '{0}.{1}' 
            OR :SMPL_LOTD_V_IMG_NAME = '{0}.{2}'";
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_V_NAME", GotchaConstants.SMPL_LOTD_V_NAME);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_V_IMG_NAME", GotchaConstants.SMPL_LOTD_V_IMG_NAME);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_V_AO", GotchaConstants.SMPL_LOTD_V_AO);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_V_AB", GotchaConstants.SMPL_LOTD_V_AB);
			SQLStatement = SQLStatement.Replace(":SMPL_LOTD_V_SAMPLE_ID", GotchaConstants.SMPL_LOTD_V_SAMPLE_ID);
			SQLStatement = string.Format(SQLStatement, name, sampleFileExtension1, sampleFileExtension2);
			// returns a single record 
			return SampleSelectStatement(SQLStatement)[0];
		}
	}
}
