using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiMainMenu : MonoBehaviour
{
    [SerializeField]
    private HighScore highScore;

    [SerializeField] private TMP_Text highScoreTM;
    
    // Start is called before the first frame update
    void Start()
    {
        highScoreTM.text = highScore.GetHighScore().ToString();
        SoundManager.soundManager.PlayLooping("mm");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    public void Play()
    {
        SceneManager.LoadScene("Cerita");
    }
    
    
}
