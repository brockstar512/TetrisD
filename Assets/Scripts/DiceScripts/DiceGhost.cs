using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DiceGhost : MonoBehaviour
{
    public Tile tile;
    public DiceBoard diceBoard;
    public DiceGroup trackedDiceGroup;

    public Tilemap tilemap { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }

    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.cells = new Vector3Int[2];
    }

    private void LateUpdate()
    {
        Clear();
        Copy();
        Drop();
        Set();

    }

    private void Clear()
    {
        this.tilemap.SetTile(this.cells[1] + this.position, null);
        this.tilemap.SetTile(this.cells[0] + this.position, null);
    }

    private void Copy()
    {
        this.cells[0] = this.trackedDiceGroup.cells[0];
        this.cells[1] = this.trackedDiceGroup.cells[1];
    }

    //todo dice could should go to the next available tile instead of sticking to its row
    private void Drop()
    {
        Vector3Int position = this.trackedDiceGroup.position;

        int current = position.y;
        int bottom = -this.diceBoard.boardSize.y / 2 -1;

        this.diceBoard.Clear(this.trackedDiceGroup);//if we dont clear before the poisition its actuall y suppose to be will return false because that very
        //piece is already occupying that space

        //this is stopping at the latest value in that row
        for(int row = current; row >= bottom; row--)
        {
            position.y = row;
            if(this.diceBoard.IsValidPosition(this.trackedDiceGroup, position))
            {
                this.position = position;
            }
            else { break; }
        }
        this.diceBoard.SetOnBoard(this.trackedDiceGroup);
    }

    void Set()
    {
        this.tilemap.SetTile(this.cells[1] + this.position, this.tile);
        this.tilemap.SetTile(this.cells[0] + this.position, this.tile);
    }

}

