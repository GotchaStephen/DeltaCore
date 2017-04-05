using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace DeltaCoreBE
{

    public static class GotchaConstants
    {
        private static bool debugOn = true;
        private static void localLog(string msg) { localLog("GotchaConstants", msg); }
        private static void localLog(string topic, string msg)
        {
            if (debugOn)
            {
                string logEntry = string.Format("{0:F}: [{1}]{2}", System.DateTime.Now, topic, msg);
                Console.WriteLine(logEntry);
            }
        }

        // LOGGING 
        public static bool ENABLE_LOGGING = true;

        public static string nvStr(string name, string value)
        {
            return String.Format("{0}[{1}]", name, value);
        }

        public static string nvStr(string name, int value)
        {
            return String.Format("{0}[{1}]", name, value);
        }

        public static void gotchaLog(string s)
        {
            if (ENABLE_LOGGING)
            {
                // Console.WriteLine(s);
                // Debug.Log(s);
            }
        }

        public static string getSamplepath(string imageName)
        {
            return sampleFolder + "\\" + imageName;
        }


        // DB TEST LOCAL

        // public const string GOTCHA_HOST = "127.0.0.1";
        // public const string GOTCHA_USERNAME = "root";
        // public const string GOTCHA_PASSWORD = "1$w@Vg008!";
        // public const string GOTCHA_DBNAME = "dc_data";


        // BD Production 

        public const string GOTCHA_HOST = "130.56.248.86";
        public const string GOTCHA_USERNAME = "dcremote";
        public const string GOTCHA_PASSWORD = "1qazxcde3@WS";
        public const string GOTCHA_DBNAME = "playertw_deltacore";


        public const int DEFAULT_ADDER_USER_ID = -1;

        // USER
        // >> Table constants
        public const string USR_TBL_NAME = GOTCHA_DBNAME + ".users";
        public const string USR_TBL_KEY = "id";
        public const string USR_TBL_ROLE = "role";
        public const string USR_TBL_STATUS = "status";
        public const string USR_TBL_FIRSTNAME = "first_name";
        public const string USR_TBL_LASTNAME = "last_name";
        public const string USR_TBL_EMAIL = "email";
        public const string USR_TBL_APPROVER = "approver";
        public const string USR_TBL_SALT = "pw_salt";
        public const string USR_TBL_HASH = "pw_hash";
        public const string USR_TBL_PW_RESET = "pw_reset_hash";
        public const string USR_TBL_PW_RESET_TS = "pw_reset_hash_ts";
        public const string USR_TBL_AO = "added_on";
        public const string USR_TBL_AB = "added_by";

        // SAMPLES
        private const string sampleFolder = "FPSamples";
        // >> Table constants
        public const string SMPL_TBL_NAME = GOTCHA_DBNAME + ".samples";
        public const string SMPL_TBL_KEY = "fp_sample_id";
        public const string SMPL_TBL_IMG_NAME = "fp_image_name";
        public const string SMPL_TBL_AO = "added_on";
        public const string SMPL_TBL_AB = "added_by";

        // FEATURES
        public const string FTR_TBL_NAME = GOTCHA_DBNAME + ".features";
        public const string FTR_TBL_KEY = "id";
        public const string FTR_TBL_IMG_NAME = "image_name";
        public const string FTR_TBL_SMPL_TYPE = "sample_type";

        public enum FingerprintFeatures
        {
            BIFURCATION = 1,
            RIDGE_ENDING,
            SHORT_RIDGE,
        };

        // READY SAMPLES EXPERT
        public const string APP_FTR_RDY_TBL_NAME = GOTCHA_DBNAME + ".v_ready_samples";
        public const string APP_FTR_RDY_TBL_KEY = "id";
        public const string APP_FTR_RDY_TBL_SMPL_ID = "sample_id";
        public const string APP_FTR_RDY_TBL_FTR_ID = "feature_id";
        public const string APP_FTR_RDY_TBL_CONF = "confidence";
        public const string APP_FTR_RDY_TBL_LOC_X = "location_x";
        public const string APP_FTR_RDY_TBL_LOC_Y = "location_y";
        public const string APP_FTR_RDY_TBL_DIRECTION = "direction";
        public const string APP_FTR_RDY_TBL_AO = "added_on";


        // APPLIED FEATURES PLAYER
        public const string APP_FTR_TBL_NAME = GOTCHA_DBNAME + ".sample_analysis_player";
        public const string APP_FTR_TBL_KEY = "id";
        public const string APP_FTR_TBL_SMPL_ID = "sample_id";
        public const string APP_FTR_TBL_USR_ID = "user_id";
        public const string APP_FTR_TBL_FTR_ID = "feature_id";
        public const string APP_FTR_TBL_PHS_ID = "phase_id";
        public const string APP_FTR_TBL_CONF = "confidence";
        public const string APP_FTR_TBL_LOC_X = "location_x";
        public const string APP_FTR_TBL_LOC_Y = "location_y";
        public const string APP_FTR_TBL_DIRECTION = "direction";
        public const string APP_FTR_TBL_AO = "added_on";

        // APPLIED FEATURES EXPERT
        public const string APP_FTR_EXP_TBL_NAME = GOTCHA_DBNAME + ".sample_analysis_expert";
        public const string APP_FTR_EXP_TBL_KEY = "id";
        public const string APP_FTR_EXP_TBL_SMPL_ID = "sample_id";
        public const string APP_FTR_EXP_TBL_USR_ID = "user_id";
        public const string APP_FTR_EXP_TBL_FTR_ID = "feature_id";
        public const string APP_FTR_EXP_TBL_CONF = "confidence";
        public const string APP_FTR_EXP_TBL_LOC_X = "location_x";
        public const string APP_FTR_EXP_TBL_LOC_Y = "location_y";
        public const string APP_FTR_EXP_TBL_DIRECTION = "direction";
        public const string APP_FTR_EXP_TBL_AO = "added_on";

        // (VIEW) LOTD SAMPLES
        public const string SMPL_LOTD_V_NAME = GOTCHA_DBNAME + ".v_samples_lotd";
        public const string SMPL_LOTD_V_SAMPLE_ID = "sample_id";
        public const string SMPL_LOTD_V_USED_DATE = "used_date";
        public const string SMPL_LOTD_V_IMG_NAME = "fp_image_name";
        public const string SMPL_LOTD_V_AO = "added_on";
        public const string SMPL_LOTD_V_AB = "added_by";


        // LOTD COMPLETED SAMPLES
        public const string SMPL_LOTD_COMPL_TBL_NAME = GOTCHA_DBNAME + ".samples_completed_lotd";
        public const string SMPL_LOTD_COMPL_TBL_KEY = "id";
        public const string SMPL_LOTD_COMPL_TBL_USR_ID = "user_id";
        public const string SMPL_LOTD_COMPL_TBL_SAMPLE_ID = "sample_id";
        public const string SMPL_LOTD_COMPL_TBL_SCORE = "score";
        public const string SMPL_LOTD_COMPL_TBL_VERDICT = "verdict";
        public const string SMPL_LOTD_COMPL_TBL_AO = "added_on";
        public const string SMPL_LOTD_COMPL_TBL_AB = "added_by";

        // Solution Applied Features Expert 
        public const string APP_FTR_SOL_LOTD_TBL_NAME = GOTCHA_DBNAME + ".samples_solution_lotd";
        public const string APP_FTR_SOL_LOTD_TBL_KEY = "id";
        public const string APP_FTR_SOL_LOTD_TBL_SMPL_ID = "sample_id";
        public const string APP_FTR_SOL_LOTD_TBL_FTR_ID = "feature_id";
        public const string APP_FTR_SOL_LOTD_TBL_CONF = "confidence";
        public const string APP_FTR_SOL_LOTD_TBL_LOC_X = "location_x";
        public const string APP_FTR_SOL_LOTD_TBL_LOC_Y = "location_y";
        public const string APP_FTR_SOL_LOTD_TBL_DIRECTION = "direction";
        public const string APP_FTR_SOL_LOTD_TBL_WEIGHT = "weight";
        public const string APP_FTR_SOL_LOTD_TBL_AO = "added_on";


        // APPLIED FEATURES LOTD
        public const string APP_FTR_LOTD_TBL_NAME = GOTCHA_DBNAME + ".samples_analysis_lotd";
        public const string APP_FTR_LOTD_TBL_KEY = "id";
        public const string APP_FTR_LOTD_TBL_SMPL_ID = "sample_id";
        public const string APP_FTR_LOTD_TBL_USR_ID = "user_id";
        public const string APP_FTR_LOTD_TBL_FTR_ID = "feature_id";
        public const string APP_FTR_LOTD_TBL_CONF = "confidence";
        public const string APP_FTR_LOTD_TBL_LOC_X = "location_x";
        public const string APP_FTR_LOTD_TBL_LOC_Y = "location_y";
        public const string APP_FTR_LOTD_TBL_DIRECTION = "direction";
        public const string APP_FTR_LOTD_TBL_AO = "added_on";



        // COMPLETED PHASE 
        public const string SMPL_PHS_TBL_NAME = GOTCHA_DBNAME + ".sample_completed";
        public const string SMPL_PHS_TBL_SMPL_ID = "sample_id";
        public const string SMPL_PHS_TBL_USR_ID = "user_id";
        public const string SMPL_PHS_TBL_AO = "added_on";

        // SAMPLE in CURRENT PHASE 
        public const string SMPL_PHS_VIEW_NAME = GOTCHA_DBNAME + ".v_sample_current_phase";
        public const string SMPL_PHS_VIEW_SMPL_ID = "sample_id";
        public const string SMPL_PHS_VIEW_PHS_ID = "current_phase";
        public const string SMPL_PHS_VIEW_USR_ID = "user_id";
        public const string SMPL_PHS_VIEW_TBL_AO = "added_on";

        // ANALYSIS VERDICTED
        public const string ANALYSIS_VER_TBL_NAME = GOTCHA_DBNAME + ".analysis_verdict";
        public const string ANALYSIS_VER_TBL_SMPL = "sample_id";
        public const string ANALYSIS_VER_TBL_VER = "verdict";
        public const string ANALYSIS_VER_TBL_USR = "user_id";
        public const string ANALYSIS_VER_TBL_AO = "added_on";
        public const string ANALYSIS_VER_TBL_AB = "user_id";

        // COMPLETED PHASE-ANALYSIS
        public const string PHS_ANALYSIS_TBL_NAME = GOTCHA_DBNAME + ".phase_analysis";
        public const string PHS_ANALYSIS_TBL_SMPL = "sample_id";
        public const string PHS_ANALYSIS_TBL_VER = "verdict";
        public const string PHS_ANALYSIS_TBL_USR = "user_id";
        public const string PHS_ANALYSIS_TBL_AO = "added_on";
        public const string PHS_ANALYSIS_TBL_AB = "user_id";

        // PHASE-COMPARISON
        public const string PHS_COMPARISON_TBL_NAME = GOTCHA_DBNAME + ".phase_comparison";
        public const string PHS_COMPARISON_TBL_SMPL = "sample_id";
        public const string PHS_COMPARISON_TBL_USR = "user_id";
        public const string PHS_COMPARISON_TBL_AO = "added_on";
        public const string PHS_COMPARISON_TBL_AB = "user_id";

        // HISTORY 
        public const string SMPL_HSTR_TBL_NAME = GOTCHA_DBNAME + ".SAMPLE_HISTORY";
        public const string SMPL_HSTR_TBL_SMPL = "sample_id";
        public const string SMPL_HSTR_TBL_ACTION = "action";
        public const string SMPL_HSTR_TBL_DET = "details";
        public const string SMPL_HSTR_TBL_AO = "added_on";
        public const string SMPL_HSTR_TBL_AB = "added_by";
    }
}