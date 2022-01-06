using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UI_GravityDirectionUpdater : MonoBehaviour
{
    [SerializeField]
    Image weakerGravityImage , normalGravityImage, strongerGravityImage;

    GravityManager gManager;

    // Start is called before the first frame update
    void Start()
    {
        gManager = GravityManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        //Normal Gravity
        if(gManager.gravityRatio == 1)
        {
            Color tempColor = normalGravityImage.color;
            tempColor.a = 1f;
            normalGravityImage.color = tempColor;

            tempColor = weakerGravityImage.color;
            tempColor.a = 0.2f;
            weakerGravityImage.color = tempColor;

            tempColor = strongerGravityImage.color;
            tempColor.a = 0.2f;
            strongerGravityImage.color = tempColor;

        }

        //Stronger Gravity
        if (gManager.gravityRatio > 1)
        {
            Color tempColor = normalGravityImage.color;
            tempColor.a = 0.2f;
            normalGravityImage.color = tempColor;

            tempColor = weakerGravityImage.color;
            tempColor.a = 0.2f;
            weakerGravityImage.color = tempColor;

            tempColor = strongerGravityImage.color;
            tempColor.a = 1f;
            strongerGravityImage.color = tempColor;

        }

        //Lighter Gravity
        if (gManager.gravityRatio < 1)
        {
            Color tempColor = normalGravityImage.color;
            tempColor.a = 0.2f;
            normalGravityImage.color = tempColor;

            tempColor = weakerGravityImage.color;
            tempColor.a = 1f;
            weakerGravityImage.color = tempColor;

            tempColor = strongerGravityImage.color;
            tempColor.a = 0.2f;
            strongerGravityImage.color = tempColor;

        }
    }
}
