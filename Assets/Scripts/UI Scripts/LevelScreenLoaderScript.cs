using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelScreenLoaderScript : MonoBehaviour {

    public bool debugOn = false ;
    private void localLog(string msg = "No message") { localLog("LevelScreenLoaderScript", msg); }
    private void localLog(string topic, string msg)
    {
        if (debugOn)
        {
            string logEntry = string.Format("{0:F}:[{1}] {2}", System.DateTime.Now, topic, msg);
            Debug.Log(logEntry);
        }
    }

    private static ArrayList levels = null;

    [SerializeField]
    public DeltaCore.GameMode gameMode;
    [SerializeField]
    public DeltaCore.LevelDifficulty difficulty;


    [SerializeField]
    private Transform levelContainer;

    [SerializeField]
    private GameObject levelPrefab;


	// Use this for initialization
	void Start () {
		if(levels == null) {
            if ( UserInfo.currentGameMode == DeltaCore.GameMode.LatentOfTheDay)
            {
                levels = Database.DownloadLotdLevels(); 
            }
            else
            {
                levels = Database.DownloadTrainingLevels(); 
            }
        }
        else
        {
            levels.Clear();
            if (UserInfo.currentGameMode == DeltaCore.GameMode.LatentOfTheDay)
            {
                levels = Database.DownloadLotdLevels();
            }
            else
            {
                levels = Database.DownloadTrainingLevels();
            }
        }

        //DEBUG
        Load();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeDifficulty(int difficulty) {
        this.difficulty = (DeltaCore.LevelDifficulty) difficulty;
        localLog(string.Format("Changing Difficult to {0}", this.difficulty)); 
        Load();
    }

    public void Load() {
        Reset();
        var levelsToShow = (from l in levels.Cast<FingerprintLevel>().ToList()
                            where l.difficulty == difficulty
                            select l).ToArray();
        for (int i = 0; i < levelsToShow.Length; i++) {
            GameObject level = Instantiate(levelPrefab);
            level.transform.SetParent(this.transform);
            LevelSelectButtonScript levelButton = level.GetComponent<LevelSelectButtonScript>();
            levelButton.SetLevel(levelsToShow[i], levelsToShow[i].sampleId);
            localLog(string.Format("Loading {0}", levelsToShow[i])); 
        }
    }

    public void LoadAll() {
        Reset();
        var levelsToShow = (from l in levels.Cast<FingerprintLevel>().ToList()
                            where true
                            select l).ToArray();
        for (int i = 0; i < levelsToShow.Length; i++) {
            GameObject level = Instantiate(levelPrefab);
            level.transform.SetParent(this.transform);
            LevelSelectButtonScript levelButton = level.GetComponent<LevelSelectButtonScript>();
            levelButton.SetLevel(levelsToShow[i], levelsToShow[i].sampleId);
        }
    }

    public void Reset() {
        foreach (Transform child in transform.GetComponentsInChildren<Transform>()) {
            if (child.gameObject == this.gameObject)
                continue;
            GameObject.Destroy(child.gameObject);
        }
    }
}
