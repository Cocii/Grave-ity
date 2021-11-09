using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moonjump : MonoBehaviour
{
    public Vector2 dir;
    public Vector2 force;
    public float magnitude;
    public float delay = 0.33f;

    public bool apply = true;
    Coroutine changeCo;

    private void Update() {
        PlayerManager manager = PlayerManager.instance;
        Vector2 gravity = manager.currentGravity;
        Vector2 move = new Vector2(manager.input.GetMoveInput.x, 0f).normalized;

        dir = (-gravity.normalized + move) * move.magnitude;
        force = dir * magnitude;
        Debug.DrawRay(transform.position, force, Color.yellow);

        if (apply) {
            manager.body.AddForce(force);
        }

        if(!manager.isGrounded && manager.wasGrounded && changeCo==null) {
            changeCo = StartCoroutine(changeApplyCo(false));
        }
        if(manager.isGrounded && !manager.wasGrounded && changeCo == null) {
            changeCo = StartCoroutine(changeApplyCo(true));
        }
        
    }

    IEnumerator changeApplyCo(bool state) {
        print("Changing to " + state);
        yield return new WaitForSeconds(delay);
        apply = state;
        changeCo = null;
        yield return null;
        
    }

}
