using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class TriggerChecker : MonoBehaviour
{

    public GameObject[] checkpoints;
    private int tempCheckPoint;

    private GameObject currentCheckpoint;

    // Start is called before the first frame update
    void Start()
    {
        checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Checkpoint")
        {
            int index = 0;
            int previousIndex = PlayerPrefs.GetInt("currentCheckPoint", 0);
            currentCheckpoint = checkpoints[previousIndex];          

            for (int i = 0; i < checkpoints.Length; i++)
            {
                if (GameObject.ReferenceEquals(checkpoints[i], collision.gameObject))
                {
                    index = i;
                    break;
                }
            }
           
            if (index > previousIndex)
            {
                //I'm saving every checkpoint as an index in playerprefs, they are ordered in scene
                tempCheckPoint = previousIndex + 1;
                PlayerPrefs.SetInt("currentCheckPoint", tempCheckPoint);
            }

            
        }

        if (collision.tag == "Death Trigger")
        {
            //Fade Screen
            //...
            GameObject fadeScreen = GameObject.FindGameObjectWithTag("FadeScreen");
            if(fadeScreen != null)
            {
                Image _img = fadeScreen.GetComponent<Image>();
                StartCoroutine(FadeIn(_img, 10f));

            }

            //fadeScreen.GetComponent<Image>().enabled = true;

            StartCoroutine(DelayReset());
            StartCoroutine(LoadScene("GameScene"));

        }
    }

    IEnumerator FadeImage(Image img, bool fadeAway)
    {
        // fade from opaque to transparent
        if (fadeAway)
        {
            // loop over 1 second backwards
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                img.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
        // fade from transparent to opaque
        else
        {
            // loop over 1 second
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                // set color with i as alpha
                img.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
    }

    IEnumerator FadeIn(Image image, float FadeRate)
    {
        float targetAlpha = 1.0f;
        Color curColor = image.color;
        while (Mathf.Abs(curColor.a - targetAlpha) > 0.0001f)
        {
            Debug.Log(image.material.color.a);
            curColor.a = Mathf.Lerp(curColor.a, targetAlpha, FadeRate * Time.deltaTime);
            image.color = curColor;
            yield return null;
        }

        curColor.a = targetAlpha; 
        image.color = curColor;

    }
    IEnumerator LoadScene(string scene)
    {
        
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(scene);
        
    }

    IEnumerator DelayReset()
    {
        yield return new WaitForSeconds(1f);
        GravityManager.instance.ResetGravity();
        transform.position = checkpoints[PlayerPrefs.GetInt("currentCheckPoint", 0)].transform.position;
    }
}
