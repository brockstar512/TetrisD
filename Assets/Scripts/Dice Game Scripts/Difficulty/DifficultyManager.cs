using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DifficultyManager : MonoBehaviour
{
    private List<DifficultyRules> difficultyOptions;
    public List<DifficultyRules> difficultyOptionsDefault;

    //public List<DifficultyRules> difficultyOptionsClassic;
    //public List<DifficultyRules> difficultyOptionsConfetti;
    //public List<DifficultyRules> difficultyOptionsFunfetti;


    public DifficultyRules currentDifficulty;
    [SerializeField] TextMeshProUGUI difficultyNumbers;
    [SerializeField] TextMeshProUGUI linesClear;

    private bool useBombs;

    public int level { get; private set; }
    public int stage { get; private set; }
    public int GetDifficultyIndex { get { return (level * 5) + stage; } }
    public float GetDifficultyStepTime{ get { return currentDifficulty.stepTime; } }
    public float GetDifficultyLocktime{ get { return currentDifficulty.lockTime; } }
    public int GetCurrentLimitGoal { get { return currentDifficulty.limit; } }
    public int CurrentLimit{ get; private set; }
//todo figure out what the threshold is to level up


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
        //get the correct game mode
        difficultyOptions = difficultyOptionsDefault;
        UpdateLevel();
    }

    //this removes the bombs and updates the selection of the dice
    void UpdateLevel()
    {
        Debug.Log($"Here is diffuculty index {GetDifficultyIndex}");
        //gets the current difficulty packet
        currentDifficulty = Instantiate(difficultyOptions[GetDifficultyIndex]);
        //sets your difficulty so you don't have to play all the way up to where you are to level up
        CurrentLimit = level == 0 && stage == 0 ? 0 : difficultyOptions[GetDifficultyIndex - 1].limit;

        //updates the UI
        UpdateLevelUI(level, stage);
    }

 
 
    //this checks if we need to update base on
    public bool UpdateLevelCheck(int value)
    {
        linesClear.text = value.ToString();

        if (value < GetCurrentLimitGoal)
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

        return currentDifficulty.diceOptions[rolledIndex];
    }

    void UpdateLevelUI(int level = 0, int stage = 0)
    {
        difficultyNumbers.text = $"{level + 1}-{stage + 1}";
    }






}
