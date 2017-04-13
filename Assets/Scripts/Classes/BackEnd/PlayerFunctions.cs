using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
// Only for Debuging will move once Server Version is loaded
using UnityEngine;

namespace DeltaCoreBE
{
    public static class PlayerFunctions
    {
        private static bool debugOn = false;
        private static void localLog(string msg) { localLog("PlayerFunctions", msg); }
        private static void localLog(string topic, string msg)
        {
            if (debugOn)
            {
                string logEntry = string.Format("{0:F}: [{1}]{2}", System.DateTime.Now, topic, msg);
                Debug.Log(logEntry);
            }
        }

        public static GotchaDB gotchaDB = new GotchaDB();
        public static List<Sample> currentSamples = new List<Sample>();

        public static Player logPlayerIn(string user, string pass = "")
        {
            return playerLoginAttempt(user, pass);
        }
        public static Player playerLoginAttempt(string user, string pass = "")
        {
            string logstr = "";
            Player currentPlayer = gotchaDB.GetPlayerbyemail(user);

            if (currentPlayer != null)
            {
                logstr = string.Format("Succesful Log in {0}[{1}]", currentPlayer.FullName, currentPlayer.Id);
                GotchaConstants.gotchaLog(logstr);
                return currentPlayer;
            }

            logstr = string.Format("Unsuccesful Login {0}[{1}]", user, pass);
            GotchaConstants.gotchaLog(logstr);
            return null;
        }

        /*
        public static int addAppliedFeatureToSample(int playerID, int sampleID, AppliedFeature af)
        {
            return gotchaDB.AddAppliedFeatureForPlayerSample( playerID, sampleID, af );
        }
        */

        public static int addAppliedFeatureToSample(int playerID, int sampleID, int featureID, float locX, float locY, int phase, int confidence, int orientation)
        {
            AppliedFeature af = new AppliedFeature(featureID, locX, locY, phase, confidence, orientation);
            return gotchaDB.AddAppliedFeatureForPlayerSample(playerID, sampleID, af);
        }

        public static bool deleteAllAppliedFeaturesSamplePlayer(int playerID, int sampleID)
        {
            gotchaDB.deleteAllAppliedFeaturesSamplePlayer(playerID, sampleID);
            gotchaDB.removeSampleFromCompleted(playerID, sampleID);
            return true;
        }

        public static int addAppliedFeatureToSampleExpert(int playerID, int sampleID, int featureID, float locX, float locY, int phase, int confidence, int orientation)
        {
            AppliedFeature af = new AppliedFeature(featureID, locX, locY, phase, confidence, orientation);
            return gotchaDB.AddAppliedFeatureForExpertSample(playerID, sampleID, af);
        }

        public static bool deleteAllAppliedFeaturesSampleExpert(int playerID, int sampleID)
        {
            gotchaDB.deleteAllAppliedFeaturesSampleExpert(playerID, sampleID);
            gotchaDB.removeSampleFromCompleted(playerID, sampleID);
            return true;
        }
        //public static bool deleteAnalysisVerdict(int playerID, int sampleID)
        //{
        ////     return gotchaDB.deleteAnalysisVerdict(playerID, sampleID);
        //}

        public static bool markSampleAsCompleted(int playerID, int sampleID)
        {
            return gotchaDB.markSampleCompleted(playerID, sampleID);
        }

        public static bool isSampleCompleted(int playerID, int sampleID)
        {
            return gotchaDB.isSampleComplete(playerID, sampleID);
        }

        public static bool isSampleReady(int sampleID)
        {
            return gotchaDB.isSampleReady(sampleID);
        }
        public static bool removeAppliedFeatureToSample(int id)
        {
            gf.gLog("Removing " + id.ToString());
            gotchaDB.removeAppliedFeatureForPlayerSampleById(id);
            return true;
        }

        public static bool loadNewSamplesForPlayer(int playerId)
        {
            currentSamples = gotchaDB.GetNewSamplesForPlayer(playerId);
            return true;
        }

        public static List<AppliedFeature> getFeaturesForPlayerSample(int playerId, int sampleId)
        {
            return gotchaDB.GetAppliedFeaturesForPlayerSample(playerId, sampleId);
        }

        public static List<AppliedFeature> getFeaturesForExpertSample(int playerId, int sampleId)
        {
            return gotchaDB.GetAppliedFeaturesForExpertSample(playerId, sampleId);
        }

        public static List<AppliedFeature> getSolutionFeaturesForSample(int sampleId)
        {
            return gotchaDB.GetSolutionAppliedFeaturesForPlayerSample(sampleId);
        }

        public static int getSampleIDfromName(string name)
        {
            return gotchaDB.GetSampleIDbyName(name).Id;
        }

        public static int addAnalysisVerdict(int playerID, int sampleID, int verdict)
        {
            return gotchaDB.addAnalysisVerdictForSample(playerID, sampleID, verdict);
        }

        // LOTD Functions 
        public static DataTable getLotdSamplesForUser(int playerID)
        {
            localLog("Getting LOTD Samples");
            return gotchaDB.GetLOTDSamplesForUser(playerID);
        }

        public static DataTable getLotdSampleForUser(int playerID)
        {
            localLog("Getting LOTD Sample");
            return gotchaDB.GetLOTDSampleForUser(playerID);
        }

        public static bool hasPlayerCompleterLOTD(int playerID)
        {
            int res = gotchaDB.playerCompletedLatentOfTheDay(playerID);
            localLog(string.Format("Player Completed LOTD : {0}", res));
            if (res == -1) { return false; }
            else { return true; }
        }

        public static int addAppliedFeatureToLotdSample(int playerID, int sampleID, int featureID, float locX, float locY, int phase, int confidence, int orientation)
        {
            AppliedFeature af = new AppliedFeature(featureID, locX, locY, phase, confidence, orientation);
            return gotchaDB.AddAppliedFeatureForLotdSample(playerID, sampleID, af);
        }

        public static bool markLotdSampleAsCompleted(int sampleID, int playerID, int verdict)
        {
            return gotchaDB.markLotdSampleCompleted(sampleID, playerID, verdict);
        }

        public static int getLotdSampleIDfromName(string name)
        {
            return gotchaDB.GetLotdSampleIDbyName(name).Id;
        }

    }

}