using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuterShooter : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject PauseMenuUI;
    AudioSource audioData;

    // Start is called before the first frame update
    void Start()
    {
        audioData = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {

            if (GameIsPaused)
            {
                audioData = GetComponent<AudioSource>();
                audioData.Play(0);
                Debug.Log("started");
                Resume();
            }
            else
            {
               
                audioData.Pause();
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
    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

    }
}
