using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    PlayerManager pManager;
    GravityManager gManager;

    public float maxGravityResourceAmount = 1000f;
    public float currentGravityResourceAmount;
    public float upsideGravityReductionAmount=200f;
    public float weakerGravityReductionAmount=200f;
    public float strongerGravityReductionAmount=550f;
    public float recoverIncreaseAmount=630f;

    [HideInInspector]
    public bool active = true;

    private void Start() {
        pManager = PlayerManager.instance;
        gManager = GravityManager.instance;
        currentGravityResourceAmount = maxGravityResourceAmount;
    }

    private void Update() {
        CheckGravity();    
    }

    private void CheckGravity() {
        Vector2 gravity = GravityManager.instance.physicsGravity;
        float defaultGravityMagnitude = GravityManager.instance.defaultGravityMagnitude;
        float amount = 0f;

        if (gravity.magnitude > defaultGravityMagnitude) {
            amount -= strongerGravityReductionAmount * Time.deltaTime;
        }
        else if (gravity.magnitude < defaultGravityMagnitude) {
            amount -= weakerGravityReductionAmount * Time.deltaTime;
        }

        if (gravity.normalized != Vector2.down) {
            amount -= upsideGravityReductionAmount * Time.deltaTime;
        } 
        else if (gravity.magnitude == defaultGravityMagnitude) {
            amount += recoverIncreaseAmount * Time.deltaTime;
        }

        if(active)
            AddAmount(amount);

        if (currentGravityResourceAmount <= 0f) {
            Reset();
        }
    }

    private void Reset() {
        gManager.ResetGravity();
        pManager.events.gravityChangesChannel.RaiseEventVoid();
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
