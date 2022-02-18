using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDeactivationTriggerEnter : MonoBehaviour
{
    public Collider2D colliderToDeactivate;
    public LayerMask layerMask;

    private void OnTriggerEnter2D(Collider2D other) {
        print("trigger enter");


        if (!Utilities.LayerInLayermask(other.gameObject.layer, layerMask)) {
            return;
        }

        Action();
    }

    private void Action() {
        print("porcoddio");
        colliderToDeactivate.enabled = false;
    }
}
