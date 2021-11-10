using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DiceGhost : MonoBehaviour
{
    public Tile tile;
    public DiceBoard diceBoard;
    public DiceGroup trackedDiceGroup;//piece we are simulating

    public Tilemap tilemap { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }

    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.cells = new Vector3Int[2];
    }

    /// <summary>
    /// after all the update/inputs are handled for that frame then run this update.
    /// ... clear the board. copy where the piece is... send it to the bottom... set the ghost tiles on the board
    /// </summary>
    private void LateUpdate()
    {
        //Clear();
        //Copy();
        //Drop();
        Drop(this.trackedDiceGroup.cells[0]+ this.trackedDiceGroup.position);
        Drop(this.trackedDiceGroup.cells[1]+ this.trackedDiceGroup.position);
        //Set();

    }
    /// <summary>
    /// removes the current ghost piece from the board to make room for the piece that we are dropping
    /// </summary>
    private void Clear()
    {
        this.tilemap.SetTile(this.cells[1] + this.position, null);
        this.tilemap.SetTile(this.cells[0] + this.position, null);
    }
    /// <summary>
    /// this copies the group that is in play and we are storing it into the ghost cells array.
    /// </summary>
    private void Copy()
    {
        this.cells[0] = this.trackedDiceGroup.cells[0];
        this.cells[1] = this.trackedDiceGroup.cells[1];
    }

    /// <summary>
    /// this places places the ghost piece all the way to the bottom of the board that is avalable.
    /// we are iterating from the bottom up. ithe first available space for both blocks we are going to put the position there then set on board.
    /// (if we're going from the top to the bottom its going to stop when it hits our piece so we take off our piece then put it back after we find the lost available position)
    /// Hence -> this.diceBoard.Clear(this.trackedDiceGroup); and -> this.diceBoard.SetOnBoard(this.trackedDiceGroup);
    /// </summary>
    private void Drop()
    {
        //Spawn location:  (-1, 3, 0)
        Vector3Int position = this.trackedDiceGroup.position;//where is your tracked piece currently

        int current = position.y;//we are starting where the piece currently is in the y direction
        int bottom = -this.diceBoard.boardSize.y / 2 - 1;//where are we going to end the iteration... at the bottom

        //take our main piece off the board
        this.diceBoard.Clear(this.trackedDiceGroup);//(if we're going from the top to the bottom its going to stop
        //when it hits our piece so we take off our piece then put it back after we find the lost available position)
        //if we dont clear before the poisition its actuall y suppose to be will return false because that very
        //piece is already occupying that space

        //this is stopping at the latest value in that coulumn
        for (int column = current; column >= bottom; column--)
        {
            //Debug.Log($"POS: {position}");
            //position: //x-> [ -3   2]
            //position: //y-> [  3  -6]

            position.y = column;
            //we are hypothesitcally saying if this was our position can we put the dice group here... until the code tells us no. when it does say no, we are
            //getting places this ghosts piece at that location then puttin the tracked dice back on the board
            //Debug.Log($"Here is the position that we are looking at {position.y}");//3 -> -6 after last tile is filled it iterates to -5
            if (this.diceBoard.IsValidPosition(this.trackedDiceGroup, position))
            {
                //IsValidPositionSingleTile
                this.position = position;
            }
            else { break; }//if we are breaking we get the last position where it was valid and placing that on the board
        }

        this.diceBoard.SetOnBoard(this.trackedDiceGroup);
    }

    
    //do what im doing in the other drop except for this individula dice
    //i need to fo it onr at a time
    private void Drop(Vector3Int singleDice)
    {
        Debug.Log("POS"+singleDice);
        Vector3Int position = singleDice;

        //where our active piece is
        int current = position.y;
        //our end point
        int bottom = -this.diceBoard.boardSize.y / 2 - 1;
        //clear board
        this.diceBoard.Clear(this.trackedDiceGroup);

        //we are starting from active dice then looking down the column for the next available spot
        for (int column = current; column >= bottom; column--)
        {
            //position: //x-> [ -3   2]
            //position: //y-> [  3  -6]

            
            //this position equals column we are checking
            position.y = column;
            

            if (this.diceBoard.IsValidPositionSingleDice(position))
            {
                this.position = position;
            }
            else { break; }
        }

        Debug.Log($"I would put a tile here at {position}");
        this.diceBoard.SetOnBoard(this.trackedDiceGroup);


    }

    /// <summary>
    /// puts the ghost piece tile on the dice board
    /// </summary>
    void Set()
    {
        this.tilemap.SetTile(this.cells[1] + this.position, this.tile);
        this.tilemap.SetTile(this.cells[0] + this.position, this.tile);
    }

    //get reference to the top.
    //find whih row we are in.
    //climb down until you find the next avail tile
}

