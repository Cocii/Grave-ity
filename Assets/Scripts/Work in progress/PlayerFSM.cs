using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM : MonoBehaviour
{
    public enum currentStateEnum {
        idle, motion, land, midair, wallback, rotating, grab, dash
    }

    public currentStateEnum currentStateVisualizer = currentStateEnum.idle;

    private FSM fsm;
    private FSMState idleState;
    private FSMState motionState;
    private FSMState landState;
    private FSMState midairState;
    private FSMState wallbackState;
    private FSMState rotatingState;
    private FSMState grabState;
    private FSMState dashState;

    void Start() {
        idleState = new FSMState();
        motionState = new FSMState();
        landState = new FSMState();
        midairState = new FSMState();
        wallbackState = new FSMState();
        rotatingState = new FSMState();
        grabState = new FSMState();
        dashState = new FSMState();






        fsm = new FSM(idleState);
    }


void Update() {
        
    }
}
