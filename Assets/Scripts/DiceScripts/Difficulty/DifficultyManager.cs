using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DifficultyManager : MonoBehaviour
{
    public List<DifficultyRules> difficultyOptions;
    public DifficultyRules currentDifficulty;
    [SerializeField] TextMeshProUGUI difficultyNumbers;
    private bool useBombs;

    public int level { get; private set; }
    public int stage { get; private set; }
    public int GetDifficultyIndex { get { return (level * 5) + stage; } }
    public float GetDifficultyStepTime
    {
        get
        {
            //Debug.Log($"Your step time is: {currentDifficulty.stepTime}");
            return currentDifficulty.stepTime;
        }
    }
    public float GetDifficultyLocktime
    {
        get
        {
            //Debug.Log($"Your lock time is: {currentDifficulty.lockTime}");
            return currentDifficulty.lockTime;
        }
    }
    public int GetCurrentLimit = 100;//todo figure out what the threshold is to level up
    //public static DifficultyManager Instance { get; private set; }
    //private static DifficultyManager instance = null;
    //public static DifficultyManager Instance
    //{
    //    get
    //    {
    //        if (instance == null)
    //        {
    //            instance = new DifficultyManager();
    //        }
    //        return instance;
    //    }
    //}

    //this starts us where we need to be
    //we are going to pass in a packet that gives us if we should use bomba dn what stage and level to start on
    public void StartGame(int level = 0, int stage = 0, bool useBombs = true)
    {
        //we get the level we selected
        this.level = level;
        //we get  the stage we selected
        this.stage = stage;
        //we got if we want to inlclude bombs
        this.useBombs = useBombs;
        UpdateLevel();
    }

    //this removes the bombs and updates the selection of the dice
    void UpdateLevel()
    {
        currentDifficulty = difficultyOptions[GetDifficultyIndex];
        if (useBombs)
        {
            currentDifficulty.RemoveBombs();
        }
        UpdateLevelUI(level, stage);
    }

    //this checks if we need to update base on
    bool UpdateLevelCheck(int value)
    {
        if (value < GetCurrentLimit)
            return false;

        if (level == 2 && stage == 4)
            return false;
        switch (stage)
        {
            case 4:
                stage = 0;
                level++;
                break;
            default:
                stage++;
                break;
        }
            UpdateLevel();
        return true;
        
    }

    //[margin between numbers is the chance they'll be chosen]
    public DiceData DiceFactory()
    {
        int numberRoll = Random.Range(1, 100);
        int rolledIndex = -1;
        //Debug.Log($"rolled number {numberRoll}");

        for (int i = 0; i < currentDifficulty.diceOptions.Count - 1;i++)
        {
            if(currentDifficulty.diceOptions[i].chance <= numberRoll)
            {
                //Debug.Log($"DICE FACTORY chance {currentDifficulty.diceOptions[i].chance}");

                rolledIndex = i;
                continue;
            }
            break;
        }
        //Debug.Log($"DICE FACTORY {rolledIndex} and rolled number {numberRoll} which will give you  {currentDifficulty.diceOptions[rolledIndex].number}");
        //Debug.Log($"DICE FACTORY {rolledIndex} and the options {currentDifficulty.diceOptions[rolledIndex]}");

        return currentDifficulty.diceOptions[rolledIndex];
    }

    void UpdateLevelUI(int level = 0, int stage = 0)
    {
        difficultyNumbers.text = $"{level + 1}-{stage + 1}";
    }



}
