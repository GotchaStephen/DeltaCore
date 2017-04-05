using System;
using System.Data ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DeltaCoreBE; 

public static class Database {


    public static FingerprintLevel latentOfTheDayID;

    //Debug variable
    static LevelData lastLevelData;
    

    //Debug Variable
    static Dictionary<string, LevelData> levelDictionary = new Dictionary<string, LevelData>();

    private static bool debugOn = true;
    private static void localLog(string msg = "No message") { localLog("Database", msg); }
    private static void localLog(string topic, string msg)
    {
        if (debugOn)
        {
            string logEntry = string.Format("{0:F}: [{1}]{2}", System.DateTime.Now, topic, msg);
            Debug.Log(logEntry);
        }
    }

    public static bool Login(string username="mahmoud.shadid@gmail.com", string password = "") {
        Init();
        UserInfo.Reset();
        Player playerinfo = PlayerFunctions.playerLoginAttempt(username, password);
        UserInfo.id= playerinfo.Id ;
        UserInfo.username = playerinfo.Email ;
        UserInfo.firstName = playerinfo.FirstName ;
        UserInfo.lastName = playerinfo.LastName;
        if (playerinfo.Role == 1) { UserInfo.userType = UserInfo.UserType.Expert; }
        else if (playerinfo.Role == 2) { UserInfo.userType = UserInfo.UserType.NormalUser; }
        else { UserInfo.userType = UserInfo.UserType.Guest ;  }
        
        string s = string.Format("{0}:{1}({2}) has logged on", UserInfo.id, UserInfo.username, UserInfo.userType);
        Debug.Log(s);
        return true;
    }


    private static int getIndexForFileSprite(Sprite[] list, string fileName)
    {
        for (int i = 0; i < list.Length; i++)
            if (list[i].name == fileName) { return i; }
        return -1;
    }

    public static ArrayList DownloadLevels() {
        ArrayList levels = new ArrayList();

        //Load LOTD Levels
        string dirName = string.Format("FingerPrintLevels/LOTD/{0:yyyy}/{0:MM}", DateTime.Now);
        Sprite[] lotdLevels = Resources.LoadAll<Sprite>(dirName);

        foreach (DataRow row in PlayerFunctions.getLotdSamplesForUser(UserInfo.id).Rows)
        {
            int sampleID = (int)row["sample_id"];
            string sampleFullName = row["fp_image_name"].ToString();
            int index = getIndexForFileSprite(lotdLevels, sampleFullName.Split('.')[0]);

            bool hasbeenAnalysed = false;
            bool lotdForToday = false;

            if (!string.IsNullOrEmpty(row["verdict"].ToString())) { hasbeenAnalysed = true; }
            if (Convert.ToDateTime(row["used_date"]).Date == DateTime.Now.Date) { lotdForToday = true; }

            if ( lotdForToday )
            {
                if ( hasbeenAnalysed) { UserInfo.completedLOTD = true; }
                else { UserInfo.completedLOTD = false; }
                latentOfTheDayID = new FingerprintLevel(lotdLevels[index].name, lotdLevels[index], 0, DeltaCore.LevelDifficulty.Hard, DeltaCore.FingerPrintQuality.MediumLow, DeltaCore.GameMode.LatentOfTheDay, !UserInfo.completedLOTD, sampleID);
            }
            // localLog(string.Format("Sample {0}:{1} Analysed:{2} ", sampleID, sampleFullName, hasbeenAnalysed));
            if (!hasbeenAnalysed)
            {
                FingerprintLevel level = new FingerprintLevel(lotdLevels[index].name, lotdLevels[index], 0, DeltaCore.LevelDifficulty.Hard, DeltaCore.FingerPrintQuality.MediumLow, DeltaCore.GameMode.LatentOfTheDay, true, sampleID);
                levels.Add(level);
            }
        }

        Debug.Log(string.Format("Loaded {0} Available", levels.Count));

        //Return
        return levels;

    }


    public static void eraseLevelData(LevelData levelData)
    {
        int sampleId = levelData.level.sampleId;
        string s = string.Format("Erasing Marker(s) for {0}:{1}({2}) on Sample[{3}]", UserInfo.id, UserInfo.username, UserInfo.userType, sampleId);
        Debug.Log(s);

        if (UserInfo.userType == UserInfo.UserType.Expert)
        {
            PlayerFunctions.deleteAllAppliedFeaturesSampleExpert(UserInfo.id, sampleId) ;
        }
        else
        {
            PlayerFunctions.deleteAllAppliedFeaturesSamplePlayer(UserInfo.id, sampleId);
        }
    }

