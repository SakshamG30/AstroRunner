using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScript : MonoBehaviour
{
    public Canvas titleCanvas;
    public Canvas instructionCanvas;
    // Start is called before the first frame update
    void Start()
    {
        titleCanvas.gameObject.SetActive(true);

        instructionCanvas.gameObject.SetActive(false);
        //LoadTitleScene();
        ResetTutorial();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    public void LoadTitleScene()
    {
        SceneManager.LoadScene("TitleScene", LoadSceneMode.Single);
    }

    public void ResetTutorial()
    {
        PlayerPrefs.DeleteKey("TutorialComplete");
        PlayerPrefs.Save();
    }

    public void GoToInstruction()
    {
        titleCanvas.gameObject.SetActive(false);

        instructionCanvas.gameObject.SetActive(true);
    }

    public void GoToTitle()
    {
        titleCanvas.gameObject.SetActive(true);

        instructionCanvas.gameObject.SetActive(false);
    }
}
