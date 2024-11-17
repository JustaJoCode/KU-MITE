using UnityEngine;
using UnityEngine.SceneManagement;
public class MrControls : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            Debug.Log("Intro");
            
            SceneManager.LoadScene("Intro");
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Debug.Log("Intro");

            SceneManager.LoadScene("Intro");
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

}
