using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eScoreEvent{
    draw,
    mine,
    gameWin,
    gameLoss,
}

public class ScoreManager : MonoBehaviour
{
    static private ScoreManager S;

    static public int SCORE_FROM_PREV_ROUND = 0;
    static public int SOCRE_THIS_ROUND = 0;
    static public int HIGH_SCORE = 0;

    [Header("Inscribed")]
    [Tooltip("If true, then score events are logged to the Console.")]
    public bool logScoreEvents = true;

    [Header("Dynamic")]
    public int chain = 0;
    public int scoreRun = 0;
    public int score = 0;

    [Header("Check this box to reset the ProspectorHighScore to 100")]
    public bool checkToResetHighScore = false;

    void Awake()
    {
        if (S != null) Debug.LogError("ScoreManager.S is already set!");
        S = this;
    
        if(PlayerPrefs.HasKey ("ProspectorHighScore")){
            HIGH_SCORE = PlayerPrefs.GetInt("ProspectorHighScore");
        }

        score += SCORE_FROM_PREV_ROUND;
        SOCRE_THIS_ROUND = 0;
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
