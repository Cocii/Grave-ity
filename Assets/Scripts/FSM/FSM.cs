using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM
{
    public FSMState current;

    public FSM(FSMState state)
    {
        current = state;
        current.Enter();
    }

    public void Update()
    {
        FSMTransition transition = current.VerifyTransitions();

        if (transition != null)
        {
            current.Exit();

            transition.Fire();
            current = current.NextState(transition);

            current.Enter();
        } 
        else
        {
            current.Stay();
        }
    }
}
