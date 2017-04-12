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

public class LotdLevelSelectButtonScript : MonoBehaviour, IPointerClickHandler {

    [SerializeField]
    Sprite spriteLOTD;

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
		imageFrame.sprite = spriteLOTD;
        
        icon.sprite = level.fingerPrint;
        label.text = "S#" + levelNumber.ToString("000");
    }

    public FingerprintLevel GetLevel() {
        return level;
    }

    public void OnPointerClick(PointerEventData eventData) {
        AnalyseScreenScript.currentLevel = Database.LoadLevelData(level);
        SceneManager.LoadScene("52_LotdAnalyseScreen");
    }
}
