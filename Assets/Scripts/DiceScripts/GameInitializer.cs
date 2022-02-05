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
    }
    //refs of all clocks
    //pass them the references they dont have nested
    //start the game coroutine


}
