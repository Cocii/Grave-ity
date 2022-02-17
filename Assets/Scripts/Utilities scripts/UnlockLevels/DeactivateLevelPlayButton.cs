using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateLevelPlayButton : MonoBehaviour
{
    //public StringListScriptable levels;
    //public string levelName;

    public LevelsUnlockerListScriptable levelList;
    public LevelsEnum level;

    private void Start() {
        //if (!levels.stringList.Contains(levelName)){
        //    gameObject.SetActive(false);
        //}

        if (!levelList.boolList[(int)level]) {
            gameObject.SetActive(false);
        }
    }
}
