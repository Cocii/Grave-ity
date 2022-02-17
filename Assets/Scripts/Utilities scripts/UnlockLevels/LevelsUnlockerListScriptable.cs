using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelsEnum {
    Forest,
    Lab,
    City
}

[CreateAssetMenu(menuName = "Scriptables/LevelsUnlockerScriptable")]
public class LevelsUnlockerListScriptable : DescriptionBaseSO
{
    public List<bool> boolList;

}
