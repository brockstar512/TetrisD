using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DiceMatch : MonoBehaviour
{
    [SerializeField]
    Tilemap mainMap;
    [SerializeField]
    Grid grid;

    private enum YGridCell
    {
        One_Top =3,
        Two = 2,
        Three = 1,
        Four =0,
        Five = -1,
        Six = -2,
        Seven = -3,
        Eight = -4,
        Nine_Bottom = -5,
    }
    private enum XGridCell
    {
        One_Left = -3,
        Two = -2,
        Three = -1,
        Four = 0,
        Five = 1,
        Six_Right = 2,
    }

    Dictionary<Vector3Int, DiceImprint> TilePos;

    //used in the tutorial
    private List<DiceImprint> tileDatas;
    Dictionary<TileBase, DiceImprint> dataFromTile;



    // Start is called before the first frame update
    void Awake()
    {
        //dataFromTile = new Dictionary<TileBase, DiceImprint>();
        ////go through the list of tiles then ad them to the dictionary
        //foreach (var tileData in tileDatas)
        //{
        //    foreach (var tile in tileData.tiles)
        //    {

        //        dataFromTile.Add(tile, tileData);
        //    }
        //}



        TilePos = new Dictionary<Vector3Int, DiceImprint>();
        //might not need to preinitialize dictionary with value... especially if i just check with
        //bool hasTile = mainMap.HasTile(position);


        for (int y = (int)YGridCell.Nine_Bottom; y <= (int)YGridCell.One_Top; y++)
        {
            for (int x = (int)XGridCell.One_Left; x <= (int)XGridCell.Six_Right; x++)
            {
                Vector3Int tile = new Vector3Int(x,y,0);
                DiceImprint emptyTile = new DiceImprint(tile);
                TilePos.Add(tile,emptyTile);
                //Debug.Log($"KEYS:{tile}");//KEYS:(2, 3, 0)


            }
        }

    }

    //maybe this tile should only have a key if there is a dice in it
    public void SetTileDict(Vector3Int Pos, DiceImprint die)
    {
        //if(TilePos.TryGetValue(Pos, out die)) return;//if there is a value at that key
        //TilePos.Add(Pos, die);
        TilePos[Pos] = die;
    }

    public void UpdateDict(Vector3Int Pos, DiceImprint die)
    {
        

        TilePos[Pos] = die;
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetMouseButtonDown(0))
        {
            MouseTileReader();
        }

    }

    private void MouseTileReader()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
        Vector3Int position = grid.WorldToCell(worldPoint);
        TileBase clickedTile = mainMap.GetTile(position);
        CheckForMatches();
        return;
        //
        Debug.Log($"At position {position} there is a {clickedTile} here it is in the dict {TilePos[position].number}");



        return;
        //RemoveTileDict(position);
        if (TilePos.ContainsKey(position))
        {
            Debug.Log($"At position {position} there is a {clickedTile} it should now be removed in the dict {TilePos[position].number}");
        }


    }

    void CheckForMatches()
    {
        //mainMap
        //check the rows first (this is just the lowest row)

        List<Vector3Int> MatchedDice = new List<Vector3Int>();

        for (int y = (int)YGridCell.Nine_Bottom; y <= (int)YGridCell.One_Top; y++)
        {
            //Debug.Log($"Checking row {y} and in the enum is {(YGridCell)y}");
            //Debug.Log(IsThereAHorizontalMatch(position));
            for (int x = (int)XGridCell.One_Left; x<= (int)XGridCell.Six_Right; x++)
            {

                Vector3Int position = new Vector3Int(x, y, 0);
                bool hasTile = mainMap.HasTile(position);
                //Debug.Log($"Checking Column {x} and in the enum is {(XGridCell)x} and is there a tile? {hasTile} here is the value of the dice {TilePos[position].number}");
                //Debug.Log(IsThereAHorizontalMatch(position));

                //if there isnt a tile break
                if (!hasTile)
                    break;

                //-3 -2 -1 0 1 2

                //3
                //2
                //1
                //0
                //-1
                //-2
                //-3
                //-4
                //-5

                //checking if its worth seeing if there is a pair by checking the bounds
                bool withinHorizontalBounds = (position.x + (int)TilePos[position].number) < 2 ? true : false;//TODO maybe dont hard code this
                bool withinVerticalBounds = (position.y + (int)TilePos[position].number) < 3 ? true : false;//TODO maybe dont hard code this
                //getting the current number we are expecting
                DiceNumber number = TilePos[position].number;

                //if it is within our bounds
                if (withinHorizontalBounds)
                {
                    //checking if our dice in the same row has a match with the number of dice that dice number is inbetween them
                    Vector3Int horizontalPosCheck = new Vector3Int(position.x + (int)number + 1, position.y, 0);

                    //if the numbers are the same check if they are connected
                    bool hasHorizontalMatch = TilePos[position].number == TilePos[horizontalPosCheck].number;

                    if (hasHorizontalMatch && IsConnected(position, horizontalPosCheck, withinHorizontalBounds))//&& we do the bounds check
                    {
                        Debug.Log($"<color=green>Match</color> pos {position} comparing with other position {horizontalPosCheck} the two numbers are {TilePos[position].number} and {TilePos[horizontalPosCheck].number}");



                    }
                }
                if (withinVerticalBounds) {
                    Vector3Int verticalPosCheck = new Vector3Int(position.x, position.y + (int)number + 1, 0);

                    bool hasVerticalMatch = TilePos[position].number == TilePos[verticalPosCheck].number;

                    if (hasVerticalMatch && IsConnected(position, verticalPosCheck, withinVerticalBounds))
                    {
                        Debug.Log($"<color=green>Match</color> pos {position} comparing with other position {verticalPosCheck} the two numbers are {TilePos[position].number} and {TilePos[verticalPosCheck].number}");

                        //Debug.Log(IsThereAVerticalMatch(position));

                    }
                }

            }
        }
    }

    void CheckEveryTile()
    {

    }

    //bool IsThereAHorizontalMatch(Vector3Int position)
    //{
    //    DiceNumber number = TilePos[position].number;
    //    Vector3Int posCheck = new Vector3Int(position.x + (int)number, position.y, 0);
    //    //Debug.Log($"New Pos: {posCheck}");
    //    return TilePos[position].number == TilePos[posCheck].number;
    //}

    //bool IsThereAVerticalMatch(Vector3Int position)
    //{
    //    DiceNumber number = TilePos[position].number;
    //    Vector3Int posCheck = new Vector3Int(position.x, position.y + (int)number, 0);
    //    //Debug.Log($"New Pos: {posCheck}");
    //    return TilePos[position].number == TilePos[posCheck].number;
    //}
    //List<Vector3Int>
    bool IsConnected(Vector3Int position, Vector3Int checkingPosition, bool inBounds)
    {
        if (!inBounds)
            return false;
        
        
        List<Vector3Int> inBetweenDice = new List<Vector3Int>();
        inBetweenDice.Add(position);
        inBetweenDice.Add(checkingPosition);

        Vector3Int directionalCheck = position.x == checkingPosition.x ? new Vector3Int(0,-1,0)  : new Vector3Int(-1, 0, 0);
        while(position != checkingPosition)
        {
            checkingPosition += directionalCheck;
            if (!mainMap.HasTile(checkingPosition))
                return false;

            inBetweenDice.Add(checkingPosition);
        }
        
        return true;
    }

    void ApplyGravity()
    {
        //start from the bottom
        //iterate up the column until you have a tile
        //go down until you do have a tile
        //repeat starting from your current location

        //go to next column
    }
  
}
