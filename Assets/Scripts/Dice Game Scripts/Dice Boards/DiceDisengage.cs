using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Threading.Tasks;


public class DiceDisengage : MonoBehaviour
{
    public DiceFXController diceFXController;

    public DiceBoard diceBoard { get; private set; }//anytime the piece moves we need to pass that info to redraw that game piece
    public DiceGroup diceGroup { get; private set; }
    public Vector3Int position { get; private set; }//i believe position is position on board
    const float DisengageDropSpeed = 0.05f;



    public void Initialize(DiceBoard diceBoard)
    {
        this.diceBoard = diceBoard;///this is the only thing here that needs to be initialized
        this.diceGroup = this.GetComponent<DiceGroup>();
    }

    public void Disengage(DiceData stillData, DiceData travelingDice, Vector3Int holdPos, Vector3Int startPos, Vector3Int finishPos)
    {

        HoldDiceInPlace(holdPos, stillData);
        StartCoroutine(TravelingDice(travelingDice, startPos, finishPos));

        //diceFXController.FX(DiceFXController.TileEffect.disengageLeft, startPos);
        //diceFXController.FX(DiceFXController.TileEffect.disengageRight, startPos);


        //if the y of the diengaging dice is great than the y of the still one its right
        if (holdPos.x < startPos.x)
        {
            //Debug.Log("FX");

            diceFXController.FX(DiceFXController.TileEffect.disengageLeft, startPos);
        }
        else
        {
            //Debug.Log("FX");

            diceFXController.FX(DiceFXController.TileEffect.disengageRight, startPos);
        }
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
        //Debug.Log("FX");
        //diceFXController.FX(DiceFXController.TileEffect.slam, finish);
        this.diceBoard.SetSingleDiceOnBoard(finish, travelingDice.tile);
        DisengageDropFX(finish);
        //diceGroup.HandlePostDisengagement();
        yield return null;
    }

    async void DisengageDropFX(Vector3Int location)
    {
        List<Task> tasks = new List<Task>();
        tasks.Add(diceFXController.FXTask(DiceFXController.TileEffect.slam, location));
        await Task.WhenAll(tasks);
        diceGroup.HandlePostDisengagement();
    }

}

