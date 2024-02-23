using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TabBar_Script : MonoBehaviour
{
    Animator anim;

    public bool tabBarShow;
    public bool tabBarHide;

    public bool tabBarMainButtonClicked;

    public TextMeshProUGUI tabBarMainButtonText;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        tabBarShow = false;
        tabBarHide = true;

        tabBarMainButtonClicked = false;

        tabBarMainButtonText.text = "OPEN";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2) || tabBarMainButtonClicked == true)
        {
            if(tabBarHide == true)
            {
                //print("yes");
                anim.SetTrigger("SlideRight");
                tabBarMainButtonText.text = "CLOSE";

                tabBarShow = true;
                tabBarHide = false;
            }

            else if (tabBarShow == true)
            {
                //print("yes");
                anim.SetTrigger("SlideLeft");
                tabBarMainButtonText.text = "OPEN";

                tabBarHide = true;
                tabBarShow = false;

            }

            tabBarMainButtonClicked = false;

        }
    }

    public void OnTabBarMainButtonClick()
    {
        tabBarMainButtonClicked = true;
    }
}
