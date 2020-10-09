using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject deathUI;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Start")        
            Cursor.lockState = CursorLockMode.None;        
        else
            Cursor.lockState = CursorLockMode.Locked;
        //deathUI = GameObject.Find("Deathpanel");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartGame()
    {
        var currScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currScene);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void Menu_Button()
    {
        SceneManager.LoadScene("Start");
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void SetDeathUI()
    {
        deathUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
}
