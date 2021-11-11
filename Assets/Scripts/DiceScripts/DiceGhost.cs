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
    public Vector3Int[] pos { get; private set; }


    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.cells = new Vector3Int[2];
        this.pos = new Vector3Int[2];
    }

    /// <summary>
    /// after all the update/inputs are handled for that frame then run this update.
    /// ... clear the board. copy where the piece is... send it to the bottom... set the ghost tiles on the board
    /// </summary>
    private void LateUpdate()
    {
        Clearing();
        Copy();
        //Drop();
        pos[0]= Dropping(this.trackedDiceGroup.cells[0] + this.trackedDiceGroup.position);
        pos[1]= Dropping(this.trackedDiceGroup.cells[1] + this.trackedDiceGroup.position);
        Setting();

    }

    // -- the following was working but the ghost pieces stick together on the horizontal line like how tetris normally works, I want each piece of dice to fall to the next avaiable tile //
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
    /// puts the ghost piece group on the dice board
    /// </summary>
    void Set()
    {
        this.tilemap.SetTile(this.cells[1] + this.position, this.tile);
        this.tilemap.SetTile(this.cells[0] + this.position, this.tile);
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
        //put the main dice back on the board
        this.diceBoard.SetOnBoard(this.trackedDiceGroup);
    }


    // -- the following sperates the ghost pieces so they aren't tied to the same row -- //

    /// <summary>
    /// This function clears where the previous position of both ghost pieces were.
    /// </summary>
    private void Clearing()
    {
        this.tilemap.SetTile(this.pos[0], null);
        this.tilemap.SetTile(this.pos[1], null);

    }

    /// <summary>
    /// This function takes in a single dice location then iterates down the coloumn to see what is the last available tile.
    /// in order to find the last avaiable tile it quickly removes the tracked piece then puts the piece back on
    /// </summary>
    /// <param name="singleDice cell location + the position of the parent class"></param>
    private Vector3Int Dropping(Vector3Int singleDice)
    {
        //Debug.Log("POS" + singleDice);
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
        return position;
    }

    /// <summary>
    /// This function could change to be more elegant but I was over shooting a row by 1, so for every tile I raise it by one.
    /// when the tiles were ontop of eachother they inhabited the same tile. They see if the spot is avaiable on the other board but they dont check to see if its avaible for this board
    /// Which is why they double up, so if they were vertical they would only take up one tile, so if the x is the same add one more in the y to raise it one more.
    /// </summary>
    void Setting()
    {
        //TODO maybe at the end I can figure out a way where things should work exactly how they're written so I don't have to keep this hard coded check
        //
        //The tiles are one lower than where they should be so bump them up to the next line
        this.pos[0].y += 1;
        this.pos[1].y += 1;
        //when the tiles are ontop of eachother they inhabit the same tile. the check only check on if the other tile map has a tile not this one, which is why they double up.
        //they see its avaiable on the other board but they dont check to see if its avaible for this board... could consider this.tilemap.HasTile(tilePosition)
        if (this.pos[1].x == this.pos[0].x)
        {
            this.pos[1].y += 1;
        }
        //set both tiles on their respective positions
        this.tilemap.SetTile(this.pos[0], this.tile);
        this.tilemap.SetTile(this.pos[1], this.tile);

    }

}

