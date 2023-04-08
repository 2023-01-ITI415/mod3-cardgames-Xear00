using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITextManager : MonoBehaviour
{ 
    private static UITextManager S;

    [Header("Inscribed")]
    public TMP_Text gameOverText;
    public TMP_Text roundResultText;
    public TMP_Text highScoreText;

    [Header("Dynamic")]
    [SerializeField]
    private bool _resultsUIFieldVisible = false;

    public bool resultsUIFieldVisible {
        get {return _resultsUIFieldVisible;}
        private set {
            _resultsUIFieldVisible = value;
            gameOverText.gameObject.SetActive(value);
            roundResultText.gameObject.SetActive(value);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (S != null) Debug.LogWarning("Attempt to set Singleton S again!");
        S = this;

        ShowHighScore();
        resultsUIFieldVisible = false;    
    }

    void ShowHighScore(){
        string str = $"High Score: {ScoreManager.HIGH_SCORE:#,##0}";
        highScoreText.text = str;
    }

    static public void GAME_OVER_UI(bool won) {
        S.GameOverUI(won);
    }



    public void GameOverUI(bool won){
        int score = ScoreManager.SCORE;
        string str;
        if (won) {
            gameOverText.text = "Round Over";
            str = "You won this round!\n"
                + $"Round Score: {ScoreManager.SCORE_THIS_ROUND:#,##0}";
        } else {
            gameOverText.text = "Game Over";
            if (ScoreManager.HIGH_SCORE <= score) { 
                str = $"You got the high score!\nHigh score: {score:#,##0}";
            } else { 
                str = $"Your final score was:\n{score:#,##0}";
            }
        }
        roundResultText.text = str;
        resultsUIFieldVisible = true;
        ShowHighScore();
    }
    
}
