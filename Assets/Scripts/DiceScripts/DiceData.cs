using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public enum DiceNumber
{
    Zero,
    One = 1,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven

}
public enum DiceColor
{
    White,
    Blue,
    Purple,
    Green,
    Red,
    Yellow,
    Orange,
    Null

}

[System.Serializable]
public class DiceData
{
    public DiceNumber number;
    public DiceColor color;
    public Tile tile;//which dice tile we are using
    //public Vector2Int[] cellLocation{get;private set;}//the cell this dice inhbits
    public Vector2Int[] cellLocation = new Vector2Int[1];//the cell this dice inhbits
    //public Vector2Int[,] wallKicks{get;private set;}
    public void Initialize()
    {
        this.cellLocation[0] = new Vector2Int(0,0);//this could just in the fields
        color = DiceColor.White;
    }

}

public struct DiceImprint
{
    public DiceNumber number;
    public DiceColor color;
    public Tile tile;
    //public TileBase tileBase;
    public Vector2Int position;

    public DiceImprint(DiceData die, Vector3Int boardPos)
    {
        this.number = die.number;
        this.color = die.color;
        this.tile = die.tile;
        this.position = new Vector2Int(boardPos.x, boardPos.y);
    }

    public DiceImprint(DiceImprint die, Vector3Int boardPos)
    {
        this.number = die.number;
        this.color = die.color;
        this.tile = die.tile;
        this.position = new Vector2Int(boardPos.x, boardPos.y);
    }

    public DiceImprint(Vector3Int boardPos)
    {
        this.number = DiceNumber.Zero;
        this.color = DiceColor.Null;
        this.tile = null;
        this.position = new Vector2Int(boardPos.x, boardPos.y);
    }


}
public class TileData: ScriptableObject
{
    public DiceNumber number;
    public DiceColor color;
    public Tile tile;
    public Vector2Int position;

}
//imprint tiles
//check in all directions if there is a match
//destroy tiles by color and number
//apply gravity to the tiles that get deleted
//adjust tile data
//chain tiles that are a match when gravity is applied