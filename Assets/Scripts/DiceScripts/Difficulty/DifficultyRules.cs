using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Difficulty", menuName = "ScriptableObjects/DifficultyPacket", order = 2)]
public class DifficultyRules : ScriptableObject
{
    public enum Difficulty
    {
        Easy = 0,
        Medium,
        Hard
    };
    public enum Stage
    {
        One = 0,
        Two,
        Three,
        Four,
        Five,
    };
    //public DiceData[] DiceOptions;
    public Difficulty difficulty;
    public Stage stage;


    public List<DiceData> diceOptions;
    public float lockTime;
    public float stepTime;
    private int scoreLimit;//should this be in charge?
    private int lineLimit;
    public int limit;

    //public DifficultyRules Copy(DifficultyRules difficultyRules)
    //{
    //    DifficultyRules difficultyRulesCopy = new DifficultyRules();
    //    return new DifficultyRules();
    //}

    public void RemoveBombs()
    {
        for(int i = 0; i < diceOptions.Count; i++)
        {
            if(diceOptions[i].number == DiceNumber.Seven)
            {
                diceOptions.RemoveAt(i);
                i--;
            }

        }
    }

}
