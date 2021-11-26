using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainGravity : MonoBehaviour
{
    public ParticleSystem rain;
    PlayerManager playerManager;
    GravityManager gravityManager;
    public float strongerGravityModifier;
    public float weakerGravityModifier;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = PlayerManager.instance;
        gravityManager = GravityManager.instance;
        rain.gravityModifier = playerManager.currentGravityRatio;

    }

    // Update is called once per frame
    void Update()
    {
        if(gravityManager.gravityDirection == Vector2.up)
        {
            rain.gravityModifier = -1f;
        }
        else
        {
            if(playerManager.currentGravityRatio < 1)
            {
                rain.gravityModifier = weakerGravityModifier;
            } 
            else if (playerManager.currentGravityRatio > 1)
            {
                rain.gravityModifier = strongerGravityModifier;
            }
            else if (playerManager.currentGravityRatio == 1)
            {
                rain.gravityModifier = 1f;
            }
        }
    }
}
