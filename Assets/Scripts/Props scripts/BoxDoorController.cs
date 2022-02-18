using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDoorController : MonoBehaviour
{
    public Transform doorTransform;
    public Collider2D platform;
    public LayerMask layerMask;

    public Vector2 posLock;
    public Vector2 posUnlock;

    public float defaultHeight;
    public float raiseHeight = 5f;
    public float raiseSpeed = 30f;
    Vector2 newPos;
    //public float forceMagnitude;

    public bool propDetected = false;

    private void Start() {
        defaultHeight = doorTransform.localPosition.y;
    }

    private void FixedUpdate() {
        if (!propDetected) {
            if(doorTransform.localPosition.y == defaultHeight)
                return;
            print("going down");
            newPos.Set(doorTransform.localPosition.x, Mathf.MoveTowards(doorTransform.localPosition.y, defaultHeight, raiseSpeed * Time.smoothDeltaTime));
        }
        else {
            if (doorTransform.position.y == defaultHeight + raiseHeight)
                return;
            print("going up");
            newPos.Set(doorTransform.localPosition.x, Mathf.MoveTowards(doorTransform.localPosition.y, defaultHeight + raiseHeight, raiseSpeed * Time.smoothDeltaTime));            
        }

        doorTransform.localPosition = newPos;

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!Utilities.LayerInLayermask(collision.gameObject.layer, layerMask)) {
            return;
        }

        //platform.offset = posUnlock;
        propDetected = true;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (!Utilities.LayerInLayermask(collision.gameObject.layer, layerMask)) {
            return;
        }

        //platform.offset = posLock;
        propDetected = false;
    }

    //private void OnTriggerStay2D(Collider2D collision) {
    //    if (!Utilities.LayerInLayermask(collision.gameObject.layer, layerMask)) {
    //        return;
    //    }
    //    //print("Stay");
        
    //}
}
