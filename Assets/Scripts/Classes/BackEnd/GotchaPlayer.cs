using System;
using System.Collections.Generic;
namespace DeltaCoreBE
{

    public class GotchaPlayer
    {
        private Sample currentSample = null;
        private Player currentPlayer = null;
        private int playerID = -1;
        // private AppliedFeature currentAppliedFeature = null;
        private int currentPhase = -1;

        // List<AppliedFeature> currentAppliedFeatures;
        List<Sample> currentSampleList;

        GotchaDB gotchaDB = new GotchaDB();
        public GotchaPlayer() { }


        // Login/Logout functions 
        public int playerLoginReturnID(string user, string pass)
        {
            string logstr = String.Empty;
            // check credentials 

            // if check credenetails are good return ID otherwise return 2 

            // return player id 
            playerID = 2;
            currentPlayer = gotchaDB.GetPlayerbyID(playerID);

            logstr = String.Format("[{0}:{1}] is now logged in", currentPlayer.Id, currentPlayer.FullName);
            GotchaConstants.gotchaLog(logstr);
            return playerID;
        }


        public bool playerLogin(int id)
        {
            string logstr = String.Empty;
            if (isPlayerLoggedIn())
            {
                logstr = String.Format("Player[{0}] already logged in", currentPlayer);
                GotchaConstants.gotchaLog(logstr);
                return true;
            }

            currentPlayer = gotchaDB.GetPlayerbyID(id);
            logstr = String.Format("[{0}:{1}] is now logged in", currentPlayer.Id, currentPlayer.FullName);
            GotchaConstants.gotchaLog(logstr);
            return true;
        }

        public bool playerLogout()
        {
            string logstr = String.Empty;
            logstr = String.Format("Player[{0}] will now be logged out", currentPlayer.Id);
            GotchaConstants.gotchaLog(logstr);
            currentPlayer = null;
            return true;
        }

        public bool isPlayerLoggedIn()
        {
            string logstr = String.Empty;
            if (currentPlayer != null)
            {
                logstr = String.Format("Player {0} logged in", currentPlayer);
                GotchaConstants.gotchaLog(logstr);
                return true;
            }
            logstr = String.Format("No player logged in");
            GotchaConstants.gotchaLog(logstr);
            return false;
        }

        // Sample functions 
        /*
        private bool loadSamplesByPhase(int phaseId, int playerId)
        {
            string logstr = String.Empty;
            if (!isPlayerLoggedIn()) { return false; }

            logstr = String.Format("Loading Phase:{0} New samples for player {1}", phaseId, playerId);
            Console.WriteLine(logstr);
            switch (phaseId)
            {
                case (int)GotchaConstants.GyroPhases.NEW:
                    currentSampleList = new List<Sample>(gotchaDB.GetNewSamplesForPlayer(playerId));
                    currentPhase = phaseId;
                    return true;
                    break;

                case (int)GotchaConstants.GyroPhases.ANALYSIS:
                    currentSampleList = new List<Sample>(gotchaDB.GetAnalysisSamplesForPlayer(playerId));
                    currentPhase = phaseId;
                    return true;
                    break;

                case (int)GotchaConstants.GyroPhases.COMPARISON:
                    currentSampleList = new List<Sample>(gotchaDB.GetAnalysisSamplesForPlayer(playerId));
                    currentPhase = phaseId;
                    return true;
                    break;

                case (int)GotchaConstants.GyroPhases.EVALUATION:
                case (int)GotchaConstants.GyroPhases.VERIFICATION:
                case (int)GotchaConstants.GyroPhases.COMPLETE:
                    return true;
                    break;

                default:
                    return true;
                    break;
            }
        }


        public void loadNewSamples()
        {
            loadSamplesByPhase((int)GotchaConstants.GyroPhases.NEW);
        }

        public void loadAnalysisSamples()
        {
            loadSamplesByPhase((int)GotchaConstants.GyroPhases.ANALYSIS);
        }

        public void loadComparisonSamples()
        {
            loadSamplesByPhase((int)GotchaConstants.GyroPhases.COMPARISON);
        }


        public bool displayLoadedSamples()
        {
            if (!isPlayerLoggedIn()) { return false; }
            foreach (Sample s in currentSampleList)
            {
                Console.WriteLine(s);
            }
            return true;
        }

        public bool loadSample(Sample s)
        {
            string logstr = String.Empty;
            if (!isPlayerLoggedIn()) { return false; }

            currentSample = s;
            logstr = String.Format("Current Sample set to ({0})", currentSample);
            GotchaConstants.gotchaLog(logstr);

            //   loadAppliedFeatureList();
            return true;
        }

        public void loadSample(int id)
        {
            string logstr = String.Empty;
            logstr = String.Format("Loading Sample using ID({0})", id);
            GotchaConstants.gotchaLog(logstr);
            loadSample(gotchaDB.GetSamplebyID(id));
        }
        */
        /*
            private void loadAppliedFeatureList()
            {
                string logstr = String.Empty;
                logstr = String.Format("Loading available Features from DB for Sample({0} by Player({1})", currentSample, currentPlayer);
                GotchaConstants.gotchaLog(logstr);
                currentAppliedFeatures = new List<AppliedFeature>(gotchaDB.GetAppliedFeaturesForPlayerSample(currentPlayer, currentSample));
            }

            public bool showAppliedFeatureList()
            {
                if (!isPlayerLoggedIn()) { return false; }
                string logstr = String.Format("Printing available Applied Features");
                GotchaConstants.gotchaLog(logstr);
                if (currentAppliedFeatures.Count > 0)
                {
                    foreach (AppliedFeature afeature in currentAppliedFeatures)
                    {
                        System.Console.WriteLine(afeature);
                    }
                }
                return true;
            }

            public void selectFeature(Feature f)
            {
                currentAppliedFeature = new AppliedFeature(f);
                string logstr = String.Format("New Feature:{0} Selected", f);
                GotchaConstants.gotchaLog(logstr);
            }

            public void selectFeature(int fid)
            {
                selectFeature(new Feature(fid));
            }

            public void setAppliedFeature(AppliedFeature af)
            {
                currentAppliedFeature = af;
            }

            public void AddAppliedFeature()
            {
                if (currentAppliedFeature == null)
                {
                    Console.WriteLine("Please select applied feature settings before proceeding");
                }
                currentAppliedFeature.Player = currentPlayer;
                currentAppliedFeature.Sample = currentSample;

                string logstr = String.Format("Setting applied feature {0} to Sample {1} By {2}", currentAppliedFeature, currentSample, currentPlayer);
                GotchaConstants.gotchaLog(logstr);

                if (currentPhase == (int)GotchaConstants.GyroPhases.NEW)
                {
                    currentPhase = (int)GotchaConstants.GyroPhases.ANALYSIS;
                    gotchaDB.moveSampleToAnalysis(currentPlayer, currentSample);
                }
                gotchaDB.AddAppliedFeatureForPlayerSample(currentPlayer, currentSample, currentAppliedFeature, currentPhase);

                // Check if this is the first feature added, if so, move sample to Analysis Phase


                // reload Feature list for sample
                loadAppliedFeatureList();
            }
         */
    }
}