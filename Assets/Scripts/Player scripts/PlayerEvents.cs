using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEvents : MonoBehaviour
{
    PlayerManager pManager;

    public UnityEvent powerSwitched;

    private void Start() {
        pManager = PlayerManager.instance;
    }
}
