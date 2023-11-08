using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameGUIScript : MonoBehaviour
{
    [SerializeField]
    GameObject GameGUI;

    [SerializeField]
    TMP_Text scoreText;

    [SerializeField]
    GameObject hintText;

    [SerializeField]
    GameObject speedUpText;

    [SerializeField]
    GameObject PauseMenu;

    [SerializeField]
    GameObject EndMenu;

    [SerializeField]
    TMP_Text finalScoreText;

    // Start is called before the first frame update
    void Start()
    {
               
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setScore(int score)
    {        
        scoreText.text = score.ToString();
    }

    IEnumerator initGui()
    {
        GameGUI.SetActive(true);
        PauseMenu.SetActive(false);
        EndMenu.SetActive(false);
        yield return new WaitForSeconds(5);
        hintText.SetActive(false);
        scoreText.gameObject.SetActive(true);
    }

    IEnumerator SpeedUp()
    {
        speedUpText.SetActive(true);
        yield return new WaitForSeconds(3);
        speedUpText.SetActive(false);
    }

    public void Pause()
    {
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
        GameGUI.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
        GameGUI.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2);
        finalScoreText.text = "Final score: " + scoreText.text;
        GameGUI.SetActive(false);
        EndMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }    

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
