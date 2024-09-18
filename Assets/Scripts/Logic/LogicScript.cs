using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogicScript : MonoBehaviour
{
    public GameObject gameOverScreen;
    private bool isPaused = false;
    private GameObject player;
    public AlienScript aliens;

    private float timer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        aliens = GameObject.FindGameObjectWithTag("Alien").GetComponent<AlienScript>();
        player = GameObject.FindGameObjectWithTag("Player");
        //ResumeGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            timer += Time.deltaTime;
        }
        // Comment this out
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    public void RestartGame()
    {
        ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GameOver()
    {
        if (player != null)
        {
            player.SetActive(false);
        }

        PauseGame();
        gameOverScreen.SetActive(true);
    }
    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        aliens.StopHorde();
        CanvasGroup canvasGroup = gameOverScreen.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true; // Allows clicks to go through
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        aliens.StartHorde();
    }

    public void LoadExit()
    {
        // Store the timer value in PlayerPrefs to access it in the end scene
        PlayerPrefs.SetFloat("LevelTime", timer);

        // Load the end scene (replace "EndScene" with your actual scene name)
        SceneManager.LoadScene("EndScene");
    }
}
