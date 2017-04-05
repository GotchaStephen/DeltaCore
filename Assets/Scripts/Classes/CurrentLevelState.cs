using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurrentLevelState {


    public static FingerprintLevel currentLevel;

    public static ArrayList markers;

    public static string actionsLog;


    public static void LogAction(string s) {
        actionsLog += s;
    }
}
