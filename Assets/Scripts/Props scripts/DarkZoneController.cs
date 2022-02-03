using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

public class DarkZoneController : MonoBehaviour
{
    
    public SpriteShapeRenderer spriteRenderer;
    public float subtract = 0.01f;
    public float targetAlpha = 0;

    private void OnTriggerEnter2D(Collider2D collision) {
        print("triggered");

        if (!collision.CompareTag("Player")) {
            return;
        }

        if (spriteRenderer)
            StartCoroutine(LightUpCo());
        //    //Color color = r.color;
        //    //color.a = targetAlpha;
        //    //r.color = color;
        //}
    }

    IEnumerator LightUpCo() {
        //print("Co started");
        
        print(spriteRenderer.color);
        while(spriteRenderer.color.a != targetAlpha) {
            Color color = spriteRenderer.color;
            color.a = Mathf.MoveTowards(color.a, targetAlpha, subtract);
            spriteRenderer.color = color;
        }

        yield break;
    }
}
