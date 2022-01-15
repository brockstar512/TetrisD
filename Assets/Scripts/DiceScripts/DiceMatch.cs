using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DiceMatch : MonoBehaviour
{
    [SerializeField]
    Tilemap mainMap;
    public DiceBoard diceBoard;
    
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
    const float GRAVITY = 0.045f;//0.045f;

    //todo should I have the bools do checks in there function then return a list of dice
    #region testing regions
    #endregion


    private void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        TilePos = new Dictionary<Vector3Int, DiceImprint>();

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
    public void RemoveTileDict(Vector3Int Pos)
    {
        //if(TilePos.TryGetValue(Pos, out die)) return;//if there is a value at that key
        //TilePos.Add(Pos, die);
        TilePos[Pos] = new DiceImprint(Pos);
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


        //RemoveDiceClicked(position);
        //CheckForMatches();
        return;
        ////
        //Debug.Log($"At position {position} there is a {clickedTile} here it is in the dict {TilePos[position].number}");



        //return;
        ////RemoveTileDict(position);
        //if (TilePos.ContainsKey(position))
        //{
        //    Debug.Log($"At position {position} there is a {clickedTile} it should now be removed in the dict {TilePos[position].number}");
        //}


    }

    [ContextMenu("Read Values")]
    void ReadValues()
    {
        foreach(KeyValuePair<Vector3Int, DiceImprint > entry in TilePos)
        {
            if((int)entry.Value.number > 0)
            {
                Debug.Log($"At Position {entry.Key} there is a value of {entry.Value.number} and color {entry.Value.color}");
            }
            
        }
    }
    //void RemoveDiceClicked(Vector3Int position)
    //{
    //    diceBoard.Clear(position);
    //    TilePos[position] = new DiceImprint(position);
    //    //ApplyGravity();
    //}

    public void Score()
    {
        CheckForMatches();
    }

    void CheckForMatches()
    {

        //-3 -2 -1 0 1 2

        //3
        //2
        //1
        //0
        //-1
        //-2 (4th)
        //-3 (3rd)
        //-4 (2nd)
        //-5 (1st) (then this column)

        Dictionary<Vector3Int, int> MatchedDice = new Dictionary<Vector3Int, int>();


        for (int y = (int)YGridCell.Nine_Bottom; y <= (int)YGridCell.One_Top; y++)
        {

            for (int x = (int)XGridCell.One_Left; x<= (int)XGridCell.Six_Right; x++)
            {
                //what position are we checking
                Vector3Int position = new Vector3Int(x, y, 0);
                //does it have a tile?
                bool hasTile = mainMap.HasTile(position);


                //if there isnt a tile go to the next one
                if (!hasTile)
                    continue;

                Debug.Log($"We are checking {position}");

                //Do they have a pair in bounds
                bool withinHorizontalBounds = (position.x + (int)TilePos[position].number) < 2 ? true : false;
                bool withinVerticalBounds = (position.y + (int)TilePos[position].number) < 3 ? true : false;

                //get the number of the current position
                DiceNumber number = TilePos[position].number;

                Debug.Log($"We are checking dice number {number}");//when dice diengage theyre value isn't kept in the pos dict
                if (number == DiceNumber.Zero)
                    continue;

                //get a list of the dice inbetween in case they are the same color
                List<Vector3Int> BetweenDice = new List<Vector3Int>();

                //if it is within our bounds
                if (withinHorizontalBounds)
                {
                    //get the dice x number away
                    Vector3Int horizontalPosCheck = new Vector3Int(position.x + (int)number + 1, position.y, 0);

                    //if the numbers are the same?
                    bool hasHorizontalMatch = TilePos[position].number == TilePos[horizontalPosCheck].number;

                    //if the colors are the same?
                    bool hasHorizontalColorMatch = TilePos[position].color == TilePos[horizontalPosCheck].color;
                    //TODO I stopped here
                    if (hasHorizontalMatch && IsConnected(position, horizontalPosCheck, withinHorizontalBounds, BetweenDice))//&& we do the bounds check
                    {
                        BetweenDice.Add(position);
                        BetweenDice.Add(horizontalPosCheck);
                        //Debug.Log($"<color=green>Match</color> pos {position} comparing with other position {horizontalPosCheck} the two numbers are {TilePos[position].number} and {TilePos[horizontalPosCheck].number}");

                        foreach (Vector3Int item in BetweenDice)
                        {
                            //Debug.Log(MatchedDice.ContainsKey(item));
                            MatchedDice[item] = MatchedDice.ContainsKey(item) ? MatchedDice[item] + 1 : 1;
                            //MatchedDice.ContainsKey
                        }

                    }
                }
                if (withinVerticalBounds) {
                    Vector3Int verticalPosCheck = new Vector3Int(position.x, position.y + (int)number + 1, 0);

                    bool hasVerticalMatch = TilePos[position].number == TilePos[verticalPosCheck].number;
                    bool hashasVerticalColorMatch = TilePos[position].color == TilePos[verticalPosCheck].color;

                    if (hasVerticalMatch && IsConnected(position, verticalPosCheck, withinVerticalBounds, BetweenDice))
                    {
                        BetweenDice.Add(position);
                        BetweenDice.Add(verticalPosCheck);
                        //Debug.Log($"<color=green>Match</color> pos {position} comparing with other position {verticalPosCheck} the two numbers are {TilePos[position].number} and {TilePos[verticalPosCheck].number}");
                        foreach (Vector3Int item in BetweenDice)
                        {
                            Debug.Log(MatchedDice.ContainsKey(item));
                            MatchedDice[item] = MatchedDice.ContainsKey(item) ? MatchedDice[item] + 1 : 1;
                                //MatchedDice.ContainsKey
                            
                        }
                        //if theyre the same color take all of them.
                        //if they arent just take the beginning and end

                    }
                }

            }
        }
        foreach(KeyValuePair<Vector3Int, int>  pair in MatchedDice)
        {
                                                           //location to delete    //level of animation                 //tile number
            Debug.Log($"<color=red>Matches In Dictionary </color>: {pair.Key} -> {pair.Value} and here is the number {(int)TilePos[pair.Key].number}");
        }
        //Debug.Break();
        RemoveTiles(MatchedDice);
    }

    void RemoveTiles(Dictionary<Vector3Int, int> MatchedDice)
    {
        
        foreach (KeyValuePair<Vector3Int, int> pair in MatchedDice) {
            //TODO send the int to the animator for the special effects
            //-the key is the tile position to remove
            //-the value is how many chains is that tile included in

            //delete this position from the board
            diceBoard.Clear(pair.Key);

            //reset position to 0
            TilePos[pair.Key] = new DiceImprint(pair.Key);
        }
        //diceBoard.SpawnGroup();
        ApplyGravity();
    }


    bool IsConnected(Vector3Int position, Vector3Int checkingPosition, bool inBounds, List<Vector3Int>listOfDiceToRemove)
    {
        //if what we're checking is out of bounds just ditch it.
        listOfDiceToRemove.Clear();
        if (!inBounds)
            return false;


        //get a reference if they are the same color
        bool sameColor = TilePos[position].color == TilePos[checkingPosition].color;

        //figure out if we are going in the x or y
        Vector3Int directionalCheck = position.x == checkingPosition.x ? new Vector3Int(0,-1,0)  : new Vector3Int(-1, 0, 0);

        //make sure we're not checking the final position
        checkingPosition += directionalCheck;

        //while start position does not equal checking position
        while (position != checkingPosition)
        {
            //if what you're checking is not connected ditch it and break
            if (!mainMap.HasTile(checkingPosition))
            {
                listOfDiceToRemove.Clear();
                return false;
            }

            //if the bookended dice are the same color gather the whole list
            if (sameColor)
                listOfDiceToRemove.Add(checkingPosition);

            //move to the next position to check
            checkingPosition += directionalCheck;
        }

        //this check is connected
        return true;
    }

    //todo this needs tlc
    [ContextMenu("Apply gravity")]
    void ApplyGravity()
    {

        //start from the bottom
        for (int x =(int)XGridCell.One_Left; x<= (int)XGridCell.Six_Right; x++)
        {
            int MovingDiceInRow = -1;
            //Debug.LogError(position);
            for (int y = (int)YGridCell.Nine_Bottom; y <= (int)YGridCell.One_Top; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                //Debug.LogError(position);// iterating correctly
                //Vector3Int below = new Vector3Int(position.x, -1+ position.y, 0);




                //TODO also if it has a dice below it continue
                //if (mainMap.HasTile(below))
                //    continue;

                //we can skip the tiles that are empty
                if (!mainMap.HasTile(position))//(y doesnt have a tile)
                    continue;

                Vector3Int startPos = position;
                Vector3Int finishPos= new Vector3Int();
                //we found a tile.. if there is another tile under it or we are at the bottom don't run
                //we are falling until we have a tile or we are at the bottom
                //falling is the position of the current dice

                //Tile currentTile = TilePos[position].tile;
                DiceImprint currentDice = new DiceImprint(TilePos[position], position);

                //Vector3Int? newPosition = new Vector3Int?();//this might ntot have to be null because if it past here it has a tile
                for (int falling = position.y;(mainMap.HasTile(new Vector3Int(x,falling-1,0)) || (falling >= (int)YGridCell.Nine_Bottom)); falling--)
                {

                    Vector3Int newPosition = new Vector3Int(x, falling, 0);
                    //Debug.Log(newPosition);
                    finishPos = newPosition;


                }
                //get final position
                MovingDiceInRow++;
                finishPos = new Vector3Int(finishPos.x, finishPos.y + MovingDiceInRow,0);
                //Debug.LogError($"Start {startPos} and finsh {finishPos}");
                if (mainMap.HasTile(finishPos))
                    continue;

                StartCoroutine(TravelingDice(currentDice, startPos, finishPos));

                //Debug.Log($"--------------------------");



            }

            diceBoard.SpawnGroup();
        }
        //iterate up the column until you have a tile
        //go down until you do have a tile
        //repeat starting from your current location

        //go to next column

    }



    IEnumerator TravelingDice(DiceImprint die, Vector3Int start, Vector3Int finish)
    {

        TilePos[start] = new DiceImprint(start);

        Vector3Int current = start;

        while (current != finish)
        {
            this.diceBoard.Clear(current);
            current = new Vector3Int(current.x, current.y + -1, 0);
            this.diceBoard.SetSingleDiceOnBoard(current, die.tile);
            yield return new WaitForSeconds(GRAVITY);
        }
        this.diceBoard.SetSingleDiceOnBoard(finish, die.tile);

        TilePos[finish] = new DiceImprint(die,finish);
        yield return null;
    }

    //IEnumerator TravelingDice(Tile tile, Vector3Int start, Vector3Int finish)
    //{

    //    TilePos[start] = new DiceImprint(start);

    //    Vector3Int current = start;

    //    while (current != finish)
    //    {
    //        this.diceBoard.Clear(current);
    //        current = new Vector3Int(current.x, current.y + -1, 0);
    //        this.diceBoard.SetSingleDiceOnBoard(current, tile);
    //        yield return new WaitForSeconds(GRAVITY);
    //    }
    //    this.diceBoard.SetSingleDiceOnBoard(finish, tile);
    //    DiceData die = new DiceData();
    //    die.number = DiceNumber.Six;
    //    die.tile = tile;
    //    TilePos[finish] = new DiceImprint(die, finish);
    //    yield return null;
    //}

}
//todo
//small chain - dictionary of dice that count
//big chain - waterfall for those that count
//todo create states