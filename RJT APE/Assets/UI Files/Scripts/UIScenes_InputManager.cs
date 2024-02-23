using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScenes_InputManager : MonoBehaviour
{
    private int sceneNumber;

    // Start is called before the first frame update
    void Start()
    {
        sceneNumber = SceneManager.GetActiveScene().buildIndex;
        //print(sceneNumber);
    }

    // Update is called once per frame
    void Update()
    {
        if ((sceneNumber == 2) && Input.GetKeyDown(KeyCode.Mouse0))
        {
            SceneManager.LoadScene(3);
        }
    }

    public void ToMainMenuScene()
    {
        SceneManager.LoadScene(1);
    }
    public void ToGUIScene()
    {
        SceneManager.LoadScene(2);
    }
    public void ToHomeSceneScene()
    {
        SceneManager.LoadScene(3);
    }
    public void ToCustomizationScene()
    {
        SceneManager.LoadScene(4);
    }
    public void ToProgressScene()
    {
        SceneManager.LoadScene(5);
    }
    public void ToSocialScene()
    {
        SceneManager.LoadScene(6);
    }
    public void ToShopScene()
    {
        SceneManager.LoadScene(7);
    }
    public void ToSettingsFromHomeScene()
    {
        SceneManager.LoadScene(8);
    }
}
