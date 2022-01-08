using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    //default is 15
    public float distance;
    public bool lowerCam = false;
    
    GameObject cameraRig;
    CinemachineVirtualCamera vCam;
    CinemachineComponentBase componentBase;

    // Start is called before the first frame update
    void Start()
    {
        cameraRig = GameObject.FindGameObjectWithTag("CameraRig");
        vCam = cameraRig.GetComponentInChildren<CinemachineVirtualCamera>();
        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(componentBase == null)
        {
            componentBase = vCam.GetCinemachineComponent(CinemachineCore.Stage.Body);
        }
        if(collision.tag == "Player")
        {
            if(componentBase is CinemachineFramingTransposer)
            {
                (componentBase as CinemachineFramingTransposer).m_CameraDistance = distance;
            }

            if(lowerCam)
            {
                (componentBase as CinemachineFramingTransposer).m_ScreenY = 0.53f;
            }
        }
    }
}
