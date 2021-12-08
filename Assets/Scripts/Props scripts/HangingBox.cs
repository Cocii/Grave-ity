using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangingBox : MonoBehaviour
{
    public List<GameObject> ropeBonesObjs;
    public HingeJoint2D boxJoint;
    public GameObject box;

    public GravityManager gManager;
    

    private void Start() {
        gManager = GravityManager.instance;
    }

    private void Update() {
        if (boxJoint == null) {
            box.transform.parent = transform.parent;
        }
    }

}
