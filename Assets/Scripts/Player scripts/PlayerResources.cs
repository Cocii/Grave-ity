using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    public float maxGravityResourceAmount = 1000f;
    public float currentGravityResourceAmount;
    public float upsideGravityReductionAmount=200f;
    public float weakerGravityReductionAmount=200f;
    public float strongerGravityReductionAmount=550f;
    public float recoverIncreaseAmount=630f;

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
            amount -= strongerGravityReductionAmount * Time.deltaTime;
        }
        else if (gravity.magnitude < GravityManager.instance.defaultGravityMagnitude) {
            amount -= weakerGravityReductionAmount * Time.deltaTime;
        }

        if (gravity.normalized != Vector2.down) {
            amount -= upsideGravityReductionAmount * Time.deltaTime;
        } 
        else if (gravity.magnitude == GravityManager.instance.defaultGravityMagnitude) {
            amount += recoverIncreaseAmount * Time.deltaTime;
        }

        if(active)
            AddAmount(amount);

        if (currentGravityResourceAmount <= 0f) {
            GravityManager.instance.ResetGravity();
            return;
        }
    }

    private void FixedUpdate() {
        
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
