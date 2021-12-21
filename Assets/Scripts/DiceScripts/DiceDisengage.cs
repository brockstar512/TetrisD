using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DiceDisengage : MonoBehaviour
{
    public DiceBoard diceBoard { get; private set; }//anytime the piece moves we need to pass that info to redraw that game piece
    public Vector3Int position { get; private set; }//i believe position is position on board
    const float DisengageDropSpeed = 0.05f;



    public void Initialize(DiceBoard diceBoard)
    {
        this.diceBoard = diceBoard;///this is the only thing here that needs to be initialized
    }

    public void Disengage(DiceData stillData, DiceData travelingDice, Vector3Int holdPos, Vector3Int startPos, Vector3Int finishPos)
    {
        //int leaveDiceBehind = 1 - DiceGroupCellIndex;//if the coninuing dice is 1 this will be 0. if the coninuing dice is 0 this will be 1
        //DiceData stillData = leaveDiceBehind == 0 ? diceGroup.data: diceGroup.dynamicData;
        //DiceData travelingDice = leaveDiceBehind == 1 ? diceGroup.data : diceGroup.dynamicData;
        HoldDiceInPlace(holdPos, stillData);

        //Vector3Int finish = new Vector3Int(position.x, finishPos.y, 0);
        Vector3Int finish = new Vector3Int(position.x, position.y + -5, 0);

        StartCoroutine(TravelingDice(travelingDice, startPos, finish));
        //TravelingDice(travelingDice, position, finish);
        //TODO the only problem i see right now is getting the finish from ghost dice and then pausing until all these functions are done

    }



    /// <summary>
    /// You need to pass in the position of the diceboard it will go, and the Dice data of which dice in the group. Put in those two and
    /// this function will place that dice tile on the diceboard at that position.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="stillData"></param>
    private void HoldDiceInPlace(Vector3Int position, DiceData stillData)
    {
        this.diceBoard.SetSingleDiceOnBoard(position, stillData.tile);
    }

    //private void TravelingDice(DiceData travelingDice, Vector3Int start, Vector3Int finish)
    //{
    //    Vector3Int current = start;
    //    this.diceBoard.SetSingleDiceOnBoard(current, travelingDice.tile);

    //    while (current != finish)
    //    {
    //        this.diceBoard.Clear(current);
    //        current = new Vector3Int(current.x, current.y + -1, 0);
    //        this.diceBoard.SetSingleDiceOnBoard(current, travelingDice.tile);
    //        continue;
    //    }
    //}

    IEnumerator TravelingDice(DiceData travelingDice, Vector3Int start, Vector3Int finish)
    {
        Vector3Int current = start;
        this.diceBoard.SetSingleDiceOnBoard(current, travelingDice.tile);

        while (current != finish)
        {
            this.diceBoard.Clear(current);
            current = new Vector3Int(current.x, current.y + -1, 0);
            this.diceBoard.SetSingleDiceOnBoard(current, travelingDice.tile);
            yield return new WaitForSeconds(DisengageDropSpeed);
        }
        this.diceBoard.SetSingleDiceOnBoard(finish, travelingDice.tile);
        yield return null;
    }


}
public struct FreeDice
{
    public Vector3Int position { get; private set; }//where you want to place it
    public Tile tile;
    public DiceNumber number;


}
