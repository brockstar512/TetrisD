using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DifficultyManager : MonoBehaviour
{
    private List<DifficultyRules> difficultyOptions;
    public List<DifficultyRules> difficultyOptionsDefault;
    DifficultyRules currentDifficulty;
    [SerializeField] TextMeshProUGUI difficultyNumbers;
    [SerializeField] TextMeshProUGUI linesClear;
    [SerializeField] AudioClip levelUpSoundFX;
 
    //if null return 0
    public int GetDifficultyIndex { get { return currentDifficulty != null ? (int)currentDifficulty.level : 0; } }
    public float GetDifficultyStepTime{ get { return currentDifficulty.stepTime; } }
    public float GetDifficultyLocktime{ get { return currentDifficulty.lockTime; } }
    public int GetCurrentLimitGoal { get { return currentDifficulty.limit; } }
    public int CurrentLimit { get; private set; }


    //this starts us where we need to be
    //we are going to pass in a packet that gives us if we should use bomba dn what stage and level to start on
    public void StartGame(int level = 0)
    {

        //get the correct game mode
        difficultyOptions = difficultyOptionsDefault;

        UpdateLevel();
    }

    //this removes the bombs and updates the selection of the dice
    void UpdateLevel(int increase = 0)
    {
        
        //gets the current difficulty packet
        currentDifficulty = Instantiate(difficultyOptions[GetDifficultyIndex + increase]);
        //sets your difficulty so you don't have to play all the way up to where you are to level up
        CurrentLimit =  difficultyOptions[GetDifficultyIndex].limit;

        //updates the UI
        if ((int)currentDifficulty.level < 10)
        {
            UpdateLevelUI(((int)currentDifficulty.level).ToString());
        }
    }

 
 
    //this checks if we need to update base on
    public bool UpdateLevelCheck(int value)
    {
        linesClear.text = value.ToString();

        if (value < GetCurrentLimitGoal)
            return false;
        SoundManager.Instance.PlaySound(levelUpSoundFX);
        UpdateLevel(1);
        return true;
        
    }

    //[margin between numbers is the chance they'll be chosen]
    public DiceData DiceFactory()
    {
        int numberRoll = Random.Range(1, 100);
        int rolledIndex = -1;
        Debug.Log($"rolled number {numberRoll}");

        for (int i = 0; i < currentDifficulty.diceOptions.Count - 1;i++)
        {
            if(currentDifficulty.diceOptions[i].chance <= numberRoll)
            {
                Debug.Log($"DICE FACTORY chance {currentDifficulty.diceOptions[i].chance}");

                rolledIndex = i;
                continue;
            }
            break;
        }

        return currentDifficulty.diceOptions[rolledIndex];
    }

    void UpdateLevelUI(string level)
    {
        difficultyNumbers.text = $"{level}";
    }

}