    public static void SaveLevelData(LevelData levelData) {

        #region Testing Saving to Database

        // Removing existing markers 
        eraseLevelData(levelData);

        if ( levelData.level.id == latentOfTheDayID.id)
        {
            UserInfo.completedLOTD = true; 
        }
        int sampleID = levelData.level.sampleId ;
        int phase = -1;

        string s = string.Format("Saving Marker(s) for {0}:{1}({2}) on Sample[{3}]", UserInfo.id, UserInfo.username, UserInfo.userType, sampleID);
        Debug.Log(s);
        foreach (MarkerData m in levelData.markers)
        {
            int confidence = (int) m.confidenceLevel;
            int orientation = (int) m.orientation;
            int featureID = (int) m.type;

            if (UserInfo.currentGameMode == DeltaCore.GameMode.LatentOfTheDay)
            {
                localLog("Adding to LOTD");
                PlayerFunctions.addAppliedFeatureToLotdSample(UserInfo.id, sampleID, featureID, m.position.x, m.position.y, phase, confidence, orientation);
            }
            else
            {
                if (UserInfo.userType == UserInfo.UserType.Expert)
                {
                    localLog("Adding to Expert");
                    PlayerFunctions.addAppliedFeatureToSampleExpert(UserInfo.id, sampleID, featureID, m.position.x, m.position.y, phase, confidence, orientation);
                }
                else
                {
                    localLog("Adding to PLayer");
                    PlayerFunctions.addAppliedFeatureToSample(UserInfo.id, sampleID, featureID, m.position.x, m.position.y, phase, confidence, orientation);
                }
            }
        }
        if (UserInfo.currentGameMode == DeltaCore.GameMode.LatentOfTheDay)
        {
            localLog("Adding Descion");
            PlayerFunctions.markLotdSampleAsCompleted( sampleID, UserInfo.id, (int)levelData.decision);
        }
        else
        {
            localLog("Adding Descion Player");
            PlayerFunctions.markSampleAsCompleted(UserInfo.id, sampleID);
        }

        
        #endregion

        if (levelDictionary.ContainsKey(levelData.level.id)) {
            levelDictionary[levelData.level.id] = levelData;
        }
        else {
            levelDictionary.Add(levelData.level.id, levelData);
        }
    }

    public static void LoadLevelDataPlayerMarkers(FingerprintLevel level, ref ArrayList playerMarker)
    {
        List<AppliedFeature> appliedFeatureList = PlayerFunctions.getFeaturesForPlayerSample(UserInfo.id, level.sampleId);
        foreach (AppliedFeature af in appliedFeatureList)
        {
            float lX = af.LocationX;
            float lY = af.LocationY;
            bool hasOrientation = false;
            if (af.Direction != 0) { hasOrientation = true; }
            playerMarker.Add(new MarkerData(lX, lY, (DeltaCore.MarkerType)af.FeatureID, (DeltaCore.MarkerConfidence)af.Confidence, hasOrientation, af.Direction));
        }
    }

    public static LevelData LoadLevelData(FingerprintLevel level) {

        LevelData loadedLevelData; 
        string s = "";
        localLog(string.Format("GameMode:{0}", UserInfo.currentGameMode)); 
        if ( UserInfo.currentGameMode == DeltaCore.GameMode.LatentOfTheDay)
        {
            level.sampleId = PlayerFunctions.getLotdSampleIDfromName(level.id);
        }
        else
        {
            level.sampleId = PlayerFunctions.getSampleIDfromName(level.id);
        }
        
        localLog(string.Format("Loading [{0}:{1}]", level.sampleId, level.id)) ; 
        if (levelDictionary.ContainsKey(level.id)){ loadedLevelData = levelDictionary[level.id] ;}
        else{loadedLevelData = new LevelData(level);}

        loadedLevelData.completed = PlayerFunctions.isSampleCompleted(UserInfo.id, level.sampleId);
        loadedLevelData.ready = PlayerFunctions.isSampleReady(level.sampleId);

        // Loading solution markers if ready 
        if (loadedLevelData.ready)
        {
            List<AppliedFeature> appliedFeatureList = PlayerFunctions.getSolutionFeaturesForSample(level.sampleId);
            s = string.Format("{0} features found on Ready sample[{1}:{2}]", appliedFeatureList.Count, level.sampleId, level.id);
            Debug.Log(s);
            loadedLevelData.scoreData.clearsolutionMarkers();
            foreach (AppliedFeature af in appliedFeatureList)
            {
                float lX = af.LocationX;
                float lY = af.LocationY;
                bool hasOrientation = false;
                if (af.Direction != 0) { hasOrientation = true; }
                loadedLevelData.scoreData.solutionMarkers.Add(new MarkerData(lX, lY, (DeltaCore.MarkerType)af.FeatureID, (DeltaCore.MarkerConfidence)af.Confidence, hasOrientation, af.Direction));
            }
        }

        // Adding markers if completed
        if (loadedLevelData.completed)
        {
            loadedLevelData.markers.Clear() ; 
            List<AppliedFeature> appliedFeatureList ;
            if (UserInfo.userType == UserInfo.UserType.Expert)
            {
                appliedFeatureList = PlayerFunctions.getFeaturesForExpertSample(UserInfo.id, level.sampleId);
            }
            else
            {
                LoadLevelDataPlayerMarkers(level, ref loadedLevelData.markers) ;
                return loadedLevelData ; 
            }

            s = string.Format("{0} features found on Completed sample[{1}:{2}]", appliedFeatureList.Count, level.sampleId, level.id);
            Debug.Log(s);
            foreach (AppliedFeature af in appliedFeatureList)
            {
                float lX = af.LocationX ;
                float lY = af.LocationY ;
                bool hasOrientation = false; 
                if (af.Direction != 0){ hasOrientation = true;  }
                loadedLevelData.markers.Add(new MarkerData(lX, lY, (DeltaCore.MarkerType)af.FeatureID, (DeltaCore.MarkerConfidence)af.Confidence, hasOrientation, af.Direction)); 
            }
        }
        return loadedLevelData ;
    }


    private static void Init() {

    }
}
