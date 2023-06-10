using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    [SerializeField] AudioListener audioListener;
    [SerializeField] GameManager gm;
    [SerializeField] Image  muteAllImage;
    [SerializeField] Image  muteMusicImage;
    [SerializeField] Sprite[] muteAllImages;
    [SerializeField] Sprite[] muteMusicImages;
    public GameObject pauseMenuUI;

    
    private bool  muted = false;

    public void MuteMusic()
    {
        if(!muted)
        {
            gm.musicSound.volume = 0f;
            muted = true;
            muteMusicImage.sprite = muteMusicImages[1];

        }

        else
        {
            gm.musicSound.volume = 0.7f;
            muted = false;
            muteMusicImage.sprite = muteMusicImages[0];

        }
        
        
    }

    public void MuteAll()
    {
        if(!muted)
        {
            audioListener.enabled = false;
            muted = true;
            muteAllImage.sprite = muteAllImages[1];
        }

        else
        {
            audioListener.enabled = true;
            muted = false;
            muteAllImage.sprite = muteAllImages[0];
        }
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }
}
