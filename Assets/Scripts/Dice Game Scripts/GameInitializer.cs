using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{

    [SerializeField] GameObject clockGameObject;

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


        clockGameObject.GetComponent<ScoreManager>().Init(diceMatch, diceGroup, difficultyManager);
        gameBoard.Init(clockGameObject.GetComponent<IClock>());
        clockGameObject.GetComponent<IClock>().StartGame();
        

        difficultyManager.StartGame(GameSetUp.difficulty);
    }


}
