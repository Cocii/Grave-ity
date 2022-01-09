using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AudioOptionSettingScriptable : ScriptableObject
{
    [Range(0,1)]
    public float ambientSoundsVolume;
    [Range(0, 1)]
    public float playerSoundsVolume;
    [Range(0, 1)]
    public float effectsSoundsVolume;
}
