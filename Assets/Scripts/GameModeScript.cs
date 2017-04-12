using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameModeScript : MonoBehaviour, IPointerClickHandler
{

	private static GameModeScript instance;
	private string sceneName = "NotSet";
    public DeltaCore.GameMode selectedGameMode;

    public static bool debugOn = true;
    private static void localLog(string msg = "No message") { localLog("GameModeScript", msg); }
    private static void localLog(string topic, string msg)
    {
        if (debugOn)
        {
            string logEntry = string.Format("{0:F}: [{1}]{2}", System.DateTime.Now, topic, msg);
            Debug.Log(logEntry);
        }
    }



    public void OnPointerClick(PointerEventData eventData)
    {
        localLog(string.Format("Loading {0}", selectedGameMode));
        if (UserInfo.id == 0) {
			localLog(string.Format("logging Guest"));
			Database.Login(); 
		}
        UserInfo.currentGameMode = selectedGameMode ;
        switch (selectedGameMode)
        {
            case DeltaCore.GameMode.NoGameMode:
                loadGameModeScene(); 
                break;
		
			case DeltaCore.GameMode.WelcomeScreen:
				loadGameModeScene(); 
				break;

			case DeltaCore.GameMode.Tutorial:
				loadTutorialScene();
                break;

            case DeltaCore.GameMode.Training:
                loadTrainingScene(); 
                break;

            case DeltaCore.GameMode.Caseworks:
                break;

            case DeltaCore.GameMode.ColdCases:
                break;

            case DeltaCore.GameMode.LatentOfTheDay:
                loadLOTDScene();
                break;

            default:
                localLog("Game Mode not found");
                break ; 
        }

    }
    
	static public void loadGameModeScene()
    {
        UserInfo.currentGameMode = DeltaCore.GameMode.NoGameMode;
		instance.sceneName = "02_WelcomeScreen-GameMode";
		SceneManager.LoadScene(instance.sceneName);
    }

	static public void loadTutorialScene()
	{
		UserInfo.currentGameMode = DeltaCore.GameMode.NoGameMode;
		instance.sceneName = "05_Introvideo" ;
		SceneManager.LoadScene(instance.sceneName);
	}

	static public void loadTrainingScene()
    {
        UserInfo.currentGameMode = DeltaCore.GameMode.Training;
		instance.sceneName = "21_TrainingLevelSelectScreen";
		SceneManager.LoadScene(instance.sceneName);
    }

    static public void loadLOTDScene()
    {
       	UserInfo.currentGameMode = DeltaCore.GameMode.LatentOfTheDay; 
		instance.sceneName = "51_LotdLevelSelectScreen"; 
		SceneManager.LoadScene(instance.sceneName);
   	}
    
    void Start () {
		instance = this;
	}
	void Update () {}
}
