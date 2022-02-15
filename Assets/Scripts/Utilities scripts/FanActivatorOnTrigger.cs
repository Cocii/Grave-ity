using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanActivatorOnTrigger : MonoBehaviour
{
    public LayerMask layerMask;
    public Fan fan;
    public MultiFanController multiFan;

    private void Start() {
        ChangeState(false);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (Utilities.LayerInLayermask(collision.gameObject.layer, layerMask)) {
            ChangeState(true);
            print("player in");
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (Utilities.LayerInLayermask(collision.gameObject.layer, layerMask)) {
            ChangeState(false);
            print("player out");
        }
    }

    private void ChangeState(bool state) {
        if (fan)
            fan.enabled = state;

        if (multiFan)
            multiFan.enabled = state;
    }
}
