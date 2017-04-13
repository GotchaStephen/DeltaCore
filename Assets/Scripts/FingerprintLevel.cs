using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerprintLevel
{


    public string id;
    public int sampleId;
    //public Sprite icon;

    public int classification;
    public DeltaCore.LevelDifficulty difficulty;
    public DeltaCore.FingerPrintQuality quality;
    public DeltaCore.GameMode gameMode;
    public bool isAvailable;
    public bool isLatentOfTheDay;
    // public DateTime availableOn; 

    public Sprite fingerPrint;

    public FingerprintLevel(string id, Sprite fingerPrint, int classification, DeltaCore.LevelDifficulty difficulty, DeltaCore.FingerPrintQuality quality, DeltaCore.GameMode gameMode, bool isAvailable = true, int sampleId = -1, bool isLatentOfTheDay = false)
    {
        this.id = id;
        this.fingerPrint = fingerPrint;
        this.classification = classification;
        this.difficulty = difficulty;
        this.quality = quality;
        this.gameMode = gameMode;
        this.isAvailable = isAvailable;
        this.sampleId = sampleId;
        this.isLatentOfTheDay = isLatentOfTheDay;
    }
    public override string ToString()
    {
        return String.Format("FingerprintLevel [{0}:{1}]", id, sampleId);
    }
}
