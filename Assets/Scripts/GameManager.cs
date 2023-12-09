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
    [SerializeField] private TextMeshProUGUI timeScore;
    [SerializeField] private TextMeshProUGUI eatScoreTM;
    [SerializeField] private TextMeshProUGUI eatHighScoreTM;

    
    [SerializeField] private GameObject buttonRushScreen;
    [SerializeField] private Slider pressCountSlider;
    private bool isButtonRushActive = false;
    private int pressCount = 0;
    private bool isUpButtomPressed;

    [SerializeField] private HighScore highScore;
    private int eatScore;
    private float timer;

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
        timer = 0;
        eatScore = 0;
    }
    
    void Update () {
        
        timer += Time.deltaTime;
        
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
    }
    

    public void GameOver()
    {
        scoreScreen.gameObject.SetActive(true);
        float minute = timer / 60;
        float second = timer % 60;

        
        if (eatScore > highScore.GetHighScore())
        {
            highScore.SetHighScore(eatScore);
        }
        
        eatScoreTM.text = eatScore.ToString();
        timeScore.text = minute.ToString("00") + ":" + second.ToString("00");
        eatHighScoreTM.text = highScore.GetHighScore().ToString();
        
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
    
    public void AddScore(int score)
    {
        eatScore += score;
    }
}
