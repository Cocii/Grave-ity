using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraPlayerTarget : MonoBehaviour
{
    CinemachineVirtualCamera vcam;
    GameObject player;

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        player = GameObject.FindGameObjectWithTag("Player");
        vcam.Follow = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
