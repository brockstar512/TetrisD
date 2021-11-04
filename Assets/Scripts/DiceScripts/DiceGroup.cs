using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DiceGroup : MonoBehaviour
{
    public DiceBoard diceBoard {get;private set;}//anytime the piece moves we need to pass that info to redraw that game piece
    public DiceData data {get;private set;}
    public DiceData dynamicData { get; private set; }
    
    public Vector3Int position {get; private set;}
    public Vector3Int[] cells {get; private set;}//we have an array of cells to help manipulate the shape when it moves/rotatets
    //one data is static the other dynamic
    //when you intialize one initialize the other



                            ///game board           spawn location      dice data currently active
    public void Initialize(DiceBoard diceBoard, Vector3Int position, DiceData data, DiceData dynamicData)
    {
        Debug.Log("Spawn location:  "+ position);
        this.diceBoard = diceBoard;
        this.position = position;
        this.data = data;
        this.dynamicData = dynamicData;
        Debug.Log("die 2:"+dynamicData.number);//
        Debug.Log("die 1:" + data.number);


        //if we have not initialized this array do it now
        if (this.cells == null)
        {
            //this.cells = new Vector3Int[data.cellLocation.Length];//single die
            this.cells = new Vector3Int[2];//changed it to have a cell for the second die
            //this.cells = new Vector3Int[1];//single die

        }
        /*
        //fill in the array with the current tile info in play
        for(int i = 0; i< data.cellLocation.Length;i++){
            this.cells[i] = (Vector3Int)data.cellLocation[i];
        }*/
        this.cells[0] = (Vector3Int)data.cellLocation[0];//(Vector3Int)
        this.cells[1] = new Vector3Int( data.cellLocation[0].x+1, data.cellLocation[0].y, 0);
        Debug.Log($"Dynamic cell is...");
        //this.cells[1] = (Vector3Int)dynamicData.cellLocation[0];


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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwapPlaces();
        }


        this.diceBoard.SetOnBoard(this);
    }

    void SwapPlaces()
    {
        //this.dynamicData.tile
        //this.data.tile
        Tile temp = this.dynamicData.tile;
        this.dynamicData.tile = this.data.tile;
        this.data.tile = temp;
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
