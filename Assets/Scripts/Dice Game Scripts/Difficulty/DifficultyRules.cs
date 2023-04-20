using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Difficulty", menuName = "ScriptableObjects/DifficultyPacket", order = 2)]
public class DifficultyRules : ScriptableObject
{

    public enum Level
    {
        Zero = 0,
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        
    };
    public Level level;
    public List<DiceData> diceOptions;
    public float lockTime;
    public float stepTime;
    private int scoreLimit;//should this be in charge?
    private int lineLimit;
    public int limit;



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
