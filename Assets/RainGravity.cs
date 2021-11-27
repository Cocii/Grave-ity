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
    
    [Header ("Rain Audio Gravity Settings")]
    private AudioSource rainAudio;
    public float reverseGravityPitch;
    public float normalGravityPitch;
    public float weakerGravityPitch;
    public float strongerGravityPitch;

    // Start is called before the first frame update
    void Start()
    {
        rainAudio = GetComponent<AudioSource>();
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
            rainAudio.pitch = Mathf.Lerp(rainAudio.pitch, reverseGravityPitch, 1f * Time.deltaTime);
        }
        else
        {
            if(playerManager.currentGravityRatio < 1)
            {
                rain.gravityModifier = weakerGravityModifier;
                rainAudio.pitch = Mathf.Lerp(rainAudio.pitch, weakerGravityPitch, 1f * Time.deltaTime);

            } 
            else if (playerManager.currentGravityRatio > 1)
            {
                rain.gravityModifier = strongerGravityModifier;
                rainAudio.pitch = Mathf.Lerp(rainAudio.pitch, strongerGravityPitch, 1f * Time.deltaTime);
            }
            else if (playerManager.currentGravityRatio == 1)
            {
                rain.gravityModifier = 1f;
                rainAudio.pitch = Mathf.Lerp(rainAudio.pitch, normalGravityPitch, 1f * Time.deltaTime);
            }
        }
    }
}
