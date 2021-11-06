using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DiceGroup : MonoBehaviour
{

    //one data is static the other dynamic
    //when you intialize one initialize the other

    public DiceBoard diceBoard {get;private set;}//anytime the piece moves we need to pass that info to redraw that game piece
    public DiceData data {get;private set;}
    public DiceData dynamicData { get; private set; }
    
    public Vector3Int position {get; private set;}
    public Vector3Int[] cells {get; private set;}//we have an array of cells to help manipulate the shape when it moves/rotatets
 



    DynamicDiceState dynamicDiceState = DynamicDiceState.Right;

    Dictionary<DynamicDiceState, Vector3Int> RotationDict =
    new Dictionary<DynamicDiceState, Vector3Int>()
    {
	    { DynamicDiceState.Right,new Vector3Int(1, 0, 0)},
        { DynamicDiceState.Down,new Vector3Int(0,  -1, 0)},
        { DynamicDiceState.Left,new Vector3Int(- 1, 0, 0)},
        { DynamicDiceState.Up,new Vector3Int(0, 1, 0)}
    };




    ///game board           spawn location      dice data currently active
    public void Initialize(DiceBoard diceBoard, Vector3Int position, DiceData data, DiceData dynamicData)
    {
        Debug.Log("Spawn location:  "+ position);
        this.diceBoard = diceBoard;
        this.position = position;
        this.data = data;
        this.dynamicData = dynamicData;
        //Debug.Log("die 2:"+dynamicData.number);//
        //Debug.Log("die 1:" + data.number);


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
        //To the right -> 
        this.cells[1] = new Vector3Int( data.cellLocation[0].x+1, data.cellLocation[0].y, 0); //this is dynamic dice position//TODO This is very imortant



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

        //TODO Testing functions
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //test function
        }


        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            Rotate(DynamicDiceState.Right);
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Rotate(DynamicDiceState.Left);
            
        }

        
        this.diceBoard.SetOnBoard(this);
    }

    /// <summary>
    /// Swaps the dice data information in the tiles
    /// </summary>
    void SwapTilesInGroup()
    {
        //swap number as well
        DiceData temp = this.dynamicData;
        this.dynamicData = this.data;
        this.data = temp;
    }

    /// <summary>
    /// Rotate takes in either a right or left parameter. If it's right, it will check what state the dynamic die
    /// currently is in and will determine what the position is to the right or left based off of that.
    /// example ...if we pass in right and our current dice is south of our static die then is will determine that left is the targeted tile.
    /// Then we check if the direction is valid. If it is we apply the rotation. if not we swap tiles and reverse the direction.
    /// </summary>
    /// <param name="intendedDirection"></param>
    void Rotate(DynamicDiceState intendedDirection)
    {
        switch (intendedDirection)
        {
            case DynamicDiceState.Right:
                switch (dynamicDiceState)
            {       
                case DynamicDiceState.Right:
                    intendedDirection = DynamicDiceState.Down;
                    break;
                case DynamicDiceState.Left:
                    intendedDirection = DynamicDiceState.Up;
                    break;
                case DynamicDiceState.Down:
                    intendedDirection = DynamicDiceState.Left;
                    break;
                case DynamicDiceState.Up:
                    intendedDirection = DynamicDiceState.Right;
                break;
            }
                break;
            case DynamicDiceState.Left:
                switch (dynamicDiceState)
                {
                    case DynamicDiceState.Right:
                        intendedDirection = DynamicDiceState.Up;
                        break;
                    case DynamicDiceState.Left:
                        intendedDirection = DynamicDiceState.Down;
                        break;
                    case DynamicDiceState.Down:
                        intendedDirection = DynamicDiceState.Right;
                        break;
                    case DynamicDiceState.Up:
                        intendedDirection = DynamicDiceState.Left;
                        break;
                }
                break;
        }
        
        if (this.diceBoard.IsValidPosition(this, this.position + RotationDict[intendedDirection]))
            ApplyRotation(intendedDirection);
        else
            ReverseRotationAndSwap(intendedDirection);
    }

    /// <summary>
    /// Apply rotation just applies the rotation from a dictionary based off of the argument passed in and updates the dynamic dice position
    /// </summary>
    /// <param name="rotation"></param>
    void ApplyRotation(DynamicDiceState rotation)
    {
        Vector3Int newPosition;
        switch(rotation)
        {
            case DynamicDiceState.Right:
                dynamicDiceState = DynamicDiceState.Right;
                newPosition = RotationDict[DynamicDiceState.Right];
                break;
            case DynamicDiceState.Up:
                dynamicDiceState = DynamicDiceState.Up;
                newPosition = RotationDict[DynamicDiceState.Up];
                break;
            case DynamicDiceState.Left:
                dynamicDiceState = DynamicDiceState.Left;
                newPosition = RotationDict[DynamicDiceState.Left];
                break;
            case DynamicDiceState.Down:
                dynamicDiceState = DynamicDiceState.Down;
                newPosition = RotationDict[DynamicDiceState.Down];
                break;
            default:
                newPosition = new Vector3Int(0, 0, 0);
                    break;

        }
        this.cells[1] = new Vector3Int(newPosition.x, newPosition.y, 0); ;
    }

    /// <summary>
    /// This function reverses the direction you intented to go and swaps the tile information. This is called when the tile you are trying to rotate to
    /// is out of bounds
    /// </summary>
    /// <param name="rotation"></param>
    void ReverseRotationAndSwap(DynamicDiceState rotation)
    {
        switch(rotation)
        {
            case DynamicDiceState.Up:
                ApplyRotation(DynamicDiceState.Down);
                break;
            case DynamicDiceState.Down:
                ApplyRotation(DynamicDiceState.Up);
                break;
            case DynamicDiceState.Right:
                ApplyRotation(DynamicDiceState.Left);
                break;
            case DynamicDiceState.Left:
                ApplyRotation(DynamicDiceState.Right);
                break;

        }
        SwapTilesInGroup();
    }




    private bool Move(Vector2Int translation)
    {
        Vector3Int newPosition = this.position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = this.diceBoard.IsValidPosition(this, newPosition);
        
        // Only save the movement if the new position is valid
        if (valid)
        {
            this.position = newPosition;
            this.position = newPosition;
        }

        return valid;
    }
}

enum DynamicDiceState
{
    Left = -1,
    Up,
    Right,
    Down
}
//if roation is valid rotate it// otherwise send the inverse and swap
//up -> data.cellLocation[0].y +1
//right ->data.cellLocation[0].x + 1
//down -> data.cellLocation[0].y -1
//left -> data.cellLocation[0].x - 1
