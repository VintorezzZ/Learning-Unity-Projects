using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject deathUI;
    public GameObject optionsPanel;
    public GameObject whilePlayPanel;
    public GameObject optionsButton;
    public Text scoreText;
    public Text deathScoreText;
    public Text deadCountText;
    public Text timeText;
    public Text ammoText;
    public Slider sensSlider;
    public float sens = 100;

    private int points;
    public int deadCount;
    public float time;
    public int ammo;

    public MMFeedbacks shakeFeedback;
    
    private bool gameover = false;
    public bool pause = false;
    //private bool mute;


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
        SoundManager.instance.pauseMute = false;
        SoundManager.instance.MuteAllSounds();
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
            if (!pause)
            {
                pause = true;
                Pause_On();
            }
            else
            {
                pause = false;
                Pause_Off();
            }
        }

        UpdateUI();
    }

    public void Pause_On()
    {
        SoundManager.instance.pauseMute = true;
        Cursor.lockState = CursorLockMode.None;
        optionsPanel.SetActive(true);
        whilePlayPanel.SetActive(false);
        ChangeTimeScale();

        if (SoundManager.instance.globalMute)
            return;

        MuteAllSounds();
    }

    public void Pause_Off()
    {
        SoundManager.instance.pauseMute = false;
        Cursor.lockState = CursorLockMode.Locked;
        optionsPanel.SetActive(false);
        whilePlayPanel.SetActive(true);
        ChangeTimeScale();

        if (SoundManager.instance.globalMute)
            return;

        MuteAllSounds();
    }

    public void ChangeTimeScale()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
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
        optionsPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
    }

    public void CountPoints(int _points)
    {
        points += _points;
        deadCount++;
    }

    void UpdateUI()
    {
        try
        {
            scoreText.text = "Score  " + points.ToString();
            deathScoreText.text = "Score   " + points.ToString();
            timeText.text = "Next wave    \n" + time.ToString("0");
            ammoText.text = ammo.ToString() + "  Ammo";
            deadCountText.text = "Cars destroyed   " + deadCount.ToString();
        }
        catch (System.Exception)
        {
            Debug.Log("Need to fix this");
        }
       
    }

    public void UpdateTimer(float _time)
    {
        time = _time;
    }

    public void UpdateAmmo(int _ammo)
    {
        ammo = _ammo;
    }

    public void PlayFeedbacks()
    {
        shakeFeedback.PlayFeedbacks();
    }

    public void MuteBgMusic()
    {
        SoundManager.instance.MuteBgMusic();
    }

    public void MuteAllSounds()
    { 
        SoundManager.instance.MuteAllSounds();    
    }

    public void GlobalMute()
    {
        SoundManager.instance.globalMute = !SoundManager.instance.globalMute;  
    }

    public void SetSensetivity()
    {
        sens = sensSlider.value;
    }
}
