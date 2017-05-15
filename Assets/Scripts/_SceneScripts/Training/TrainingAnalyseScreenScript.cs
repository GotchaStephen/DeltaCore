using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DeltaCoreBE;



public class TrainingAnalyseScreenScript : MonoBehaviour
{
    [SerializeField]
    const string className = "TrainingAnalyseScreenScript" ; 
    public bool localOn = true;
    public bool globalOn = true;
    private void localLog(string msg, string topic = "L:" + className){ if (localOn)  { logMsg(msg, topic); }}
    private void globalLog(string msg, string topic = "G:" + className){ if (globalOn) { logMsg(msg, "G:" + className); }}
    private void logMsg(string msg, string topic)
    {
        string logEntry = string.Format("{0:F}: [{1}] {2}", System.DateTime.Now, topic, msg);
        Debug.Log(logEntry);
    }

    private static TrainingAnalyseScreenScript instance;
    private FingerPrintTrainingGameManager trainingGM ; 

    public static LevelData currentLevel;

    public GameObject fingerPrint;
    public GameObject featureMarkerPrefab;
    public InputField notesInputField;

    public Text userMessageText;
    
    // Use this for initialization
    void Start()
    {
        instance = this;
        Load();
        UserInfo.lastAction = UserInfo.UserAction.EnterLevel;
        // currentLevel.LastLevelAction = DeltaCore.UserLevelAction.NoAction;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Load()
    {
        localLog(String.Format("Loading {0}", currentLevel));
        
        //load fingerprint
        // Testing on a certain level 
        fingerPrint.GetComponent<ImageProcessingController>().setSprite(currentLevel.level.fingerPrint);
        fingerPrint.GetComponent<ImageProcessingController>().Reset();
        //Load markers
        foreach (MarkerData md in currentLevel.markers)
        {
            GameObject marker = Instantiate(featureMarkerPrefab, fingerPrint.transform);
            marker.GetComponent<FeatureMarker>().Init(md);
        }
        localLog("# currentLevel markers " + currentLevel.markers.Count.ToString());
        localLog("# solution markers " + currentLevel.solutionPoints.Count.ToString());
        // Loading Markers ( User and Solution )
        trainingGM = new FingerPrintTrainingGameManager(currentLevel.markers, currentLevel.solutionPoints) ;

        string notesHolder = "";
        notesInputField.text = notesHolder;

        //DEBUG
        FindObjectOfType<Debug_SecondarySideMenuManager>().Set(5);
    }

    public void QuitAnalysis()
    {
        currentLevel = null;
        instance = null;
        GameModeScript.loadTrainingScene(); 
    }

    public static void LogAction(string action, string tag)
    {
        action = "<" + tag + "> " + System.DateTime.Now.ToString() + " " + action + "\n";
        currentLevel.LogAction(action);
    }
    public static void LogAction(string action)
    {
        LogAction(action, "untagged");
    }

    public void SaveMarkers()
    {
        currentLevel.resetMarkers();
        
        foreach (FeatureMarker marker in fingerPrint.GetComponentsInChildren<FeatureMarker>())
        {
            if (marker.placed) { currentLevel.markers.Add(new MarkerData(marker)); }
            // if (!marker.isInPlacingMode) { currentLevel.markers.Add(new MarkerData(marker)); }
            //   currentLevel.markers.Add(new MarkerData(marker)) ;
        }
        localLog("# currentLevel markers " + currentLevel.markers.Count.ToString());
    }
    public void DeleteMarkers()
    {
        foreach (FeatureMarker marker in fingerPrint.GetComponentsInChildren<FeatureMarker>())
        {
            if (!marker.isInPlacingMode)
                currentLevel.markers.Add(new MarkerData(marker));
            GameObject.DestroyImmediate(marker.gameObject);
        }
    }

    public void UpdateUserNotes(string notes)
    {
        currentLevel.UpdateUserNotes(notes);
    }

    public static void SaveLevel()
    {
        instance.SaveMarkers();
        Database.SaveLevelData(currentLevel);
        UserInfo.lastAction = UserInfo.UserAction.Save;
    }

    public void SubmitAnalysis()
    {
        instance.SaveMarkers();
        localLog("Saving to Database");
        Database.SaveLevelData(currentLevel);
        UserInfo.lastAction = UserInfo.UserAction.Submit;
        currentLevel.completed = true;
        QuitAnalysis();
    }
    public void ResetAll()
    {
        currentLevel.completed = false;

        // Remove From Screen 
        instance.DeleteMarkers();

        // Remove from Level Data 
        currentLevel.resetMarkers();

        // Remove from DataBase 
        Database.eraseLevelData(currentLevel);

        // Update Score 
        // currentLevel.updateLevelData();
        trainingGM.updatePlayerData(currentLevel.markers);
        UserInfo.lastAction = UserInfo.UserAction.ResetAll;
    }

    public void ResetLastSaved()
    {
        // Remove From Screen 
        instance.DeleteMarkers();

        // Remove from Level Data 
        currentLevel.resetMarkers();

        // Reload Saved Data to Current Level
        Database.LoadLevelDataPlayerMarkers(currentLevel.level, ref currentLevel.markers);

        // Draw initial data on screen
        Load();

        // Update Score 
//         currentLevel.updateLevelData();
        trainingGM.updatePlayerData(currentLevel.markers);
        UserInfo.lastAction = UserInfo.UserAction.ResettoLastSaved;
    }

    private static void fixfirstDeleteBug(GameObject affectedObject)
    {
        Vector2 affectedObjectPosition = new Vector2(affectedObject.transform.localPosition.x, affectedObject.transform.localPosition.y);
        List<int> buggyElements = new List<int>();
        int counter = 0;
        foreach (MarkerData marker in currentLevel.markers)
        {
            if (marker.position == affectedObjectPosition)
            {
                buggyElements.Add(counter);
                instance.localLog(string.Format("Bug Detected @ index[{0}]", counter));
            }
            counter++;
        }
        foreach (int index in buggyElements)
        {
            if (buggyElements.Count > index && index < 0) { currentLevel.markers.RemoveAt(index); }
        }
    }

    public void updateScoreData()
    {
        trainingGM.updatePlayerData(currentLevel.markers);
        globalLog("Last Action Text " + trainingGM.PastActionText);
        globalLog("Hint " + trainingGM.getHintText());
    }

    public static void updateAction(DeltaCore.UserLevelAction action, GameObject affectedObject)
    {
        if (!currentLevel.completed)
        {
            UserInfo.lastAction = UserInfo.UserAction.LevelAction;
            currentLevel.LastLevelAction = action;
            instance.SaveMarkers();
            if (action == DeltaCore.UserLevelAction.RemoveMarker) { fixfirstDeleteBug(affectedObject); }
            // currentLevel.updateLevelData();
            instance.updateScoreData();
            instance.localLog("# currentLevel markers " + currentLevel.markers.Count.ToString());
        }
    }

}