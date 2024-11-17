using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject PauseMenuUI;
    AudioSource audioData;
    AudioSource audioClip;

    // Start is called before the first frame update
    void Start()
    {
        audioClip = GetComponent<AudioSource>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            
            if(GameIsPaused)
            {
                audioClip.Pause();
                Resume();
            }
            else
            {
                audioClip = GetComponent<AudioSource>();
                audioClip.Play(0);
                Debug.Log("started");
                Pause();
            }
        }
        
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene("Intro");
    }
    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

    }

    public void QuitGame()
    {
        Debug.Log("Quiting!");
        Application.Quit();
    }
}
