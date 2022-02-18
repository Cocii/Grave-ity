using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public LevelLoader loader;
    [Header("Carica il livello successivo a quello selezionato")]
    public LevelsEnum level;

    private void OnTriggerEnter2D(Collider2D collision) {
        switch (level) {
            case LevelsEnum.Forest:
                loader.LoadSecondLevel();
                break;
            case LevelsEnum.Lab:
                loader.LoadThirdLevel();
                break;
            case LevelsEnum.City:
                loader.LoadFinalScene();
                break;
        }
    }
}
