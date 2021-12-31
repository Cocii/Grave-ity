using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomTime
{
    public static float maxDT = 1f / 30f;

    public static float TargetFrameRate {
        get { return 1f / maxDT; }
        set { maxDT = 1f / value; }
    }
    public static float deltaTime {
        get {
            if (Time.deltaTime > maxDT)
                return maxDT;
            return Time.deltaTime;
        }
    }
}
