using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDeactivator : MonoBehaviour
{
    [SerializeField]
    GameObject[] toDeactivate;
    [SerializeField]
    float delay;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Show(delay));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Show(float delay)
    {
        foreach(GameObject go in toDeactivate)
        {
            go.SetActive(true);
        }
        yield return new WaitForSeconds(delay);
        foreach (GameObject go in toDeactivate)
        {
            go.SetActive(false);
        }
    }
}
