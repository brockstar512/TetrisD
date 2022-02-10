using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField ]GameObject countDownPrefab;
    [SerializeField] GameObject countUpPrefab;
    public bool isCountUp;
    private GameObject clock;

    [SerializeField] DiceMatch diceMatch;
    [SerializeField] DiceBoard gameBoard;


    [SerializeField] DifficultyManager difficultyManager;
    //this script will have everything from the menu so it should also be the one to tell the difficult


    [ContextMenu("Start")]
    public void StartGame()
    {
        switch (isCountUp)
        {
            case true:
                clock = Instantiate(countUpPrefab);
                break;
            case false:
                clock = Instantiate(countDownPrefab);
                break;
        }
        
        clock.GetComponent<ScoreManager>().Init(diceMatch);
        gameBoard.Init(clock.GetComponent<IClock>());

        clock.GetComponent<IClock>().StartGame();

        difficultyManager.StartGame();
    }



}
