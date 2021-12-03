using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vent : MonoBehaviour
{
    GravityManager gManager;
    public float raiseHeight;
    public float defaultForceMagnitude;
    public float forceMagnitude;
    public LayerMask layerToAffect;
    public Rigidbody2D targetBody;
    public BoxCollider2D underPropCollider;

    void Start()
    {
        gManager = GravityManager.instance;
    }

    private void FixedUpdate() {
        if (targetBody == null)
            return;

        Vector2 direction = transform.up;
        Vector2 force = direction * forceMagnitude;
        targetBody.AddForce(force);
        
    }

    void Update() {
        forceMagnitude = (2 - gManager.gravityRatio) * defaultForceMagnitude;

        Vector2 direction = transform.up;

        Debug.DrawRay(transform.position, direction * raiseHeight, Color.cyan);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, raiseHeight, layerToAffect);
        if (hit) {
            targetBody = hit.transform.gameObject.GetComponent<Rigidbody2D>();
        }
        else {
            if (targetBody)
                targetBody = null;

            hit = Physics2D.Raycast(transform.position, direction, raiseHeight*1.2f, layerToAffect);
            if (hit && gManager.gravityRatio < 1f) {
                MovePropCollider(hit.distance);
            }
            else {
                MovePropCollider(0f);
            }
        }
    }

    private void MovePropCollider(float distance) {
        if (distance == 0) {
            ResetPropCollider();
            return;
        }

        float sizeOffset = transform.lossyScale.y * underPropCollider.size.y;
        
        underPropCollider.offset = new Vector2(0f, (distance - sizeOffset)*1/ transform.lossyScale.y);

        underPropCollider.size = new Vector2(0.5f, 1f);
    }

    private void ResetPropCollider() {
        underPropCollider.size = new Vector2(1f, 1f);
        underPropCollider.offset = Vector2.zero;
    }
}
