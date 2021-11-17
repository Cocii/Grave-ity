using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static float Map(float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public static Vector2 RoundV2(Vector2 v) {
        v.Set(Mathf.Round(v.x * 10f) / 10f, Mathf.Round(v.y * 10f) / 10f);
        return v;
    }

    public static Vector3 RoundV3(Vector3 v) {
        v.Set(Mathf.Round(v.x * 10f) / 10f, Mathf.Round(v.y * 10f) / 10f, Mathf.Round(v.z * 10f) / 10f);
        return v;
    }

    public static Vector3 RoundToIntV3(Vector3 v) {
        v.Set(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));
        return v;
    }
}
