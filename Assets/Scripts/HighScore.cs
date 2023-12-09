using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "Highscore", menuName = "ScriptableObjects/Highscore")]
    public class HighScore :ScriptableObject
    {
        [SerializeField] private int highScore = 0;
        
        public int GetHighScore()
        {
            return highScore;
        }
        
        public void SetHighScore(int newHighScore)
        {
            highScore = newHighScore;
        }
    }
}