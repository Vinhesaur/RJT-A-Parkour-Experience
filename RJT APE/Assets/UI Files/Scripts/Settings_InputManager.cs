using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings_InputManager : MonoBehaviour
{
    public GameObject graphicsSS;
    public GameObject audioSS;
    public GameObject keyBindingsSS;
    public GameObject accessibilitySS;
    public GameObject manualSS;


    // Start is called before the first frame update
    void Start()
    {
        graphicsSS.SetActive(true);
        audioSS.SetActive(false);
        keyBindingsSS.SetActive(false);
        accessibilitySS.SetActive(false);
        manualSS.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void graphicsButtonOnClick()
    {
        print("clicked");
        audioSS.SetActive(false);
        keyBindingsSS.SetActive(false);
        accessibilitySS.SetActive(false);
        manualSS.SetActive(false);
        graphicsSS.SetActive(true);
    }

    public void audioButtonOnClick()
    {
        print("clicked");

        keyBindingsSS.SetActive(false);
        accessibilitySS.SetActive(false);
        manualSS.SetActive(false);
        graphicsSS.SetActive(false);
        audioSS.SetActive(true);
    }
    public void keyBindingButtonOnClick()
    {
        print("clicked");

        accessibilitySS.SetActive(false);
        manualSS.SetActive(false);
        graphicsSS.SetActive(false);
        audioSS.SetActive(false);
        keyBindingsSS.SetActive(true);

    }
    public void accessibilityButtonOnClick()
    {
        print("clicked");

        keyBindingsSS.SetActive(false);
        manualSS.SetActive(false);
        graphicsSS.SetActive(false);
        audioSS.SetActive(false);
        accessibilitySS.SetActive(true);
    }

    public void manualButtonOnClick()
    {
        print("clicked");
    
        keyBindingsSS.SetActive(false);
        accessibilitySS.SetActive(false);
        graphicsSS.SetActive(false);
        audioSS.SetActive(false);
        manualSS.SetActive(true);
    }
}
