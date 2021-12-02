using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deactivator : MonoBehaviour
{

    Rigidbody2D rbody;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    private void OnBecameVisible()
    {
        rbody.simulated = true;
    }

    private void OnBecameInvisible()
    {
        rbody.simulated = false;
    }
}
