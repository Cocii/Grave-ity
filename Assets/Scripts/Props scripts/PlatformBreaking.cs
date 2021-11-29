using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBreaking : MonoBehaviour
{
    Explodable explodable;
    PiecesDestroyer destroyer;

    // Start is called before the first frame update
    void Start()
    {
        explodable = GetComponent<Explodable>();
        destroyer = FindObjectOfType<PiecesDestroyer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("BreakingBox"))
        {
            explodable.explode();
            destroyer.DestroyPieces();
        }       
    }
}
