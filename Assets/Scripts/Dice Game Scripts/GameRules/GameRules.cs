using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameRules : MonoBehaviour
{
    //if I want to make a online version I probably have to delete this script. its too referenced
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] Clock clock;
    [SerializeField] DifficultyManager DifficultyManager;
    [SerializeField] DiceBoard diceBoard;
    [SerializeField] GameObject gameOverScorePanel;
    [SerializeField] GameObject highScoreText;
    [SerializeField] TextMeshProUGUI highScoreValue;
    const int linebreakerLimit = 10;
    //for score
    public void LineBreaker()
    {
        if (scoreManager.diceLinesCleared < linebreakerLimit)
            return;
        EndGame();
        diceBoard.GameOver();
    }

    
    public void TimeAttack()
    {
        diceBoard.GameOver();
        EndGame();
        clock.ResetClock();
    }

    public void Marathon()
    {
        EndGame();
    }

    private void EndGame()
    {
        clock.isCounting = false;
        gameOverScorePanel.SetActive(true);

        int scoreTarget = GetHighscore();

        bool isNewHighscore = scoreManager.currentScore > scoreTarget;
        Debug.Log($"old score {scoreTarget} and new score {scoreManager.currentScore}");

        if (isNewHighscore)
        {
            highScoreText.SetActive(true);
        }
        DataStore.Instance.playerData.UpdateScore(GameSetUp.gameType, scoreManager.currentScore);
        highScoreValue.text = scoreManager.currentScore.ToString();

    }

    int GetHighscore()
    {
        switch (GameSetUp.gameType)
        {
            case GameSetUp.GameType.Marathon:
                return DataStore.Instance.playerData.marathonHighscore;
            case GameSetUp.GameType.LineBreaker:
                return DataStore.Instance.playerData.lineBreakerHighscore;
            case GameSetUp.GameType.TimeAttack:
                return DataStore.Instance.playerData.timeAttackHighscore;
        }
        return 0;
    }




}
