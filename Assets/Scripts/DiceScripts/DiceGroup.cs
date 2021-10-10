using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceGroup : MonoBehaviour
{
    public DiceBoard diceBoard {get;private set;}//anytime the piece moves we need to pass that info to redraw that game piece
    public DiceData data {get;private set;}
    public Vector3Int position {get; private set;}
    public Vector3Int[] cells {get; private set;}//we have an array of cells to help manipulate the shape when it moves/rotatets
    



                            ///game board           spawn location      dice data currently active
    public void Initialize(DiceBoard diceBoard, Vector3Int position, DiceData data)
    {
        this.diceBoard = diceBoard;
        this.position = position;
        this.data = data;


        //if we have initialized this array do it now
        if(this.cells == null){
            //this.cells = new Vector3Int[data.cellLocation.Length];//single die
                        this.cells = new Vector3Int[1];//single die

        }

        //fill in the array with the current tile info in play
        for(int i = 0; i< data.cellLocation.Length;i++){
            this.cells[i] = (Vector3Int)data.cellLocation[i];
        }


    }
}
