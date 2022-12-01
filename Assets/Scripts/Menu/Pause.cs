using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Player.PlayerController.Instance.GetComponent<ActivatePause>().enabled = true;

            Time.timeScale = 1;
            Destroy(gameObject);
        }
    }

    public void RestartButtonClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitButtonClick()
    {
        Application.Quit();
    }
}
