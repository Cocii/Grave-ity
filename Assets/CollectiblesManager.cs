using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CollectiblesManager : MonoBehaviour
{
    public List<GameObject> collectibles;
    public List<GameObject> locked;

    // Start is called before the first frame update
    void Start()
    {
        //Cycle until PlayerPrefs collectibles index: from 1 to 8, 0 means no collectibles.
        // - Deactivate temp image and activate proper image
        PlayerPrefs.SetInt("Collectibles", 8);
        int index = PlayerPrefs.GetInt("Collectibles");
        if(index != 0)
        {
            for (int i = 0; i < index; i++)
            {
                locked[i].active = false;
                collectibles[i].active = true;
            }
        }

        if(index != 8)
        {
            for(int i = 7; i >= index; i--)
            {
                locked[i].active = true;
                collectibles[i].active = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
