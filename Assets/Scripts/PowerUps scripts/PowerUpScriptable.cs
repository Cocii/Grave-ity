using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpIdEnum {
    Empty,
    ConsumeLessResource,
    RecoverResourceFaster,
}

[CreateAssetMenu(menuName = "Scriptables/PowerUpScriptable")]
public class PowerUpScriptable : ScriptableObject {
    public PowerUpIdEnum powerId;
    public string powerName;
    public Sprite powerIcon;
    public string powerDescription;


    public virtual void ActivationFunction() {
        //Debug.Log(powerId + " activated");
    }

    public virtual void DeactivationFunction() {
        //Debug.Log(powerId + " deactivated");
    }
}
