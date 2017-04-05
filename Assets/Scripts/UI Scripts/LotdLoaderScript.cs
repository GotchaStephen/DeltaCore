using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LotdLoaderScript : MonoBehaviour {

    private static ArrayList levels = null;

    [SerializeField]
    private static bool debugOn = true;

    private static void localLog(string msg = "No message") { localLog("LotdLoaderScript", msg); }
    private static void localLog(string topic, string msg)
    {
        if (debugOn)
        {
            string logEntry = string.Format("{0:F}: [{1}]{2}", System.DateTime.Now, topic, msg);
            Debug.Log(logEntry);
        }
    }

    [SerializeField]
    private Transform levelContainer;

    [SerializeField]
    private GameObject levelPrefab;

    public Text completedText; 

    private bool LotdLoaded ;
    private bool LotdCompleted ;

    // Use this for initialization
    void Start () {
        LotdLoaded = false; 
        //DEBUG
        //Load();
	}
	
	// Update is called once per frame
	void Update () {
        if (!LotdLoaded)
        {
            if (Database.latentOfTheDayID != null)
            {
                localLog(string.Format("Sample Status {0}", UserInfo.completedLOTD));
                if (UserInfo.completedLOTD) {
                    localLog(string.Format("User already completed LOTD"));
                    completedText.gameObject.SetActive(true) ;
                }
                else { Load(); }
                LotdLoaded = true;
            }
        }
	}

    public void Load() {
        Reset();
        localLog("Loading Latent Of the Day"); 
        GameObject level = Instantiate(levelPrefab);
        level.transform.SetParent(this.transform);
        LevelSelectButtonScript levelButton = level.GetComponent<LevelSelectButtonScript>();
        localLog(string.Format("Adding {0}", Database.latentOfTheDayID.sampleId)); 
        levelButton.SetLevel(Database.latentOfTheDayID, Database.latentOfTheDayID.sampleId);
    }

   public void Reset() {
        foreach (Transform child in transform.GetComponentsInChildren<Transform>()) {
            if (child.gameObject == this.gameObject)
                continue;
            GameObject.Destroy(child.gameObject);
        }
    }
}
