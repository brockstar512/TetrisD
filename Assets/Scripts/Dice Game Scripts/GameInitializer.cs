using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{

    [SerializeField] GameObject clockGameObject;
    public bool isCountUp;
    private GameObject clock;

    [SerializeField] DiceMatch diceMatch;
    [SerializeField] DiceBoard gameBoard;
    [SerializeField] DiceGroup diceGroup;


    [SerializeField] DifficultyManager difficultyManager;
    //this script will have everything from the menu so it should also be the one to tell the difficult

    private void Awake()
    {
        LoadingManager.Instance.OnNewScene += StartGame;

    }
    private void OnDestroy()
    {
        LoadingManager.Instance.OnNewScene -= StartGame;

    }
    [ContextMenu("Start")]
    public void StartGame()
    {
        Debug.Log($"We are playing difficulty {GameSetUp.difficulty} with game mode for {GameSetUp.gameType}");
        CountDown countDown = clockGameObject?.GetComponent<CountDown>();
        CountUp countUp = clockGameObject?.GetComponent<CountUp>();


        switch (isCountUp)
        {
            case true:
                clock = clockGameObject;
                countDown.enabled = false;
                countUp.enabled = true;
                break;
            case false:
                clock = clockGameObject;
                countDown.enabled = true;
                countUp.enabled = false;
                break;
        }
        
        clock.GetComponent<ScoreManager>().Init(diceMatch, diceGroup, difficultyManager);
        gameBoard.Init(clock.GetComponent<IClock>());

        clock.GetComponent<IClock>().StartGame();

        difficultyManager.StartGame(GameSetUp.difficulty);
    }



}
