using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DiceMatch : MonoBehaviour
{
    public DiceBoard diceBoard;
    [SerializeField] Tilemap mainMap;    
    [SerializeField] Grid grid;
    List<bool> TaskList = new List<bool>();
    public DiceFXController diceFXController;

    #region Grid Data
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
    bool taskIsRunning = false;
    Dictionary<Vector3Int, DiceImprint> TilePos;
    #endregion 

    const float GRAVITY = 0.045f;//0.045f;
    private int Chain;

    private void Awake()
    {
        Initialize();
    }

    void Update()
    {
        if (taskIsRunning)
        {
            if (TaskList.Contains(false))
                return;

            //Debug.Log("TurnOFFTaskManager");
            taskIsRunning = false;

            //read board again
            CheckForMatches();

        }

        if (Input.GetMouseButtonDown(0))
        {
            //MouseTileReader();
        }

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

    /// <summary>
    /// replaces the dice value in the tile pos dictionary
    /// </summary>
    /// <param name="Pos"></param>
    /// <param name="die"></param>
    public void SetTileDict(Vector3Int Pos, DiceImprint die)
    {
        TilePos[Pos] = die;
    }

    /// <summary>
    /// deletes the tiles that have a match
    /// </summary>
    /// <param name="Pos"></param>
    public void RemoveTileDict(Vector3Int Pos)
    {
        TilePos[Pos] = new DiceImprint(Pos);
    }

    public void Score()
    {
        Chain = 0;
        CheckForMatches();
    }

    /// <summary>
    /// this is the Main function of this script. It checks every tile and sees if there is a match. From there it handles the logic of which functions to call next
    /// </summary>
    void CheckForMatches()
    {
        bool hasMatch = false;
        //-3 -2 -1 0 1 2

        //3  (...)
        //2  (...)
        //1  (...)
        //0  (...)
        //-1 (...)
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

                //Do they have a pair in bounds
                bool withinHorizontalBounds = (position.x + (int)TilePos[position].number) < 2 ? true : false;
                bool withinVerticalBounds = (position.y + (int)TilePos[position].number) < 3 ? true : false;

                //get the number of the current position
                DiceNumber number = TilePos[position].number;

                //if the dice is zero go to the next tile
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

                    //if there is a horizontal match and they have a dice in between them...
                    if (hasHorizontalMatch && IsConnected(position, horizontalPosCheck, withinHorizontalBounds, BetweenDice))
                    {

                        //figure out if we need to loop through all this again because the board has changed
                        hasMatch = true;
                        //add the start position
                        BetweenDice.Add(position);
                        //add the matching dice
                        BetweenDice.Add(horizontalPosCheck);
                        //this log will tell you what the position and number of the matching dice is
                        //Debug.Log($"<color=green>Match</color> pos {position} comparing with other position {horizontalPosCheck} the two numbers are {TilePos[position].number} and {TilePos[horizontalPosCheck].number}");

                        //add the inbetween tiles and give them a value that corresponds with how often they occur
                        foreach (Vector3Int item in BetweenDice)
                        {
                            MatchedDice[item] = MatchedDice.ContainsKey(item) ? MatchedDice[item] + 1 : 1;
                        }
                    }
                }
                //we do the same with the vertial bounds as we did with the horizontal.
                if (withinVerticalBounds) {
                    Vector3Int verticalPosCheck = new Vector3Int(position.x, position.y + (int)number + 1, 0);

                    bool hasVerticalMatch = TilePos[position].number == TilePos[verticalPosCheck].number;
                    bool hashasVerticalColorMatch = TilePos[position].color == TilePos[verticalPosCheck].color;

                    if (hasVerticalMatch && IsConnected(position, verticalPosCheck, withinVerticalBounds, BetweenDice))
                    {
                        //figure out if we need to loop through all this again because the board has changed
                        hasMatch = true;
                        BetweenDice.Add(position);
                        BetweenDice.Add(verticalPosCheck);
                        //Debug.Log($"<color=green>Match</color> pos {position} comparing with other position {verticalPosCheck} the two numbers are {TilePos[position].number} and {TilePos[verticalPosCheck].number}");
                        foreach (Vector3Int item in BetweenDice)
                        {
                            MatchedDice[item] = MatchedDice.ContainsKey(item) ? MatchedDice[item] + 1 : 1;                            
                        }
                    }
                }

            }

        }

        #region Helper Log
        {
            /* this will tell you the tile that will be removed the level of animation and the tile number.
            foreach(KeyValuePair<Vector3Int, int>  pair in MatchedDice)
            {
                                                               //location to delete    //level of animation                 //tile number
                Debug.Log($"<color=red>Matches In Dictionary </color>: {pair.Key} -> {pair.Value} and here is the number {(int)TilePos[pair.Key].number}");
            }
            */
        }
        #endregion
        if (hasMatch)
        {
            //pass in the matched dice dictionary if there is a match otherwise we should continue playing
            RemoveTiles(MatchedDice);
            //keep track of how many times we've ran this for waterfall bonus
            Chain++;
            return;
        }
        //continue playing.
        diceBoard.SpawnGroup();
    }

    /// <summary>
    /// We pass in the dictionary of all the dice that need to be removed. the key is the location that needs to be removed and the value is
    /// how many chains that tile was involved with. At the endit applies gravity to the free floating tiles.
    /// </summary>
    /// <param name="MatchedDice"></param>
    void RemoveTiles(Dictionary<Vector3Int, int> MatchedDice)
    {
        //Debug.LogError("removing tiles");
        foreach (KeyValuePair<Vector3Int, int> pair in MatchedDice) {
            //TODO send the int to the animator for the special effects
            //-the key is the tile position to remove
            //-the value is how many chains is that tile included in

            //delete this position from the board
            diceBoard.Clear(pair.Key);

            //reset position to 0
            TilePos[pair.Key] = new DiceImprint(pair.Key);

            //make tasks then apply gravity once the tasks are finished
            //diceFXController.FX(DiceFXController.TileEffect.pop, pair.Key);
            diceFXController.FX(pair.Key);

        }
        Debug.LogError("APPLY GRAVITY");
        //apply the gravity
        ApplyGravity();
    }

    /// <summary>
    /// This sunction check to see if there is a tile in every cell between the tile you are at and that tiles number away. if it is the same color it will add it to the
    /// list of dice to remove. if it is not the same color it will just return that it is completely connected. We pass in the current position. The position that tiles number distance away.
    /// If that checked position is in bounds. The list that will gather which dice should also be removed if any.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="checkingPosition"></param>
    /// <param name="inBounds"></param>
    /// <param name="listOfDiceToRemove"></param>
    /// <returns></returns>
    bool IsConnected(in Vector3Int position, Vector3Int checkingPosition,in bool inBounds, List<Vector3Int>listOfDiceToRemove)
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

    /// <summary>
    /// This function just looks at all the empty spaces and pulls the tiles that are above them down so there are no more remaining gaps.
    /// </summary>
    void ApplyGravity()
    {
        TaskList.Clear();

        //start from the bottom
        for (int x =(int)XGridCell.One_Left; x<= (int)XGridCell.Six_Right; x++)
        {
            int MovingDiceInRow = -1;

            for (int y = (int)YGridCell.Nine_Bottom; y <= (int)YGridCell.One_Top; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
 
                //we can skip the tiles that are empty
                if (!mainMap.HasTile(position))//(y doesnt have a tile)
                    continue;

                Vector3Int startPos = position;
                Vector3Int finishPos= new Vector3Int();

                //we found a tile.. if there is another tile under it or we are at the bottom don't run
                //we are falling until we have a tile or we are at the bottom
                //falling is the position of the current dice


                DiceImprint currentDice = new DiceImprint(TilePos[position], position);

                for (int falling = position.y;(mainMap.HasTile(new Vector3Int(x,falling-1,0)) || (falling >= (int)YGridCell.Nine_Bottom)); falling--)
                {
                    Vector3Int newPosition = new Vector3Int(x, falling, 0);
                    finishPos = newPosition;
                }

                //get final position
                MovingDiceInRow++;
                finishPos = new Vector3Int(finishPos.x, finishPos.y + MovingDiceInRow,0);

                if (mainMap.HasTile(finishPos))
                    continue;

                TaskList.Add(false);
                StartCoroutine(TravelingDice(currentDice, startPos, finishPos));

            }
        }
        //Debug.Log("TurnOnTaskManager");
        taskIsRunning = true;
        diceBoard.SpawnGroup();//this needs to be removed at some point when i figure out how to do waterfall checks without coroutines getting in the way
    }

    /// <summary>
    /// This function pushes the remaining dice down from what you pass in as start to what you pass in as finish.
    /// </summary>
    /// <param name="die"></param>
    /// <param name="start"></param>
    /// <param name="finish"></param>
    /// <returns></returns>
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
        for(int i =0; i < TaskList.Count; i++)
        {
            if (!TaskList[i])
            {
                TaskList[i] = false;
                break;
            }
        }
        yield return null;
    }

    #region Test Functions

    private void MouseTileReader()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
        Vector3Int position = grid.WorldToCell(worldPoint);
        TileBase clickedTile = mainMap.GetTile(position);
        ReadValues(position);
    }

    void ReadValues(Vector3Int position)
    {
        Debug.Log($"At Position {TilePos[position].position} there is a value of {TilePos[position].number} and color {TilePos[position].color}");
    }

    void ReadValues()
    {
        foreach (KeyValuePair<Vector3Int, DiceImprint> entry in TilePos)
        {
            if ((int)entry.Value.number > 0)
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
    #endregion

}
