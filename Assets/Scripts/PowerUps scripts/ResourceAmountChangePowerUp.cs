using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/ResourceAmountChangePowerUp")]
public class ResourceAmountChangePowerUp : PowerUpScriptable
{
    public float weakGravityAmountMult;
    public float strongGravityAmountMult;
    public float upsideGravityAmountMult;
    public float recoverGravityAmountMult;

    public override void ActivationFunction() {
        PlayerManager pManager = PlayerManager.instance;

        switch (powerId) {
            case PowerUpIdEnum.ConsumeLessResource:
                pManager.resources.weakerGravityReductionAmount *= weakGravityAmountMult;
                pManager.resources.strongerGravityReductionAmount *= strongGravityAmountMult;
                pManager.resources.upsideGravityReductionAmount *= upsideGravityAmountMult;
                break;

            case PowerUpIdEnum.RecoverResourceFaster:
                pManager.resources.recoverIncreaseAmount *= recoverGravityAmountMult;
                break;

            default:
                break;
        } 
    }

    public override void DeactivationFunction() {
        PlayerManager pManager = PlayerManager.instance;

        switch (powerId) {
            case PowerUpIdEnum.ConsumeLessResource:
                pManager.resources.weakerGravityReductionAmount *= 1 / weakGravityAmountMult;
                pManager.resources.strongerGravityReductionAmount *= 1 / strongGravityAmountMult;
                pManager.resources.upsideGravityReductionAmount *= 1 / upsideGravityAmountMult;
                break;

            case PowerUpIdEnum.RecoverResourceFaster:
                pManager.resources.recoverIncreaseAmount *= 1 / recoverGravityAmountMult;
                break;

            default:
                break;
        }
    }
}
