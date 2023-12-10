using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
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
    [SerializeField] private Animator buttonRushAnimator;
    
    [SerializeField] private Slider pressCountSlider;
    private bool isButtonRushActive = false;
    private int pressCount = 0;
    private const int baseTargetPressCount = 15;
    private const int baseTargetPressCount_next = 25;
    private int targetPressCount;
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
                    Camera.main.GetComponent<Animator>().SetTrigger("heavyShake");
                    pressCount++;
                    pressCountSlider.value = (float) pressCount/targetPressCount;
                    Debug.Log( (float) pressCount/targetPressCount);
                    isUpButtomPressed = false;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.W) )
                {
                    Camera.main.GetComponent<Animator>().SetTrigger("heavyShake");
                    pressCount++;
                    pressCountSlider.value = (float) pressCount/targetPressCount;
                    Debug.Log( (float) pressCount/targetPressCount);
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
        if (score <= 1000)
        {
            targetPressCount = baseTargetPressCount + (int) (score / 100);
        }
        else
        {
            targetPressCount = baseTargetPressCount_next + (score/1000);
        }

        pressCount = 0;
        pressCountSlider.value = 0;
        
        isButtonRushActive = true;
        buttonRushScreen.SetActive(true);
    }
    
    public void DeactivateButtonRushPhase()
    {
        isButtonRushActive = false;
        buttonRushScreen.SetActive(false);
        // return IsButtonRushSuccess();
    }
    
    public void PlaySuccessAnimation()
    {
        buttonRushAnimator.SetTrigger("success");
    }
    
    public bool IsButtonRushSuccess()
    {
        return pressCount >= targetPressCount;
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
