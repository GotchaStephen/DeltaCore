/* Copyright (C) 2017 Francesco Sapio - All Rights Reserved
 * The license use of this code and/or any portion is restricted to the 
 * the project DELTA CORE, subject to the condition that this
 * copyright notice shall remain.
 *  
 * Modification, distribution, reproduction or sale of this 
 * code for purposes outside of the license agreement is 
 * strictly forbidden.
 * 
 * NOTICE:  All information, intellectual and technical
 * concepts contained herein are, and remain, the property of Francesco Sapio.
 *
 * Attribution is required.
 * Please write to: contact@francescosapio.com
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.SceneManagement;

public class LevelSelectButtonScript : MonoBehaviour, IPointerClickHandler {

    [SerializeField]
    const string className = "LevelSelectButtonScript";
    public bool localOn = true;
    public bool globalOn = true;
    private void localLog(string msg, string topic = "L:" + className) { if (localOn) { logMsg(msg, topic); } }
    private void globalLog(string msg, string topic = "G:" + className) { if (globalOn) { logMsg(msg, "G:" + className); } }
    private void logMsg(string msg, string topic)
    {
        string logEntry = string.Format("{0:F}: [{1}] {2}", System.DateTime.Now, topic, msg);
        Debug.Log(logEntry);
    }

    [SerializeField]
    Sprite spriteDifficultyEasy, spriteDifficultyMedium, spriteDifficultyHard, spriteDifficultyExpert;

    Text label;
    Image icon;
    Image imageFrame;

    int levelNumber;

    FingerprintLevel level;
    

    // Use this for initialization
    void Start () {
        label = GetComponentInChildren<Text>();
        icon = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        imageFrame = GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void SetLevel(FingerprintLevel level, int levelNumber) {
        Start();
        this.level = level;
        this.levelNumber = levelNumber;
        // localLog("Setting" + levelNumber.ToString() + "-" + level.sampleId);

        switch (level.difficulty) {
            case DeltaCore.LevelDifficulty.Easy:
                imageFrame.sprite = spriteDifficultyEasy;
                break;
            case DeltaCore.LevelDifficulty.Medium:
                imageFrame.sprite = spriteDifficultyMedium;
                break;
            case DeltaCore.LevelDifficulty.Hard:
                imageFrame.sprite = spriteDifficultyHard;
                break;
            case DeltaCore.LevelDifficulty.Expert:
                imageFrame.sprite = spriteDifficultyExpert;
                break;
        }

        icon.sprite = level.fingerPrint;
        // localLog("S#" + levelNumber.ToString("000") + "-" + level.sampleId);
        label.text = ""; // "S#" + levelNumber.ToString("000");
    }

    public FingerprintLevel GetLevel() {
        return level;
    }

    public void OnPointerClick(PointerEventData eventData) {

        LevelData levelToLoad = Database.LoadLevelData(level);
        localLog(String.Format("Level Selected {0}", levelToLoad));
        localLog(String.Format("Game Mode {0}", UserInfo.currentGameMode));

        if ( UserInfo.currentGameMode == DeltaCore.GameMode.LatentOfTheDay)
        {
            AnalyseScreenScript.currentLevel = levelToLoad ;
            SceneManager.LoadScene ("52_LotdAnalyseScreen");
		} else if  ( UserInfo.currentGameMode == DeltaCore.GameMode.Training)
        {
            TrainingAnalyseScreenScript.currentLevel = levelToLoad ;
            SceneManager.LoadScene ("22_TrainingAnalyseScreen");
		}
    }
}


