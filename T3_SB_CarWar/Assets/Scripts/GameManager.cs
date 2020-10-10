using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject deathUI;
    public GameObject optinonsPanel;
    public GameObject whilePlayPanel;
    public GameObject optionsButton;
    public Text scoreText;
    public Text deathScoreText;
    public Text deadCountText;
    public Text timeText;
    public Text ammoText;

    bool Pause = false;
    private int points;
    public int deadCount;
    public float time;
    public int ammo;
    bool gameover = false;
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
        points = 0;
        Time.timeScale = 1;
        if (SceneManager.GetActiveScene().name == "Start")        
            Cursor.lockState = CursorLockMode.None;        
        else
            Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (gameover)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {            

            if (!Pause)
            {
                Cursor.lockState = CursorLockMode.None;
                optinonsPanel.SetActive(true);
                whilePlayPanel.SetActive(false);
                //optionsButton.SetActive(false);
                //ChangeTimeScale();
                Pause = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                optinonsPanel.SetActive(false);
                whilePlayPanel.SetActive(true);
                //optionsButton.SetActive(true);
                Pause = false;
                //ChangeTimeScale();
            }
            
            //Time.timeScale = -Time.timeScale;
        }

        UpdateUI();

    }


    public void ChangeTimeScale()
    {
        //if (Time.timeScale == 1)
        //{
        //    Time.timeScale = 0;
        //}
        //else
        //{
        //    Time.timeScale = 1;
        //}

        //if (Cursor.lockState == CursorLockMode.Locked)
        //{
        //    Cursor.lockState = CursorLockMode.None;
        //}
        //else
        //    Cursor.lockState = CursorLockMode.Locked;

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
        gameover = true;
        deathUI.SetActive(true);
        whilePlayPanel.SetActive(false);
        optinonsPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
    }

    public void CountPoints(int _points)
    {
        points += _points;
        deadCount++;
    }

    void UpdateUI()
    {
        scoreText.text = "Score  " + points.ToString();
        deathScoreText.text = "Score   " + points.ToString();
        timeText.text = "Next wave    \n" + time.ToString("0");
        ammoText.text = ammo.ToString() + "  Ammo";
        deadCountText.text = "Cars destroyed   " + deadCount.ToString();
    }

    public void UpdateTimer(float _time)
    {
        time = _time;
    }

    public void UpdateAmmo(int _ammo)
    {
        ammo = _ammo;
    }
}
