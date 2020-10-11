using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public bool muteMusic = false;
    public AudioSource music;
    private AudioListener audioListener;
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

        DontDestroyOnLoad(this);
    }

    public void MuteBgMusic()
    {
        muteMusic = !muteMusic;
        music.mute = !music.mute;
    }


    public void MuteAllSounds()
    {
        if (GameManager.instance.globalMute == true)
        {
            AudioListener.volume = 0;
            return;
        }
        else if (GameManager.instance.globalMute == false)
        {
            if (AudioListener.volume == 1)
                AudioListener.volume = 0;
            else
                AudioListener.volume = 1;
        }

    }
 
}
