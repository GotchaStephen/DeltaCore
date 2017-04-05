using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DeltaCore
{

    public enum GameMode { Tutorial, Training, Caseworks, ColdCases, LatentOfTheDay, NoGameMode };

    public enum LevelDifficulty { Easy, Medium, Hard, Expert, NA };

    public enum FingerPrintType { TrainingPrints, LatentOfTheDay }

    public enum FingerPrintQuality { VeryHigh, MediumHigh, MediumLow, Low };

    public enum MarkerConfidence { Low, Medium, High };

    public enum MarkerType { EdgeRidge, Bifurcation, ShortRidge };

    public enum AnalysisDecision { ValueForIdentification, ValueForExclusion, NoValue, NotSet };

    public enum LevelProgress { NotAttempted, Threshold1, Threshold2, Threshold3, Completed, Perfected }

    public enum UserLevelAction { NoAction, AddMarker, MoveMarker, RemoveMarker };
}
