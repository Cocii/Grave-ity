using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moonjump : MonoBehaviour
{
    public Vector2 dir;
    public Vector2 force;
    [Range(0,10)]
    public float magnitudeMult;
    public float delayActivation = 0.6f;
    public float delayDeactivation = 0.3f;

    public bool apply = false;
    public bool coRunning = false;
    public Coroutine changeCo;

    private void Update() {
        PlayerManager manager = PlayerManager.instance;
        float gravityRatio = manager.currentGravityRatio;
        Vector2 move = new Vector2(manager.input.GetMoveInput.x, 0f);

        if (gravityRatio >= 1 || move.magnitude == 0) {
            apply = false;

            if (coRunning) {
                StopCoroutine(changeCo);
                coRunning = false;
            }
                
            return;
        }
            
        Vector2 gravity = manager.currentGravity;

        dir = (-gravity.normalized + move).normalized;
        force = dir * gravity.magnitude * magnitudeMult;
        //Debug.DrawRay(transform.position, force, Color.yellow);

        if(manager.isGrounded && !coRunning && !apply) {
            //print("Changing to true");
            changeCo = StartCoroutine(changeApplyCo(true, delayActivation));
        }
        else if (!manager.isGrounded && !coRunning && apply) {
            //print("Changing to false");
            changeCo = StartCoroutine(changeApplyCo(false, delayDeactivation));
        }

        if (apply) {
            //manager.body.AddForce(force);
            manager.characterController.AddBoostForce(force);
        }
        //else if (changeCo == null) {
        //    changeCo = StartCoroutine(changeApplyCo(true, delayActivation));
        //}
    }

    IEnumerator changeApplyCo(bool state, float delay) {
        coRunning = true;
        yield return new WaitForSeconds(delay);
        apply = state;
        coRunning = false;
        yield break;
    }

}
