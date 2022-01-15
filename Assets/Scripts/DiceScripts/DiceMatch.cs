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
    void RemoveDiceClicked(Vector3Int position)
    {
        diceBoard.Clear(position);
        TilePos[position] = new DiceImprint(position);
        //ApplyGravity();
    }

    void CheckForMatches()
    {
        //mainMap
        //check the rows first (this is just the lowest row)
        //


        //
        Dictionary<Vector3Int, int> MatchedDice = new Dictionary<Vector3Int, int>();


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
                Debug.LogError($"We are creating a new between dice");
                List<Vector3Int> BetweenDice = new List<Vector3Int>();
                //if it is within our bounds
                if (withinHorizontalBounds)
                {
                    //checking if our dice in the same row has a match with the number of dice that dice number is inbetween them
                    Vector3Int horizontalPosCheck = new Vector3Int(position.x + (int)number + 1, position.y, 0);

                    //if the numbers are the same check if they are connected
                    bool hasHorizontalMatch = TilePos[position].number == TilePos[horizontalPosCheck].number;
                    bool hasHorizontalColorMatch = TilePos[position].color == TilePos[horizontalPosCheck].color;

                    if (hasHorizontalMatch && IsConnected(position, horizontalPosCheck, withinHorizontalBounds, BetweenDice))//&& we do the bounds check
                    {
                        Debug.Log($"<color=green>Match</color> pos {position} comparing with other position {horizontalPosCheck} the two numbers are {TilePos[position].number} and {TilePos[horizontalPosCheck].number}");

                        foreach(Vector3Int item in BetweenDice)
                        {
                            MatchedDice[item] = MatchedDice.ContainsKey(item) ? MatchedDice[item] + 1 : 0;
                            //MatchedDice.ContainsKey
                        }
                        ////if theyre the same color take all of them.
                        //if (!hasHorizontalColorMatch)
                        //{
                        //    MatchedDice[position] += 1;
                        //    MatchedDice[horizontalPosCheck] += 1;
                        //}

                        ////if they arent just take the beginning and end
                        //else
                        //{
                            
                        //}

                    }
                }
                if (withinVerticalBounds) {
                    Vector3Int verticalPosCheck = new Vector3Int(position.x, position.y + (int)number + 1, 0);

                    bool hasVerticalMatch = TilePos[position].number == TilePos[verticalPosCheck].number;
                    bool hashasVerticalColorMatch = TilePos[position].color == TilePos[verticalPosCheck].color;

                    if (hasVerticalMatch && IsConnected(position, verticalPosCheck, withinVerticalBounds, BetweenDice))
                    {
                        Debug.Log($"<color=green>Match</color> pos {position} comparing with other position {verticalPosCheck} the two numbers are {TilePos[position].number} and {TilePos[verticalPosCheck].number}");
                        foreach (Vector3Int item in BetweenDice)
                        {
     
                            MatchedDice[item] = MatchedDice.ContainsKey(item) ? MatchedDice[item] + 1 : 0;
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
            //this confused me because i forgot that if one dice is in the same line as a dice thats going to be remove it still counts as a chain
            //they don't always have to be bookends to count as a chain
            //Debug.Log($"<color=red>Matches In Dictionary </color>: {pair.Key} -> {pair.Value} and here is the number {(int)TilePos[pair.Key].number}");
        }
        //Debug.Break();
        RemoveTiles(MatchedDice);
    }

    void RemoveTiles(Dictionary<Vector3Int, int> MatchedDice)
    {
        foreach (KeyValuePair<Vector3Int, int> pair in MatchedDice) {
            //ToDO send the int to the animator for the special effects
            diceBoard.Clear(pair.Key);
            TilePos[pair.Key] = new DiceImprint(pair.Key);
        }

    }


    //TODO needs tlc
    bool IsConnected(Vector3Int position, Vector3Int checkingPosition, bool inBounds, List<Vector3Int>listOfDiceToRemove)
    {
        listOfDiceToRemove.Clear();
        if (!inBounds)
            return false;


        listOfDiceToRemove.Add(position);
        listOfDiceToRemove.Add(checkingPosition);
        bool sameColor = TilePos[position].color == TilePos[checkingPosition].color;

        //List<Vector3Int> inBetweenDice = new List<Vector3Int>();

        Vector3Int directionalCheck = position.x == checkingPosition.x ? new Vector3Int(0,-1,0)  : new Vector3Int(-1, 0, 0);

        while(position != checkingPosition)
        {
            checkingPosition += directionalCheck;
            if (!mainMap.HasTile(checkingPosition))
            {
                listOfDiceToRemove.Clear();
                return false;
            }

            if (sameColor)
                listOfDiceToRemove.Add(checkingPosition);

        }

        return true;
    }

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
                    Debug.Log(newPosition);
                    finishPos = newPosition;


                }
                //get final position
                MovingDiceInRow++;
                finishPos = new Vector3Int(finishPos.x, finishPos.y + MovingDiceInRow,0);
                //Debug.LogError($"Start {startPos} and finsh {finishPos}");
                if (mainMap.HasTile(finishPos))
                    continue;

                //TODO Need to get the dice group 
                //StartCoroutine(TravelingDice(currentTile, startPos, finishPos));
                StartCoroutine(TravelingDice(currentDice, startPos, finishPos));

                //Debug.Log($"--------------------------");



            }


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