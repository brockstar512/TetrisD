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

    [System.Serializable]
    public class DiceRate
    {
        public int chance;
        public DiceData option;
    }

    public DiceRate[] diceOptions;
    public float lockTime;
    public float stepTime;
    private int scoreLimit;//should this be in charge?
    private int lineLimit;

    //public DiceData SupplyDice()
    //{
    //    return DiceData;
    //}

}
