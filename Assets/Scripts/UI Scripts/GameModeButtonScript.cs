using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameModeButtonScript : MonoBehaviour, IPointerClickHandler {

    private enum GameMode { Tutorial, Training, Caseworks, ColdCases, LatentOfTheDay };

    [SerializeField]
    private GameMode gameMode;

    public void OnPointerClick(PointerEventData eventData) {
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
