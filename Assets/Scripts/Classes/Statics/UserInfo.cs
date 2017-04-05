using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UserInfo
{

    public enum UserType { Guest, NormalUser, Subscriber, Expert, Analyser };
    public enum UserAction { Login, EnterLevel, ExitLevel, ResettoLastSaved, ResetAll, Save, LevelAction, Submit };
    // public enum GameMode { NotSet, Tutorial, Training, Caseworks, ColdCases, LOTD };

    public static int id;
    public static string username;
    public static string firstName;
    public static string lastName;
    public static string errorString;
    public static UserType userType;
    public static UserAction lastAction;
    public static DeltaCore.GameMode currentGameMode;
    public static bool completedLOTD ;
    public static string lotdName ;

    public static void Reset()
    {
        userType = UserType.Guest;
        username = null;
        errorString = null;
        lastAction = UserAction.Login;
        currentGameMode = DeltaCore.GameMode.NoGameMode;
    }
}
