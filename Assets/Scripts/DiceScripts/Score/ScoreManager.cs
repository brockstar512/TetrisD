using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public DiceMatch diceMatch;
    public int currentScore = 0;
    public int scoreToAdd = 0;
    NumberCounterUpdater numberCounterUpdater;
    NumberCounter numberCounter;
    // Start is called before the first frame update
    public void Init(DiceMatch diceMatch)
    {
        numberCounterUpdater = this.gameObject.transform.GetComponent<NumberCounterUpdater>();
        numberCounter = this.gameObject.transform.GetComponent<NumberCounter>();

        diceMatch.scoreEvent += Bookend;
        diceMatch.bombEvent += Bomb;

    }



    void HardDropBonus(int depth)
    {

        //depth / did break?
    }

    public void Bookend(int DiceNumber, bool isSameColor, int chain)//6 * 1.5
    {
        Debug.Log($"<color=yellow>{chain}</color>");

        //What about the dice in between? is clearning that amount a bonus enough
        //or should there be extra for the inbetween dice
        //Numbers
        //# * 1.1
        //Same color lines
        //# * 1.5
        float bonus = isSameColor ? 17.5f : 12.5f;
        int score = Mathf.CeilToInt(DiceNumber * bonus) * Mathf.CeilToInt(WaterFallBonus(chain));
        scoreToAdd += score;
        Debug.Log($"scoreToAdd:: {scoreToAdd}");
        //numberCounterUpdater.SetValue(scoreToAdd);
        UpdateScore(scoreToAdd);

    }


    public void Bomb(int DiceNumber)
    {
        //# of dice involved? * .75 (every square involved)//8 * .75 = 6
        scoreToAdd+= Mathf.CeilToInt(DiceNumber * 7.5f);
        UpdateScore(scoreToAdd);

    }

    void UpdateScore(int score)
    {
        numberCounter.Value = score;
    }

    private float WaterFallBonus(int chain)
    {
        float multiplier = 1;
        switch (chain)
        {
            case 1:
                multiplier = 1;
                break;
            case 2:
                multiplier = 1.2f;
                break;
            case 3:
                multiplier = 1.4f;
                break;
            case 4:
                multiplier = 1.6f;
                break;
            case 5:
                multiplier = 1.8f;
                break;
            case 6:
                multiplier = 2f;
                break;
            default:
                break;
        }

        return multiplier;
    }
}
/*
 * 
Numbers

# * 1.1

Same color lines

# * 1.5



Bomb

# * .75



//animate chain
//animate you wether its the 





-How long can you go before you fail

-# of lines 3/5  dice disappear. Numbers of dice correspond with the number you need to get to.  If you dissolve 6 whether its a line or bookends and you were at 4/30 it’s now 10/30...if its out x out of x lines then add whoever finished faster  or whoever did it faster gets a bonus but does not win


-How many points can you get in a given time




Waterfall chain (keep it decimal but round up)

2 = 1.2

3 =  1.4

4 =  1.6

5 =  1.8

6 =  2





Multiple removal bonus?
 * */