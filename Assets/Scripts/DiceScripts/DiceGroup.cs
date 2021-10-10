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
        Debug.Log("Spawn location:  "+ position);
        this.diceBoard = diceBoard;
        this.position = position;
        this.data = data;


        //if we have not initialized this array do it now
        if(this.cells == null)
        {
            //this.cells = new Vector3Int[data.cellLocation.Length];//single die
            this.cells = new Vector3Int[1];//single die

        }

        //fill in the array with the current tile info in play
        for(int i = 0; i< data.cellLocation.Length;i++){
            this.cells[i] = (Vector3Int)data.cellLocation[i];
        }


    }

    void Update()
    {
        //MoveController();
        this.diceBoard.Clear(this);
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(Vector2Int.left);
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(Vector2Int.right);
        }
        if(Input.GetKeyDown(KeyCode.DownArrow)){
            Move(Vector2Int.down);
        }
        
        this.diceBoard.SetOnBoard(this);
    }

    private bool Move(Vector2Int translation)
    {
        Vector3Int newPosition = this.position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = this.diceBoard.IsValidPosition(this, newPosition);
        Debug.Log("IS VALID?: " + valid);
        // Only save the movement if the new position is valid
        if (valid)
        {
            this.position = newPosition;
            this.position = newPosition;
        }

        return valid;
    }
}
