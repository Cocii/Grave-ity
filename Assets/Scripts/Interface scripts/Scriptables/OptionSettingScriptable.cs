using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class OptionSettingScriptable : ScriptableObject
{
    [Range(0,1)]
    public float ambientSoundsVolume;
    [Range(0, 1)]
    public float playerSoundsVolume;
    [Range(0, 1)]
    public float gravityChangeSoundsVolume;
}
