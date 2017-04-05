using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameModeButtonScript : MonoBehaviour, IPointerClickHandler {

    private enum GameMode { Tutorial, Training, Caseworks, ColdCases, LatentOfTheDay };

    [SerializeField]
    private GameMode gameMode;
    public static bool debugOn = false;
    private static void localLog(string msg = "No message") { localLog("GameModeButtonScript", msg); }
    private static void localLog(string topic, string msg)
    {
        if (debugOn)
        {
            string logEntry = string.Format("{0:F}: [{1}]{2}", System.DateTime.Now, topic, msg);
            Debug.Log(logEntry);
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        localLog("Loading Next Screen"); 
        LoadNextScreen();
    }

    private void LoadNextScreen() {
        switch (gameMode) {
            case GameMode.Tutorial:
                break;
            case GameMode.Training:
                break;
            case GameMode.Caseworks:
                break;
            case GameMode.ColdCases:
                break;
            case GameMode.LatentOfTheDay:
                break;
        }
    }
}
