using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public enum DiceNumber
{
    One = 1,
    Two,
    Three,
    Four,
    Five,
    Six

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

}

[System.Serializable]
public class DiceData
{
    public DiceNumber number;
    private DiceColor color;
    public Tile tile;//which dice tile we are using
    //public Vector2Int[] cellLocation{get;private set;}//the cell this dice inhbits
    public Vector2Int[] cellLocation = new Vector2Int[1];//the cell this dice inhbits
    //public Vector2Int[,] wallKicks{get;private set;}
    public void Initialize()
    {
        this.cellLocation[0] = new Vector2Int(0,0);//this could just in the fields
    }

}