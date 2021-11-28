using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Transition
public delegate bool FSMCondition();

//Action
public delegate void FSMAction();

public class FSMTransition 
{
    public FSMCondition myCondition;

    private List<FSMAction> myActions = new List<FSMAction>();

    public FSMTransition(FSMCondition condition, FSMAction[] actions = null)
    {
        myCondition = condition;
        if (actions != null) 
            myActions.AddRange(actions);
    }

    public void Fire()
    {
        if (myActions != null)
            foreach (FSMAction action in myActions)
                action();
    }
}
