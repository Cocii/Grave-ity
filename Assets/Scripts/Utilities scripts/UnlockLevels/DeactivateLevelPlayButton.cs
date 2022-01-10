using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateLevelPlayButton : MonoBehaviour
{
    public StringListScriptable levels;
    public string levelName;

    private void Start() {
        if (!levels.stringList.Contains(levelName)){
            gameObject.SetActive(false);
        }
    }
}
