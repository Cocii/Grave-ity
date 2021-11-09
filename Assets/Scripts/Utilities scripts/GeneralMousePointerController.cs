using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//faccio scomparire il mouse mentre si gioca
public class GeneralMousePointerController : MonoBehaviour
{
    public bool visible;
    public bool alwaysNotVisible;


    void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        visible = false;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            toggleState();

            if (alwaysNotVisible && visible)
                toggleState();
        }
    }

    private void toggleState() {
        visible = !visible;

        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = visible;
    }
}
