using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton { get; private set; }
    [SerializeField] private GameObject scoreScreen;
    [SerializeField]private TextMeshProUGUI timeScore;

    private float timer;
    
    [SerializeField] private GameObject buttonRushScreen;
    [SerializeField] private TextMeshProUGUI pressCountText;
    private bool isButtonRushActive = false;
    private int pressCount = 0;
    private bool isUpButtomPressed;

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
                    pressCountText.text = pressCount.ToString();
                    isUpButtomPressed = false;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.W) )
                {
                    pressCount++;
                    pressCountText.text = pressCount.ToString();
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
        
        timeScore.text = minute.ToString("00") + ":" + second.ToString("00");
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
        isButtonRushActive = true;
        buttonRushScreen.SetActive(true);
    }
    
    public int DeactivateButtonRushPhase()
    {
        isButtonRushActive = false;
        buttonRushScreen.SetActive(false);
        return pressCount;
    }
}
