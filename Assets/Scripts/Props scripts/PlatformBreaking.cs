using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBreaking : MonoBehaviour
{
    Explodable explodable;
    PiecesDestroyer destroyer;
    public AudioSource break_audio;

    // Start is called before the first frame update
    void Start()
    {
        explodable = GetComponent<Explodable>();
        destroyer = FindObjectOfType<PiecesDestroyer>();
        break_audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("BreakingBox"))
        {
            break_audio.Play();
            explodable.explode();
            destroyer.DestroyPieces();
        }       
    }
}
