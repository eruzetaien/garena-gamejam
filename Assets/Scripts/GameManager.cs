using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton { get; private set; }
    [SerializeField] private GameObject scoreScreen;
    [SerializeField] private TextMeshProUGUI scoreInGameTM;
    [SerializeField] private TextMeshProUGUI scoreTM;
    [SerializeField] private TextMeshProUGUI highScoreTM;

    
    [SerializeField] private GameObject buttonRushScreen;
    [SerializeField] private Slider pressCountSlider;
    private bool isButtonRushActive = false;
    private int pressCount = 0;
    private bool isUpButtomPressed;

    [SerializeField] private GameObject pauseMenu;
    
    [SerializeField] private HighScore highScore;
    private int score;
    private float secondTimer = 0f;   

    public void Awake()
    {
        if (Singleton != null && Singleton != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Singleton = this;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        InvokeRepeating("AddTimeScore",1f,1f);
    }
    
    void Update () {
        if (isButtonRushActive)
        {
            if (isUpButtomPressed)
            {
                if (Input.GetKeyDown(KeyCode.S) )
                {
                    pressCount++;
                    pressCountSlider.value = pressCount;
                    isUpButtomPressed = false;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.W) )
                {
                    pressCount++;
                    pressCountSlider.value = pressCount;
                    isUpButtomPressed = true;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Pause");
            PauseGame();
        }
    }
    

    public void GameOver()
    {
        scoreScreen.gameObject.SetActive(true);

        if (score > highScore.GetHighScore())
        {
            highScore.SetHighScore(score);
        }
        
        scoreTM.text = score.ToString();
        highScoreTM.text = highScore.GetHighScore().ToString();
        
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        scoreScreen.gameObject.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ActivateButtonRushPhase()
    {
        pressCount = 0;
        pressCountSlider.value = pressCount;
        
        isButtonRushActive = true;
        buttonRushScreen.SetActive(true);
    }
    
    public int DeactivateButtonRushPhase()
    {
        isButtonRushActive = false;
        buttonRushScreen.SetActive(false);
        return pressCount;
    }

    public void AddTimeScore()
    {
        AddScore(1);
    }

    public void AddScore(int addedScore)
    {
        score += addedScore;
        scoreInGameTM.text = "Score : " +  score.ToString();
    }
    
    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }
    
    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }
    
    public void QuitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }
}
