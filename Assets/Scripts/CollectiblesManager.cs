using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CollectiblesManager : MonoBehaviour
{
    public List<GameObject> collectibles;
    public List<GameObject> locked;

    private List<int> collectables;

    // Start is called before the first frame update
    void Start()
    {
        collectables = new List<int>();

        /*
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
        */

        RetrieveCollected();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void RetrieveCollected()
    {
        int graduation = PlayerPrefs.GetInt("1");
        collectables.Add(graduation);
        int confirmation = PlayerPrefs.GetInt("2");
        collectables.Add(confirmation);
        int discovery = PlayerPrefs.GetInt("3");
        collectables.Add(discovery);
        int theGamble = PlayerPrefs.GetInt("4");
        collectables.Add(theGamble);
        int theBeginningOfTheEnd = PlayerPrefs.GetInt("5");
        collectables.Add(theBeginningOfTheEnd);
        int reachingForHope = PlayerPrefs.GetInt("6");
        collectables.Add(reachingForHope);
        int allThatRemains = PlayerPrefs.GetInt("7");
        collectables.Add(allThatRemains);
        int repeat = PlayerPrefs.GetInt("8");
        collectables.Add(repeat);

        for (int i = 0; i < collectables.Count; i++)
        {
            if(collectables[i] == 0)
            {
                //Locked
                locked[i].active = true;
                collectibles[i].active = false;
            }
            else
            {
                //Unlocked
                locked[i].active = false;
                collectibles[i].active = true;
            }
        }

    }
}
