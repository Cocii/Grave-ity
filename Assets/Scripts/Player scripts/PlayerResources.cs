using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    public float maxGravityResourceAmount;
    public float currentGravityResourceAmount;
    public float upsideGravityReductionAmount;
    public float weakerGravityReductionAmount;
    public float strongerGravityReductionAmount;
    public float recoverIncreaseAmount;

    public bool active = true;


    private void Start() {
        currentGravityResourceAmount = maxGravityResourceAmount;
    }

    private void Update() {
        CheckGravity();    
    }

    private void CheckGravity() {
        Vector2 gravity = GravityManager.instance.physicsGravity;
        float amount = 0f;

        if (gravity.magnitude > GravityManager.instance.defaultGravityMagnitude) {
            amount -= strongerGravityReductionAmount;
        }
        else if (gravity.magnitude < GravityManager.instance.defaultGravityMagnitude) {
            amount -= weakerGravityReductionAmount;
        }

        if (gravity.normalized != Vector2.down) {
            amount -= upsideGravityReductionAmount;
        } 
        else if (gravity.magnitude == GravityManager.instance.defaultGravityMagnitude) {
            amount += recoverIncreaseAmount;
        }

        if(active)
            AddAmount(amount);

        if (currentGravityResourceAmount <= 0f) {
            GravityManager.instance.ResetGravity();
            return;
        }
    }

    private void Reduce(float amount) {
        currentGravityResourceAmount -= amount;
        currentGravityResourceAmount = Mathf.Clamp(currentGravityResourceAmount, 0f, maxGravityResourceAmount);
    }

    private void AddAmount(float amount) {
        currentGravityResourceAmount += amount;
        currentGravityResourceAmount = Mathf.Clamp(currentGravityResourceAmount, 0f, maxGravityResourceAmount);
    }

}
