using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangingBox : MonoBehaviour
{
    public List<GameObject> ropeBonesObjs;
    public GravityManager gManager;
    public float forceMagnitude;

    private void Start() {
        gManager = GravityManager.instance;
    }

    private void Update() {
        //Vector2 gravity = gManager.physicsGravity;
        //ApplyForceToBones(gravity.normalized);
        
    }

    private void ApplyForceToBones(Vector2 direction) {
        foreach (GameObject bone in ropeBonesObjs) {
            Rigidbody2D body = bone.GetComponent<Rigidbody2D>();
            body.AddForce(direction * forceMagnitude);
        }
    }
}
