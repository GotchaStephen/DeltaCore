using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DeltaCoreBE;

public class AnalyseScreenScript : MonoBehaviour
{
    [SerializeField]
    public static bool debugOn = true;
    private static void localLog(string msg = "No message") { localLog("AnalyseScreenScript", msg); }
    private static void localLog(string topic, string msg)
    {
        if (debugOn)
        {
            string logEntry = string.Format("{0:F}: [{1}]{2}", System.DateTime.Now, topic, msg);
            Debug.Log(logEntry);
        }
    }

    private static AnalyseScreenScript instance;
    public static LevelData currentLevel;

    public GameObject fingerPrint;
    public GameObject featureMarkerPrefab;
    public InputField notesInputField;

    public Text userMessageText;
    public Button submitButton;
    public DeltaCore.AnalysisDecision descision;

    // Use this for initialization
    void Start()
    {
        instance = this;
        Load();
        descision = DeltaCore.AnalysisDecision.NotSet;
        UserInfo.lastAction = UserInfo.UserAction.EnterLevel;
        currentLevel.LastLevelAction = DeltaCore.UserLevelAction.NoAction;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Load()
    {
        //fingerPrint.GetComponent<ImageProcessingController>().setSprite(CurrentLevelState.currentLevel.fingerPrint);

        //load fingerprint
        // Testing on a certain level 
        fingerPrint.GetComponent<ImageProcessingController>().setSprite(currentLevel.level.fingerPrint);
        fingerPrint.GetComponent<ImageProcessingController>().Reset();
        localLog(string.Format("Loading Sample[{0}:{1}]", currentLevel.level.sampleId, currentLevel.level.id));
        //Load markers
        if (UserInfo.currentGameMode == DeltaCore.GameMode.LatentOfTheDay)
        {
            // dont load previos markers
        }
        else
        {
            foreach (MarkerData md in currentLevel.markers)
            {
                GameObject marker = Instantiate(featureMarkerPrefab, fingerPrint.transform);
                marker.GetComponent<FeatureMarker>().Init(md);
            }
        }

        string notesHolder = "";
        // currentLevel.updateLevelData();
        notesInputField.text = notesHolder;

        //DEBUG
        FindObjectOfType<Debug_SecondarySideMenuManager>().Set(5);
    }

    public void QuitAnalysis()
    {
        currentLevel = null;
        instance = null;
        if ( UserInfo.currentGameMode == DeltaCore.GameMode.LatentOfTheDay)
        {
			GameModeScript.loadLOTDScene();
        }
        else if (UserInfo.currentGameMode == DeltaCore.GameMode.Training)
        {
            SceneManager.LoadScene("12_TrainingLevelSelectScreen");
        }
        else
        {
            UserInfo.currentGameMode = DeltaCore.GameMode.NoGameMode; 
            SceneManager.LoadScene("02_WelcomeScreen-GameMode");
        }
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
            if (!marker.isInPlacingMode) { currentLevel.markers.Add(new MarkerData(marker)); }
        }
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

    public void updateDescision(int playerDescision)
    {
        descision = (DeltaCore.AnalysisDecision)playerDescision;
    }
    public void SubmitAnalysis()
    {
        if (descision == DeltaCore.AnalysisDecision.NotSet)
        {
            instance.userMessageText.text = "Need to set Descion";
            localLog("Desicion Not Set");
        }
        else
        {
            instance.SaveMarkers();
            localLog("Saving to Database");
            Database.SaveLevelData(currentLevel);
            UserInfo.lastAction = UserInfo.UserAction.Submit;
            currentLevel.completed = true;
            QuitAnalysis();
        }
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
        // currentLevel.updateLevelData();

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
                localLog("Action", string.Format("Bug Detected @ index[{0}]", counter));
            }
            counter++;
        }
        foreach (int index in buggyElements)
        {
            if (buggyElements.Count > index && index < 0) { currentLevel.markers.RemoveAt(index); }
        }
    }


    public static void updateAction(DeltaCore.UserLevelAction action, GameObject affectedObject)
    {
        if (!currentLevel.completed)
        {
            UserInfo.lastAction = UserInfo.UserAction.LevelAction;
            currentLevel.LastLevelAction = action;
            instance.SaveMarkers();
            if (action == DeltaCore.UserLevelAction.RemoveMarker) { fixfirstDeleteBug(affectedObject); }

             //currentLevel.updateLevelData();
        }
    }

}