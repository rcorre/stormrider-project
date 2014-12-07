using UnityEngine;

public static class Util {
    public static void Assert(bool cond, string message = "Assertion Failure") {
        if (!cond) { Debug.LogError(message); }
    }
}
