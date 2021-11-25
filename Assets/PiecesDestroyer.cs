using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecesDestroyer : MonoBehaviour
{
    public float destroyTime;
    public GameObject[] pieces;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(destroyTime);

        foreach (GameObject piece in pieces)
        {
            Destroy(piece);

        }
    }

    public void DestroyPieces()
    {
        pieces = GameObject.FindGameObjectsWithTag("BreakablePiece");

        
        foreach(GameObject piece in pieces)
        {
            StartCoroutine(FadeOutMaterial(piece, destroyTime));
           
        }
        
        StartCoroutine(WaitAndDestroy());
    }

    IEnumerator FadeOutMaterial(GameObject objectToFade, float fadeSpeed)
    {
        Renderer rend = objectToFade.transform.GetComponent<Renderer>();
        Color matColor = rend.material.color;
        float alphaValue = rend.material.color.a;

        while (rend.material.color.a > 0f)
        {
            alphaValue -= Time.deltaTime / fadeSpeed;
            rend.material.color = new Color(matColor.r, matColor.g, matColor.b, alphaValue);
            yield return null;
        }
        rend.material.color = new Color(matColor.r, matColor.g, matColor.b, 0f);
    }
}
